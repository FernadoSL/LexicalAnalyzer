﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using AnalisadorLexico.TiposToken;

namespace AnalisadorLexico
{
    public class Lexical
    {
        public int IdentifiersCounter { get; set; }
        
        public string CurrentScope { get; set; }

        public Lexical()
        {
            this.IdentifiersCounter = 1;
        }

        public List<Token> Analize(string filePath, Type tokenType)
        {
            List<BaseToken> result = new List<BaseToken>();

            FileStream file = new FileStream(filePath, FileMode.Open);
            using (var stream = new StreamReader(file))
            {
                string line = "";
                int lineNumber = 1;

                var tokens = GetAllTokens(tokenType);
                var tokenTypesOneChar = GetTokensOfSize(1, tokenType);

                while ((line = stream.ReadLine()) != null)
                {
                    this.GetIdentifiersAndConstants(line, tokens, result, lineNumber);

                    // C:\Users\alu2015110289\Documents\LexicalAnalyzer\miniJavaFactorial.mjar
                    
                    char[] lineCharArray = line.ToArray();
                    string word = string.Empty;
                    for (int i = 0; i < lineCharArray.Length; i++)
                    {
                        char currentChar = lineCharArray[i];
                        string currentTwoChar = string.Empty;
                        if (i + 1 < lineCharArray.Length - 1)
                            currentTwoChar = string.Concat(currentChar, lineCharArray[i + 1]);

                        if (currentChar != ' ')
                            word = string.Concat(word, currentChar);
                        else
                            word = string.Empty;

                        if (tokens.Contains(currentTwoChar))
                        {
                            AddToken(result, lineNumber, currentTwoChar);
                            word = string.Empty;
                        }

                        if (tokenTypesOneChar.Contains(currentChar.ToString()) && !tokens.Contains(currentTwoChar))
                        {
                            AddToken(result, lineNumber, currentChar.ToString());
                            word = string.Empty;
                        }

                        if (tokens.Contains(word))
                        {
                            AddToken(result, lineNumber, word);
                            word = string.Empty;
                        }
                    }

                    lineNumber++;
                }

                return result;
            }
        }

        private void GetIdentifiersAndConstants(string line, List<string> tokens, List<BaseToken> result, int lineNumber)
        {
            var splitedLine = line.Split(" ").ToList();
            var orderedTokens = tokens.OrderByDescending(t => t.Length);
            foreach (var token in orderedTokens)
            {
                splitedLine = string.Join(" ", splitedLine).Split(token).ToList();
            }
            splitedLine = string.Join("", splitedLine).Split(" ").ToList();
            splitedLine = splitedLine.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
            
            foreach (var token in splitedLine)
            {
                if (line.Contains(this.GetTokenString(TokenTypeMiniJava.SCLASS)))
                {
                    this.CurrentScope = splitedLine.FirstOrDefault();
                }

                if (line.Contains(this.GetTokenString(TokenTypeMiniJava.SINT)))
                {

                }

                bool isNumeric = int.TryParse(token, out int x);
                if (isNumeric)
                    AddToken(result, lineNumber, token, TokenTypeMiniJava.SCONSTANT);
                else
                    AddToken(result, lineNumber, token, TokenTypeMiniJava.SIDENTIFIER, this.IdentifiersCounter);

                this.IdentifiersCounter++;
            }
        }

        private void AddToken(List<BaseToken> result, int lineNumber, string word)
        {
            this.AddToken(result, lineNumber, word, this.GetTokenType(word));
        }

        private void AddToken(List<Token> result, int lineNumber, string word, TokenTypeMiniJava tokenType, int identifierCounter = 0)
        {
            var foundedToken = new Token() { Lexema = word, Linha = lineNumber, Tipo = tokenType, Id = identifierCounter, Scope = this.CurrentScope };
            result.Add(foundedToken);
        }

        public List<BaseToken> AnalizeV2(string filePath, Type tokenType)
        {
            int tokenMaxSize = 8;
            List<BaseToken> result = new List<BaseToken>();
            for (int tokenSize = tokenMaxSize; tokenSize >= 1; tokenSize--)
            {
                int lineNumber = 1;
                string line = string.Empty;
                List<string> allTokensOfSize = GetTokensOfSize(tokenSize, tokenType);

                FileStream file = new FileStream(filePath, FileMode.Open);
                using (var stream = new StreamReader(file))
                {
                    while ((line = stream.ReadLine()) != null)
                    {
                        char[] lineCharArray = line.ToArray();
                        string word = string.Empty;
                        for (int j = 0; j <= lineCharArray.Length; j++)
                        {
                            string currentChar = string.Empty;
                            if (j <= lineCharArray.Length - tokenSize)
                            {
                                for (int k = 0; k < tokenSize; k++)
                                {
                                    currentChar = string.Concat(currentChar, lineCharArray[j + k]);
                                }
                            }

                            if (allTokensOfSize.Contains(currentChar))
                            {
                                var foundedToken = new BaseToken() { Lexema = currentChar, Linha = lineNumber };
                                word = string.Empty;
                                result.Add(foundedToken);
                            }
                        }

                        lineNumber++;
                    }
                }
            }
            return result;
        }

        private List<string> GetTokensOfSize(int size, Type typeToken)
        {
            List<string> tokens = this.GetAllTokens(typeToken);
            var allTokensOfI = tokens.Where(t => t.Count() == size).ToList();

            return allTokensOfI;
        }

        private List<string> GetAllTokens(Type typeToken)
        {
            List<string> tokens = new List<string>();

            if (typeof(TokenTypeMiniJava) == typeToken)
            {
                Enum.GetValues(typeToken)
                    .Cast<TokenTypeMiniJava>()
                    .ToList()
                    .ForEach(tt => tokens.Add(GetTokenString(tt)));
            }
            else if (typeof(TokenTypeLdp) == typeToken)
            {
                Enum.GetValues(typeToken)
                    .Cast<TokenTypeLdp>()
                    .ToList()
                    .ForEach(tt => tokens.Add(GetTokenString(tt)));
            }

            return tokens;
        }

        private string GetTokenString(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        private TokenTypeMiniJava GetTokenType(string word)
        {
            var tokenList = Enum.GetValues(typeof(TokenTypeMiniJava))
                            .Cast<TokenTypeMiniJava>()
                            .ToList();

            foreach (var tipoToken in tokenList)
            {
                if (this.GetTokenString(tipoToken) == word)
                    return tipoToken;
            }

            return TokenTypeMiniJava.SABRE_PARENTESES;
        }
    }
}

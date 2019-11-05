using System;
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
        public List<Token> Analize(string filePath, Type tokenType)
        {
            List<Token> result = new List<Token>();

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

        private void GetIdentifiersAndConstants(string line, List<string> tokens, List<Token> result, int lineNumber)
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
                bool isNumeric = int.TryParse(token, out int x);
                if (isNumeric)
                    AddToken(result, lineNumber, token, TokenTypeMiniJava.SCONSTANT);
                else
                    AddToken(result, lineNumber, token, TokenTypeMiniJava.SIDENTIFIER);
            }
        }

        private void AddToken(List<Token> result, int lineNumber, string word)
        {
            this.AddToken(result, lineNumber, word, this.GetTokenType(word));
        }

        private void AddToken(List<Token> result, int lineNumber, string word, TokenTypeMiniJava tokenType)
        {
            var foundedToken = new Token() { Lexema = word, Linha = lineNumber, Tipo = tokenType };
            result.Add(foundedToken);
        }

        public List<Token> AnalizeV2(string filePath, Type tokenType)
        {
            int tokenMaxSize = 8;
            List<Token> result = new List<Token>();
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
                                var foundedToken = new Token() { Lexema = currentChar, Linha = lineNumber };
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

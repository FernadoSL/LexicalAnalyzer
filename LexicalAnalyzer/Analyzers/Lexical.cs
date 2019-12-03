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
        public int IdentifiersCounter { get; set; }
        
        public string CurrentScope { get; set; }

        public Lexical()
        {
            this.IdentifiersCounter = 1;
        }

        public List<BaseToken> Analize(string filePath, Type tokenType)
        {
            List<BaseToken> result = new List<BaseToken>();

            FileStream file = new FileStream(filePath, FileMode.Open);
            using (var stream = new StreamReader(file))
            {
                string actualScope = string.Empty;
                string line = "";
                int lineNumber = 1;

                var tokens = GetAllTokens(tokenType);
                var tokenTypesOneChar = GetTokensOfSize(1, tokenType);

                while ((line = stream.ReadLine()) != null)
                {
                    actualScope = this.GetScope(actualScope, line);
                    this.GetIdentifiersAndConstants(line, tokens, result, lineNumber, actualScope);
                    
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

        private string GetScope(string actualScope, string line)
        {
            if (line.Contains(this.GetTokenString(TokenTypeMiniJava.SCLASS)))
            {
                actualScope = line.Split(" ")[1];
            }

            return actualScope;
        }

        private void GetIdentifiersAndConstants(string line, List<string> tokens, List<BaseToken> result, int lineNumber, string scope)
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

                bool isNumeric = int.TryParse(token, out int x);
                if (isNumeric)
                    AddToken(result, lineNumber, token, TokenTypeMiniJava.SCONSTANT, this.CurrentScope);
                else
                {
                    string type = string.Empty;
                    var identifierList = line.Replace("(", " ").Replace(")", " ").Replace("{", " ").Replace("}", " ").Replace(";", " ").Split(" ").ToList();
                    int preIndexIdentifier = identifierList.IndexOf(token) - 1;

                    if (preIndexIdentifier >= 0)
                        type = identifierList.ToArray()[preIndexIdentifier];

                    var addedToken = AddToken(result, lineNumber, token, TokenTypeMiniJava.SIDENTIFIER, this.CurrentScope, type);

                    this.VerifySysOutIdentifier(line, identifierList, addedToken);

                    addedToken.IsMain = token.Equals("main");
                    addedToken.IsMethod = addedToken.IsMain || (preIndexIdentifier - 1 >= 0 && identifierList.ToArray()[preIndexIdentifier - 1].Equals(this.GetTokenString(TokenTypeMiniJava.SPUBLIC)));

                    if (addedToken.IsMethod)
                        this.CurrentScope = addedToken.Lexema;
                }
            }
        }

        private void VerifySysOutIdentifier(string line, List<string> identifierList, BaseToken addedToken)
        {
            string identiFierConsoleOut = "System.out.println";
            if (identifierList.Contains(identiFierConsoleOut))
            {
                string printParameter = line.Replace(identiFierConsoleOut + "(", "").Replace(");", "").Trim();

                addedToken.PrintParameter = printParameter;
            }
        }

        private void AddToken(List<BaseToken> result, int lineNumber, string word)
        {
            this.AddToken(result, lineNumber, word, this.GetTokenType(word));
        }

        private BaseToken AddToken(List<BaseToken> result, int lineNumber, string word, TokenTypeMiniJava tokenType, string scope = "", string type = "")
        {
            var foundedToken = new BaseToken() { Lexema = word, Linha = lineNumber, TokenType = tokenType, Scope = scope, Type = type };
            result.Add(foundedToken);

            return foundedToken;
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

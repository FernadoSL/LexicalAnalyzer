using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace AnalisadorLexico
{
    public class Lexical
    {
        public List<Token> Analize(string filePath)
        {
            List<Token> result = new List<Token>();

            FileStream file = new FileStream(filePath, FileMode.Open);
            using (var stream = new StreamReader(file))
            {
                string line = "";
                int lineNumber = 1;

                var tokens = GetAllTokens();
                var tokenTypesOneChar = GetTokensOfSize(1);

                while ((line = stream.ReadLine()) != null)
                {
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

        private void AddToken(List<Token> result, int lineNumber, string word)
        {   
            var foundedToken = new Token() { Lexema = word, Linha = lineNumber, Tipo = this.GetTokenType(word) };   
            result.Add(foundedToken);
        }

        private void AddToken(List<Token> result, int lineNumber, string word, TokenTypeLdp tipoToken)
        {
            var foundedToken = new Token() { Lexema = word, Linha = lineNumber, Tipo = tipoToken };
            result.Add(foundedToken);
        }

        public List<Token> AnalizeV2(string filePath)
        {
            int tokenMaxSize = 8;
            List<Token> result = new List<Token>();
            for (int tokenSize = tokenMaxSize; tokenSize >= 1; tokenSize--)
            {
                int lineNumber = 1;
                string line = string.Empty;
                List<string> allTokensOfSize = GetTokensOfSize(tokenSize);

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

        private List<string> GetTokensOfSize(int size)
        {
            List<string> tokens = this.GetAllTokens();
            var allTokensOfI = tokens.Where(t => t.Count() == size).ToList();

            return allTokensOfI;
        }

        private List<string> GetAllTokens()
        {
            List<string> tokens = new List<string>();
            Enum.GetValues(typeof(TokenTypeLdp))
                .Cast<TokenTypeLdp>()
                .ToList()
                .ForEach(tt => tokens.Add(GetTokenString((TokenTypeLdp)tt)));

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

        private TokenTypeLdp GetTokenType(string word)
        {
            var tokenList = Enum.GetValues(typeof(TokenTypeLdp))
                            .Cast<TokenTypeLdp>()
                            .ToList();

            foreach (var tipoToken in tokenList)
            {
                if (this.GetTokenString(tipoToken) == word)
                    return tipoToken;
            }

            return TokenTypeLdp.SABRE_PARENTESIS;
        }
    }
}

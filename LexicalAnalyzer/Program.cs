using AnalisadorLexico.TiposToken;
using LexicalAnalyzer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalisadorLexico
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = string.Empty;
            if (args.Any())
            {
                filePath = args[0];
            }
            else
            {
                Console.WriteLine("Input file path:");
                filePath = Console.ReadLine();
            }
            
            Lexical lexico = new Lexical();
            List<Token> tokenList = lexico.Analize(filePath, typeof(TokenTypeLdp));

            Console.WriteLine("Número de tokens: " + tokenList.Count);
            foreach (var token in tokenList.OrderBy(t => t.Linha))
            {
                Console.WriteLine($"Lexema: {token.Lexema} \nLinha: {token.Linha} \nTipoToken: {token.Tipo} \n---");
            }
        }
    }
}

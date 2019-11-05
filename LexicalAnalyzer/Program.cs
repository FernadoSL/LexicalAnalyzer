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

            RecursivePredictiveParser parser = new RecursivePredictiveParser();

            // Get Tokens
            List<Token> tokenList = parser.LexicalAnalyzer.Analize(filePath, typeof(TokenTypeMiniJava));
            
            // Populate Symbol Table
            foreach (var token in tokenList.Where(t => 
                t.Tipo == TokenTypeMiniJava.SIDENTIFIER || t.Tipo == TokenTypeMiniJava.SCONSTANT))
            {
                parser.SymbolTable.TokenTable.TryAdd(token.Lexema, token);
            }

            // Print Results
            Console.WriteLine("Número de tokens: " + tokenList.Count);
            foreach (var token in tokenList.OrderBy(t => t.Linha))
            {
                Console.WriteLine($"Lexema: {token.Lexema} \nLinha: {token.Linha} \nTipoToken: {token.Tipo} \n---");
            }

            Console.WriteLine("Tabela de Símbolos: ");
            foreach (var simbolo in parser.SymbolTable.TokenTable)
            {
                Console.WriteLine(simbolo.Key + " " + simbolo.Value.Linha);
            }
        }
    }
}

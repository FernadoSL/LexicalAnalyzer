using AnalisadorLexico.TiposToken;
using LexicalAnalyzer.Entities;
using LexicalAnalyzer.Entities.CodeGenerator;
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
            List<BaseToken> tokenList = parser.LexicalAnalyzer.Analize(filePath, typeof(TokenTypeMiniJava));
            
            // Populate Symbol Table
            foreach (var token in tokenList.Where(t => 
                t.TokenType == TokenTypeMiniJava.SIDENTIFIER || t.TokenType == TokenTypeMiniJava.SCONSTANT))
            {
                parser.SymbolTable.TokenTable.TryAdd(token.Lexema, token);
            }

            // Print Results
            Console.WriteLine("Número de tokens: " + tokenList.Count);
            foreach (var token in tokenList.OrderBy(t => t.Linha))
            {
                Console.WriteLine($"Lexema: {token.Lexema} \nLinha: {token.Linha} \nTipoToken: {token.TokenType} \n---");
            }

            Console.WriteLine("Tabela de Símbolos: \n---");
            foreach (var simbolo in 
                parser.SymbolTable.TokenTable
                .Where(t => t.Value.IsMain || t.Value.IsMethod || t.Value.IsVariable)
                .OrderBy(t => t.Value.IsMethod))
            {
                Console.WriteLine(
                    $"Lexema: {simbolo.Key } \n" +
                    $"Escopo: {simbolo.Value.Scope} \n" +
                    $"Tipo: {simbolo.Value.Type} \n" +
                    $"É Método: {simbolo.Value.IsMethod} \n" +
                    $"É Variável: {simbolo.Value.IsVariable} \n" +
                    $"---"
                );
            }

            var codeGenerator = new CCodeGenerator();
            Console.WriteLine(codeGenerator.CreateHelloWorld());

            Console.ReadKey();
            //C:\Users\Gavb-Fernando\Documents\repos\LexicalAnalyzer\miniJavaFactorial.mjar
        }
    }
}

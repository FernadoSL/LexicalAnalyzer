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
            int attributionCount = 1;
            int printCount = 1;
            foreach (var token in 
                tokenList.Where(t => t.TokenType == TokenTypeMiniJava.SIDENTIFIER || 
                                     t.TokenType == TokenTypeMiniJava.SCONSTANT ||
                                     t.IsAttribution))
            {
                string key = token.Lexema + "-" + token.Scope;
                if (token.IsAttribution)
                {
                    key = key + "-" + attributionCount;
                    attributionCount++;
                }
                else if (token.Lexema.Equals("ln"))
                {
                    key = key + "-" + printCount;
                    printCount++;
                }

                bool success = parser.SymbolTable.TokenTable.TryAdd(key, token);
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
                //.Where(t => t.Value.IsMain || t.Value.IsMethod || t.Value.IsVariable)
                .OrderBy(t => t.Value.IsMethod))
            {
                Console.WriteLine(
                    $"Lexema: {simbolo.Value.Lexema } \n" +
                    $"Linha: {simbolo.Value.Linha} \n" + 
                    $"Escopo: {simbolo.Value.Scope} \n" +
                    $"Tipo: {simbolo.Value.Type} \n" +
                    $"É Método: {simbolo.Value.IsMethod} \n" +
                    $"É Variável: {simbolo.Value.IsVariable} \n" +
                    $"---"
                );
            }

            var codeGenerator = new CCodeGenerator();
            //Console.WriteLine(codeGenerator.CreateHelloWorld());
            Console.WriteLine(codeGenerator.CreateProgramFromSymbolTable(parser.SymbolTable));

            Console.ReadKey();
            //C:\Users\Gavb-Fernando\Documents\repos\LexicalAnalyzer\miniJavaFactorial.mjar
            //C:\Users\FERNANDO\Documents\PortiRepositório\Ciências Da Computação\Compiladores\LexicalAnalyzer\miniJavaFactorial.mjar
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalisadorLexico
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexical lexico = new Lexical();
            List<Token> tokenList = lexico.Analize(args[0]);

            Console.WriteLine("Número de tokens: " + tokenList.Count);
            foreach (var token in tokenList.OrderBy(t => t.Linha))
            {
                Console.WriteLine($"Lexema: {token.Lexema} \nLinha: {token.Linha} \nTipoToken: {token.Tipo} \n---");
            }
        }
    }
}
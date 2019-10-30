using AnalisadorLexico;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalyzer.Entities
{
    public abstract class Parser
    {
        public SymbolTable SymbolTable { get; set; }

        public Lexical LexicalAnalyzer { get; set; }

        public Token Token { get; set; }

        public Parser()
        {

        }
    }
}

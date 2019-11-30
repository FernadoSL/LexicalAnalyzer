using AnalisadorLexico;

namespace LexicalAnalyzer.Entities
{
    public abstract class Parser
    {
        public SymbolTable SymbolTable { get; set; }

        public Lexical LexicalAnalyzer { get; set; }

        public BaseToken Token { get; set; }
    }
}

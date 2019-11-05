namespace LexicalAnalyzer.Entities
{
    public class RecursivePredictiveParser : Parser
    {
        public RecursivePredictiveParser()
        {
            this.LexicalAnalyzer = new AnalisadorLexico.Lexical();
            this.SymbolTable = new SymbolTable();
        }
    }
}

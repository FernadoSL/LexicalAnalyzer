using AnalisadorLexico;

namespace LexicalAnalyzer.Entities.Tokens
{
    public class IdentifierToken : BaseToken
    {
        public string Scope { get; set; }

        public string Valor { get; set; }

        public string Endereco { get; set; }

        public string TipoDado { get; set; }
    }
}

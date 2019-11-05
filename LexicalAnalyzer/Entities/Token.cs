using AnalisadorLexico.TiposToken;

namespace AnalisadorLexico
{
    public class Token
    {
        public TokenTypeMiniJava Tipo { get; set; }

        public string Lexema { get; set; }

        public int Linha { get; set; }

        public int Coluna { get; set; }
    }
}

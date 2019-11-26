using AnalisadorLexico.TiposToken;

namespace AnalisadorLexico
{
    public class Token
    {
        public TokenTypeMiniJava Tipo { get; set; }

        public string Lexema { get; set; }

        public int Linha { get; set; }

        public int Coluna { get; set; }

        public int Id { get; set; }

        public string Scope { get; set; }

        public string DataType { get; set; }
    }
}

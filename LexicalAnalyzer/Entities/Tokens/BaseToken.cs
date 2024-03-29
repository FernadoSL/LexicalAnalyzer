﻿using AnalisadorLexico.TiposToken;

namespace AnalisadorLexico
{
    public class BaseToken
    {
        public TokenTypeMiniJava TokenType { get; set; }

        public string Lexema { get; set; }

        public int Linha { get; set; }

        public int Coluna { get; set; }

        public int Id { get; set; }

        public string Scope { get; set; }

        public string Type { get; set; }

        public bool IsMain { get; set; }

        public  bool IsVariable 
        { 
            get
            {
                return !this.IsMethod && !string.IsNullOrEmpty(this.Type) && !this.Type.Equals("new");
            }
        }

        public bool IsMethod { get; set; }

        public string PrintParameter { get; set; }

        public bool IsAttribution { get; set; }

        public string AttributionVariable { get; set; }

        public string AttributionValue { get; set; }
    }
}

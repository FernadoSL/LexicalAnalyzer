using AnalisadorLexico;
using System.Collections.Generic;

namespace LexicalAnalyzer.Entities
{
    public class SymbolTable
    {
        public Dictionary<string, BaseToken> TokenTable {get;set;}

        public SymbolTable()
        {
            this.TokenTable = new Dictionary<string, BaseToken>();
        }

        public void SetAttribute(string key, string attribute, string valor)
        {
            var token = this.TokenTable[key];
            var field = token.GetType().GetProperty(attribute);

            field.SetValue(token, valor);
        }
    }
}

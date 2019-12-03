using System.Collections.Generic;
using System.Linq;

namespace LexicalAnalyzer.Entities.CodeGenerator
{
    public class CCodeGenerator
    {
        public string CreateHelloWorld()
        {
            string main = this.CreateMain();
            string print = this.CreatePrint("Hello World");

            string result = InsertIntoMain(main, print);
            string resultFormated = this.FormatNewLine(result);

            return this.RemoveReservedWords(resultFormated);
        }

        public string CreateProgramFromSymbolTable(SymbolTable symbolTable)
        {
            string code = string.Empty;
            code = CreateFunctionsFromSymbolTable(symbolTable, code);

            string codeFormated = this.FormatNewLine(code);

            return this.RemoveReservedWords(codeFormated);
        }

        private string CreateFunctionsFromSymbolTable(SymbolTable symbolTable, string main)
        {
            foreach (var token in symbolTable.TokenTable.Where(t => t.Value.IsMethod))
            {
                string type = token.Value.Lexema.Equals("main") ? "int" : token.Value.Type;
                string function = this.CreateFunction(type, token.Value.Lexema, "0");
                function = InsertStatementsInFunction(symbolTable, token, function);

                main = $"{function} \n\n{main}";
            }

            return main;
        }

        private string InsertStatementsInFunction(SymbolTable symbolTable, KeyValuePair<string, AnalisadorLexico.BaseToken> token, string function)
        {
            foreach (var tokenInFunction in symbolTable.TokenTable.Where(t => t.Value.Scope.Equals(token.Value.Lexema)).OrderByDescending(t => t.Value.Linha))
            {
                if (tokenInFunction.Value.IsVariable)
                {
                    string variableInFunction = this.CreateVariable(tokenInFunction.Value.Type, tokenInFunction.Value.Lexema);
                    function = this.InsertIntoMain(function, variableInFunction);
                }

                if (tokenInFunction.Value.Lexema.Equals("ln"))
                {
                    string printInFunction = this.CreatePrint(tokenInFunction.Value.PrintParameter);
                    function = this.InsertIntoMain(function, printInFunction);
                }
            }

            return function;
        }

        public string CreateMain()
        {
            return "int main() { return 0;}";
        }

        public string CreatePrint(string parameter)
        {
            return $"printf({parameter});";
        }

        public string CreateVariable(string dataType, string variableName)
        {
            if (dataType.Contains("String"))
                dataType = dataType.Replace("String", "char[100]");

            if (dataType.Contains("[]"))
                dataType = dataType.Replace("[]", "[100]");

            return $"{dataType} {variableName};";
        }

        public string CreateVariable(string dataType, string variableName, string initialValue)
        {
            return $"{dataType} {variableName} = {initialValue};";
        }

        public string CreateAttribution(string variableName, string value)
        {
            return $"{variableName} = {value};";
        }

        public string CreateFunction(string returnType, string functionName, string returnValue)
        {
            return string.Format("{0} {1}()", returnType, functionName) + " { \n" + " return " + returnValue + ";" + "}";
        }

        public string InsertIntoMain(string mainFunction, string newCode)
        {
            List<string> mainSplited = mainFunction.Split("{").ToList();
            mainSplited.Insert(1, " { ");
            mainSplited.Insert(2, newCode);

            string result = string.Join("", mainSplited);

            return result;
        }

        public string FormatNewLine(string code)
        {
            string formatedCode = code.Replace("{", "{\n").Replace(";", ";\n");

            return formatedCode;
        }

        public string RemoveReservedWords(string code)
        {
            List<string> reservedWords = new List<string>()
            {
                "new Fac()."
            };

            foreach (var reservedWord in reservedWords)
            {
                code = code.Replace(reservedWord, "");
            }

            return code;
        }
    }
}

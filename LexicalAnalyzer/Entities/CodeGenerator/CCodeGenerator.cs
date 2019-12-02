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
            return result;
        }

        public string CreateMain()
        {
            return "int main() { return 0; }";
        }

        public string CreatePrint(string parameter)
        {
            return $"printf({parameter})";
        }

        public string CreateVariable(string dataType, string variableName)
        {
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

        public string InsertIntoMain(string mainFunction, string newCode)
        {
            List<string> mainSplited = mainFunction.Split("{").ToList();
            mainSplited.Insert(1, " { ");
            mainSplited.Insert(2, newCode);

            string result = string.Join("", mainSplited);

            return result;
        }
    }
}

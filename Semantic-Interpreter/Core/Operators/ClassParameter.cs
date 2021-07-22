using Semantic_Interpreter.Core.Operators;

namespace Semantic_Interpreter.Core.Items
{
    public class ClassParameter : IHaveName
    {
        public ClassParameter(string name) => Name = name;
        
        public string Name { get; set; }
    }
}
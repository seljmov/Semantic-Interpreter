using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Module : MultilineOperator
    {
        public Module(string name)
        {
            Name = name;
            OperatorID = GenerateOperatorId();
        }

        public readonly VariableStorage VariableStorage = new();
        public readonly FunctionStorage FunctionStorage = new();
        
        public string Name { get; }
        public sealed override string OperatorID { get; set; }
        
        public override void Execute() { }
    }
}
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Module : MultilineOperator
    {
        public Module(string name)
        {
            Name = name;
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        public readonly FunctionStorage FunctionStorage = new();
        
        public string Name { get; }
        public sealed override string OperatorId { get; set; }
        
        public sealed override BlockSemanticOperator Operators { get; set; }

        public override void Execute() { }
    }
}
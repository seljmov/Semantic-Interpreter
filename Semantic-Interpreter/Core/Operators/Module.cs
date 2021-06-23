using Semantic_Interpreter.Core;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Module : MultilineOperator
    {
        public Module(string name)
        {
            Start = null;
            Name = name;
            OperatorID = GenerateOperatorId();
        }

        public VariableStorage VariableStorage = new();
        
        private Start Start { get; set; }
        public string Name { get; }
        public override string OperatorID { get; set; }

        public void SetStart(Start start)
        {
            Start = start;
            Start.Parent = this;
            Child = Start;
        }
        
        public override void Execute() { }
    }
}
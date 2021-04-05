using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Core
{
    public class Module : MultilineOperator
    {
        public Module(string name, BlockSemanticOperator blockSemanticOperator)
        {
            Beginning = null;
            Name = name;
            BlockSemanticOperator = blockSemanticOperator;
        }
        
        private Beginning Beginning { get; set; }
        private string Name { get; set; }
        private BlockSemanticOperator BlockSemanticOperator { get; set; }

        public void SetBeginning(Beginning beginning)
        {
            Beginning = beginning;
            Beginning.Parent = this;
            BlockSemanticOperator.Add(beginning);
        }
        
        public void Execute()
        {
        }

        public override string ToString() => "module: " + Name;
    }
}
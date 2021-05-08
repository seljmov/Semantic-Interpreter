using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Core
{
    public class Module : SemanticOperator
    {
        public Module(string name)
        {
            Beginning = null;
            Name = name;
        }
        
        private Beginning Beginning { get; set; }
        public string Name { get; set; }

        public void SetBeginning(Beginning beginning)
        {
            Beginning = beginning;
            Beginning.Parent = this;
            Child = Beginning;
        }
        
        public override void Execute()
        {
        }

        public override string ToString() => "module: " + Name;
    }
}
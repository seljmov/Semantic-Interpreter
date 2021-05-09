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
        
        // TODO: Нужно ли?
        private Beginning Beginning { get; set; }
        public string Name { get; }

        public void SetBeginning(Beginning beginning)
        {
            Beginning = beginning;
            Beginning.Parent = this;
            Child = Beginning;
        }
        
        public override void Execute() { }
    }
}
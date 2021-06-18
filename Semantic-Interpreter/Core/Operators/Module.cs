using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Core
{
    public class Module : SemanticOperator
    {
        public Module(string name)
        {
            Start = null;
            Name = name;
        }
        
        private Start Start { get; set; }
        public string Name { get; }

        public void SetStart(Start start)
        {
            Start = start;
            Start.Parent = this;
            Child = Start;
        }
        
        public override void Execute() { }
    }
}
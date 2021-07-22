namespace Semantic_Interpreter.Core
{
    public abstract class SemanticOperator
    {
        public SemanticOperator Parent { get; set; }
        public SemanticOperator Child { get; set; }
        public SemanticOperator Next { get; set; }
        public SemanticOperator Previous { get; set; }
        
        public abstract void Execute();
        
        public Module FindRoot()
        {
            var curr = Parent;
            while (curr.Parent != null && !(curr is Module))
            {
                curr = curr.Parent;
            }

            var module = (Module) curr;
            return module;
        }
    }
}
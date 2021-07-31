namespace Semantic_Interpreter.Core
{
    public abstract class SemanticOperator
    {
        public SemanticOperator Parent { get; set; }
        public SemanticOperator Child { get; set; }
        public SemanticOperator Next { get; set; }
        public SemanticOperator Previous { get; set; }
        
        public abstract void Execute();
        
        public Root GetRoot()
        {
            var curr = Parent;
            while (curr.Parent != null && !(curr is Root))
            {
                curr = curr.Parent;
            }

            var root = (Root) curr;
            return root;
        }
    }
}
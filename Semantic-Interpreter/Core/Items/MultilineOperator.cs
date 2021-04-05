namespace Semantic_Interpreter.Core
{
    public abstract class MultilineOperator : ISemanticOperator
    {
        public ISemanticOperator Parent { get; set; }
        public ISemanticOperator Next { get; set; }
        public ISemanticOperator Previous { get; set; }
        public BlockSemanticOperator Block { get; set; }
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
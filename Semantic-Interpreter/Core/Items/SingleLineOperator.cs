namespace Semantic_Interpreter.Core.Items
{
    public abstract class SingleLineOperator : ISemanticOperator
    {
        public ISemanticOperator Parent { get; set; }
        public ISemanticOperator Next { get; set; }
        public ISemanticOperator Previous { get; set; }
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
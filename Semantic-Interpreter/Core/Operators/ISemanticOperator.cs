namespace Semantic_Interpreter.Core
{
    public interface ISemanticOperator
    {
        public ISemanticOperator Parent { get; set; }
        public ISemanticOperator Next { get; set; }
        public ISemanticOperator Previous { get; set; }
        
        public void Execute();
    }
}
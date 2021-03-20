namespace Semantic_Interpreter.Items
{
    /// <summary>
    ///     Интерфейс всех операторов
    /// </summary>
    public interface ISemanticOperator
    {
        public ISemanticOperator Parent { get; internal set; }
        public ISemanticOperator Child { get; internal set; }
        public ISemanticOperator Next { get; internal set; }
        public ISemanticOperator Previous { get; internal set; }
    }
}
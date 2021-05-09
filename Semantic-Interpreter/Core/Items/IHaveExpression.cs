namespace Semantic_Interpreter.Core
{
    // TODO: Нужно ли?
    public interface IHaveExpression : IExpression
    {
        public IExpression Expression { get; set; }
    }
}
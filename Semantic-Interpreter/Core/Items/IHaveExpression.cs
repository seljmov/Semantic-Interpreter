namespace Semantic_Interpreter.Core
{
    public interface IHaveExpression : IExpression
    {
        public IExpression Expression { get; set; }
    }
}
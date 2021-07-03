namespace Semantic_Interpreter.Core.Items
{
    public interface ICalculated
    {
        public IExpression Expression { get; set; }

        public IValue GetValue();
    }
}
namespace Semantic_Interpreter.Core.Items
{
    public interface IFunction
    {
        IValue Execute(params IValue[] args);
    }
}
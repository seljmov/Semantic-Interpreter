namespace Semantic_Interpreter.Core.Items
{
    public interface IFunction
    {
        Value Execute(params Value[] args);
    }
}
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Parser.Expressions
{
    public interface IExpression
    {
        IValue Eval();
    }
}
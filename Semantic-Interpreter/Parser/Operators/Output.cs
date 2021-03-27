using System;
using Semantic_Interpreter.Parser.Expressions;

namespace Semantic_Interpreter.Parser.Operators
{
    public class Output : IOperator
    {
        public Output(IExpression expression) => Expression = expression;
        
        public IExpression Expression { get; set; }

        public void Execute() => Console.Write(Expression.Eval());

        public override string ToString() => $"output: {Expression}";
    }
}
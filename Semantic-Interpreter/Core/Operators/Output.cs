using System;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class Output : SingleLineOperator
    {
        public Output(IExpression expression) => Expression = expression;
        
        public IExpression Expression { get; set; }

        public void Execute() => Console.Write(Expression.Eval());

        public override string ToString() => $"output: {Expression}";
    }
}
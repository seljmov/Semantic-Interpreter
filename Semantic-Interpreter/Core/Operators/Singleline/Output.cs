using System;

namespace Semantic_Interpreter.Core
{
    public class Output : SemanticOperator
    {
        public Output(IExpression expression) 
            => Expression = expression;
        
        public IExpression Expression { get; }

        public override void Execute() 
            => Console.Write(Expression.Eval());
    }
}
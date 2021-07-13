using System;

namespace Semantic_Interpreter.Core
{
    public class Return : SemanticOperator
    {
        public Return(IExpression expression)
        {
            Expression = expression;
        }
        
        public IExpression Expression { get; set; }
        public IValue Result { get; set; }
        
        public override void Execute()
        {
            Result = Expression.Eval();

            var curr = Parent;
            while (!(curr is Function))
            {
                curr = curr.Parent;
            }

            ((Function) curr).Return = this;

            throw new Exception();
        }
    }
}
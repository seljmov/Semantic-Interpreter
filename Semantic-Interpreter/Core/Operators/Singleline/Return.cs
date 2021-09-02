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
        public Value Result { get; set; }
        
        public override void Execute()
        {
            Result = Expression.Eval();

            var curr = Parent;
            while (!(curr is Function) && !(curr is MethodFunction))
            {
                curr = curr.Parent;
            }

            switch (curr)
            {
                case Function function:
                    function.Return = this;
                    break;
                case MethodFunction methodFunction:
                    methodFunction.Return = this;
                    break;
            }
            
            throw new Exception();
        }
    }
}
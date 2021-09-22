using System;
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class ArrayAccessExpression : IExpression
    {
        public ArrayAccessExpression(List<IExpression> indexes, IHaveExpression haveExpression)
        {
            Indexes = indexes;
            HaveExpression = haveExpression;
        }
        
        private List<IExpression> Indexes { get; }
        private IHaveExpression HaveExpression { get; set; }

        public Value Eval()
        {
            if (HaveExpression.Expression is not ArrayExpression)
            {
                if (HaveExpression.Expression.Eval() is ArrayValue arrayValue)
                {
                    HaveExpression.Expression = new ArrayExpression(arrayValue);
                } 
                else throw new Exception("Что-то пошло не так...");
            }

            return ((ArrayExpression) HaveExpression.Expression).Get(Indexes);
        }
    }
}
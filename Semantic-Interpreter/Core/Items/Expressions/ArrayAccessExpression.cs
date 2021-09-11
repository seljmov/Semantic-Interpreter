using System;
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class ArrayAccessExpression : IExpression
    {
        public ArrayAccessExpression(List<IExpression> indexes, IExpression expression)
        {
            Indexes = indexes;
            Expression = expression;
        }
        
        private List<IExpression> Indexes { get; }
        private IExpression Expression { get; set; }

        public Value Eval()
        {
            if (Expression is not ArrayExpression)
            {
                if (Expression.Eval() is ArrayValue arrayValue)
                {
                    Expression = new ArrayExpression(arrayValue);
                } 
                else throw new Exception("Что-то пошло не так...");
            }

            return ((ArrayExpression) Expression).Get(Indexes);
        }
    }
}
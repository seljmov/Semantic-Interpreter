using System;
using System.Collections.Generic;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class InterpolationExpression : IExpression
    {
        public InterpolationExpression(List<IExpression> expressions)
        {
            _expressions = expressions;
        }
        
        private readonly List<IExpression> _expressions;
        
        public Value Eval()
        {
            var result = "";
            
            _expressions.ForEach(expression => result += expression.Eval().AsString());

            return new StringValue(result);
        }
    }
}
using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public abstract class Expression : IExpression
    {
        public abstract IValue Eval();

        protected static IExpression FormatExpression(IExpression expression)
        {
            if (expression is Variable variable)
            {
                return variable.Type switch
                {
                    SemanticTypes.Integer => new ValueExpression(variable.Eval().AsInteger()),
                    SemanticTypes.Real => new ValueExpression(variable.Eval().AsReal()),
                    SemanticTypes.Boolean => new ValueExpression(variable.Eval().AsInteger()),
                    SemanticTypes.String => new ValueExpression(variable.Eval().AsString()),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return expression;
        }
    }
}
using System;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class Output : SemanticOperator
    {
        public Output(IExpression expression) => Expression = expression;
        
        public IExpression Expression { get; set; }

        public override void Execute() => Console.Write(FormatExpression(Expression).Eval());

        public override string ToString() => $"output: {Expression}";
        
        private IExpression FormatExpression(IExpression expression)
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
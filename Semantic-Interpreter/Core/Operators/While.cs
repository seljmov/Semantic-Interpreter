using System;

namespace Semantic_Interpreter.Core
{
    public class While : SemanticOperator
    {
        public While(IExpression expression, BlockSemanticOperator @operator)
        {
            Expression = expression;

            foreach (var op in @operator.Operators)
            {
                op.Parent = this;
            }
            
            Operator = @operator;
            
        }
        
        private IExpression Expression { get; set; }
        private SemanticOperator Operator { get; set; }
        
        public override void Execute()
        {
            var cond = Expression.Eval().AsInteger();
            while (cond != 0)
            {
                Operator.Execute();
                cond = Expression.Eval().AsInteger();
            }
        }
    }
}
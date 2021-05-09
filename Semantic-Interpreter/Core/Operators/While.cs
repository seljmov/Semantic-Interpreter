using System;

namespace Semantic_Interpreter.Core
{
    public class While : SemanticOperator
    {
        public While(IExpression expression, BlockSemanticOperator operators)
        {
            Expression = expression;

            foreach (var op in operators.Operators)
            {
                op.Parent = this;
            }
            
            Operators = operators;
        }
        
        public IExpression Expression { get; }
        public BlockSemanticOperator Operators { get; }
        
        public override void Execute()
        {
            var cond = Expression.Eval().AsInteger();
            while (cond != 0)
            {
                Operators.Execute();
                cond = Expression.Eval().AsInteger();
            }
        }
    }
}
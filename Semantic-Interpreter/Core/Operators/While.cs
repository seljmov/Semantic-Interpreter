using System;

namespace Semantic_Interpreter.Core
{
    public class While : MultilineOperator
    {
        public While(IExpression expression, BlockSemanticOperator operators)
        {
            Expression = expression;

            foreach (var op in operators.Operators)
            {
                op.Parent = this;
            }
            
            Operators = operators;
            OperatorID = GenerateOperatorID();
        }
        
        public IExpression Expression { get; }
        public BlockSemanticOperator Operators { get; }
        
        public override string OperatorID { get; set; }
        
        public override void Execute()
        {
            var result = Expression.Eval().AsBoolean();
            while (result)
            {
                Operators.Execute();
                result = Expression.Eval().AsBoolean();
            }
        }
    }
}
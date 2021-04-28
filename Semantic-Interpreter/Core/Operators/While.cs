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
            while (Expression.Eval().AsInteger() != 0)
            {
                Operator.Execute();
            }
        }
    }
}
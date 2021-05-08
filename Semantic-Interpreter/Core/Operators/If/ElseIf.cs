namespace Semantic_Interpreter.Core
{
    public class ElseIf : SemanticOperator
    {
        public ElseIf(IExpression expression, BlockSemanticOperator operators)
        {
            Expression = expression;
            Operators = operators;
        }
        
        public IExpression Expression { get; set; }
        private BlockSemanticOperator Operators { get; set; }
        public override void Execute()
        {
            Operators.Execute();
        }
    }
}
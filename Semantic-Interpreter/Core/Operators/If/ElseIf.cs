namespace Semantic_Interpreter.Core
{
    public class ElseIf : SemanticOperator
    {
        public ElseIf(IExpression expression, BlockSemanticOperator operators)
        {
            Expression = expression;
            Operators = operators;
        }
        
        public IExpression Expression { get; }
        public BlockSemanticOperator Operators { get; }
        public override void Execute() => Operators.Execute();
    }
}
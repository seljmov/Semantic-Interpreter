namespace Semantic_Interpreter.Core
{
    public class ElseIf : MultilineOperator
    {
        public ElseIf(IExpression expression, BlockSemanticOperator operators)
        {
            Expression = expression;
            Operators = operators;
            OperatorID = GenerateOperatorID();
        }
        
        public IExpression Expression { get; }
        public BlockSemanticOperator Operators { get; }
        public override string OperatorID { get; set; }
        public override void Execute() => Operators.Execute();
    }
}
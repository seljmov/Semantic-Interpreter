namespace Semantic_Interpreter.Core
{
    public class ElseIf : MultilineOperator
    {
        public ElseIf()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public IExpression Expression { get; set; }
        public sealed override BlockSemanticOperator Operators { get; set; }
        public sealed override string OperatorId { get; set; }

        public override void Execute() => Operators.Execute();
    }
}
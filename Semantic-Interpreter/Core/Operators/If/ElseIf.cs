namespace Semantic_Interpreter.Core
{
    public class ElseIf : MultilineOperator
    {
        public ElseIf() => OperatorID = GenerateOperatorId();
        
        public IExpression Expression { get; set; }
        public BlockSemanticOperator Operators { get; set; }
        public sealed override string OperatorID { get; set; }

        public override void Execute() => Operators.Execute();
    }
}
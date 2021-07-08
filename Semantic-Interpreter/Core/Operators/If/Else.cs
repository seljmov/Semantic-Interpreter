namespace Semantic_Interpreter.Core
{
    public class Else : MultilineOperator
    {
        public Else()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public sealed override BlockSemanticOperator Operators { get; set; }
        public sealed override string OperatorId { get; set; }
        
        public override void Execute() => Operators.Execute();
    }
}
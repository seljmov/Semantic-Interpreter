namespace Semantic_Interpreter.Core
{
    public class Else : MultilineOperator
    {
        public Else() => OperatorID = GenerateOperatorId();
        
        public BlockSemanticOperator Operators { get; set; }
        public sealed override string OperatorID { get; set; }
        
        public override void Execute() => Operators.Execute();
    }
}
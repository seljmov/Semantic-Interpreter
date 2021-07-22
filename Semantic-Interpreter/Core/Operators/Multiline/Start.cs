namespace Semantic_Interpreter.Core
{
    public class Start : MultilineOperator
    {
        public Start()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public sealed override string OperatorId { get; set; }
        
        public sealed override BlockSemanticOperator Operators { get; set; }
        
        public override void Execute() { }
    }
}
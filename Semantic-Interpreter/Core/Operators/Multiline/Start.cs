namespace Semantic_Interpreter.Core
{
    public class Start : MultilineOperator
    {
        public Start() => OperatorId = GenerateOperatorId();
        
        public sealed override string OperatorId { get; }
        
        public override void Execute() { }
    }
}
namespace Semantic_Interpreter.Core
{
    public class Start : MultilineOperator
    {
        public Start() => OperatorID = GenerateOperatorId();
        
        public sealed override string OperatorID { get; set; }
        
        public override void Execute() { }
    }
}
namespace Semantic_Interpreter.Core
{
    public class Procedure : BaseFunction
    {
        public Procedure() => OperatorID = GenerateOperatorId();
        
        public sealed override string OperatorID { get; set; }
        
        public override void Execute()
        {
            foreach (var t in Operators.Operators)
            {
                t.Execute();
            }
            
            VerifyParametersExpressions();
        }
    }
}
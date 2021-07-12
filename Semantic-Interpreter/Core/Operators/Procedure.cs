namespace Semantic_Interpreter.Core
{
    public class Procedure : BaseFunction
    {
        public Procedure() 
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }

        public sealed override string OperatorId { get; set; }
        
        public override void Execute()
        {
            foreach (var t in Operators.Operators)
            {
                t.Execute();
            }
            
            VerifyParametersExpressions();
            ClearVariableStorage();
        }
    }
}
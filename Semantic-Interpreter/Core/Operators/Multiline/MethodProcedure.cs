using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class MethodProcedure : BaseFunction, IMethod
    {
        public MethodProcedure()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public sealed override string OperatorId { get; set; }
        public ClassParameter ClassParameter { get; set; }
        
        public override void Execute()
        {
            Operators.Operators.ForEach(x => x.Execute());
            
            VerifyParametersExpressions();
            ClearVariableStorage();
        }
    }
}
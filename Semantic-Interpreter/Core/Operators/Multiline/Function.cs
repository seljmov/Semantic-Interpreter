using System;

namespace Semantic_Interpreter.Core
{
    public class Function : BaseFunction, IHaveType
    {
        public Function()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public SemanticType SemanticType { get; set; }
        public Return Return { get; set; }
        public sealed override string OperatorId { get; set; }
        
        public override void Execute()
        {
            foreach (var t in Operators.Operators)
            {
                try
                {
                    t.Execute();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            
            VerifyParametersExpressions();
            ClearVariableStorage();
        }
    }
}
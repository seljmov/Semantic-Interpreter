using System.Linq.Expressions;

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

        private void VerifyParametersExpressions()
        {
            var module = FindRoot();
            if (Parameters != null)
            {
                foreach (var t in Parameters)
                {
                    if (t.ParameterType == ParameterType.Var && t.InitExpression != t.Expression)
                    {
                        module.VariableStorage.Replace(t.VariableId, t.Expression);
                    }
                }
            }
        }
    }
}
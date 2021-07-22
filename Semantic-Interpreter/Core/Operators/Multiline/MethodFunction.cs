using System;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class MethodFunction : BaseFunction, IMethod
    {
        public MethodFunction()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public ClassParameter ClassParameter { get; set; }
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
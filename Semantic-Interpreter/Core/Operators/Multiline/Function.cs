using System;

namespace Semantic_Interpreter.Core
{
    public class Function : BaseFunction, IHaveReturn
    {
        public Function() => OperatorId = GenerateOperatorId();
        
        public SemanticType SemanticType { get; set; }
        public Return Return { get; set; }
        public sealed override string OperatorId { get; }
        
        public override void Execute()
        {
            foreach (var x in Block)
            {
                try
                {
                    x.Execute();
                }
                catch (Exception)
                {
                    VerifyParametersExpressions();
                    IHaveBlock.ClearVariableStorage(Block);
                    throw new  Exception();
                }
            }
            
            // VerifyParametersExpressions();
            // IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
using System;
using Semantic_Interpreter.Core.Items.Types;

namespace Semantic_Interpreter.Core
{
    public class Function : BaseFunction, IHaveReturn, IHaveType
    {
        public Function() => OperatorId = GenerateOperatorId();
        
        public ISemanticType Type { get; set; }
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
        }
    }
}
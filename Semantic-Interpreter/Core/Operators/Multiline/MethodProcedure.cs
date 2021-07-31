namespace Semantic_Interpreter.Core
{
    public class MethodProcedure : BaseFunction, IHaveClassParameter
    {
        public MethodProcedure() => OperatorId = GenerateOperatorId();

        public sealed override string OperatorId { get; }
        public string ClassParameter { get; set; }
        
        public override void Execute()
        {
            Block.ForEach(x => x.Execute());
            
            VerifyParametersExpressions();
            IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
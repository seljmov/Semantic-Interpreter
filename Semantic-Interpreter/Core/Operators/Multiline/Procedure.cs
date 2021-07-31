namespace Semantic_Interpreter.Core
{
    public class Procedure : BaseFunction
    {
        public Procedure() => OperatorId = GenerateOperatorId();

        public sealed override string OperatorId { get; }
        
        public override void Execute()
        {
            Block.ForEach(x => x.Execute());
            
            VerifyParametersExpressions();
            IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
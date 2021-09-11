namespace Semantic_Interpreter.Core
{
    public class Procedure : BaseFunction
    {
        public Procedure() => OperatorId = GenerateOperatorId();

        public sealed override string OperatorId { get; }
        
        public override void Execute()
        {
            foreach (var t in Block)
            {
                t.Execute();
            }
            
            VerifyParametersExpressions();
            IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class Else : MultilineOperator, IHaveBlock
    {
        public Else() => OperatorId = GenerateOperatorId();
        
        public List<SemanticOperator> Block { get; set; }
        public sealed override string OperatorId { get; }

        public override void Execute()
        {
            Block.ForEach(x => x.Execute());
            IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
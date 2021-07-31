using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class ElseIf : MultilineOperator, IHaveBlock
    {
        public ElseIf() => OperatorId = GenerateOperatorId();
        
        public IExpression Expression { get; set; }
        public List<SemanticOperator> Block { get; set; }
        public sealed override string OperatorId { get; }

        public override void Execute()
        {
            Block.ForEach(x => x.Execute());
            IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
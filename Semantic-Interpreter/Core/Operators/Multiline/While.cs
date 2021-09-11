using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class While : MultilineOperator, IHaveBlock
    {
        public While() => OperatorId = GenerateOperatorId();
        
        public IExpression Expression { get; set; }

        public sealed override string OperatorId { get; }
        
        public List<SemanticOperator> Block { get; set; }
        
        public override void Execute()
        {
            var result = Expression.Eval().AsBoolean();
            while (result)
            {
                foreach (var t in Block)
                {
                    t.Execute();
                }
                
                IHaveBlock.ClearVariableStorage(Block);

                result = Expression.Eval().AsBoolean();
            }
        }
    }
}
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class If : MultilineOperator, IHaveBlock
    {
        public If() => OperatorId = GenerateOperatorId();

        public IExpression Expression { get; set; }
        public List<SemanticOperator> Block { get; set; }
        public List<ElseIf> ElseIfs { get; set; }
        public Else Else { get; set; }
        public sealed override string OperatorId { get; }
        
        public override void Execute()
        {
            var result = Expression.Eval().AsBoolean();
            if (result)
            {
                Block.ForEach(x => x.Execute());
            }
            else
            {
                if (ElseIfs != null)
                {
                    foreach (var elseIf in ElseIfs)
                    {
                        var elseIfResult = elseIf.Expression.Eval().AsBoolean();
                        if (elseIfResult)
                        {
                            elseIf.Execute();
                            return;
                        }
                    }
                    
                }

                if (Else != null)
                {
                    Else.Execute();
                    return;
                }
            }
            
            IHaveBlock.ClearVariableStorage(Block);
        }
    }
}
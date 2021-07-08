using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class If : MultilineOperator
    {
        public If()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }

        public IExpression Expression { get; set; }
        
        public sealed override BlockSemanticOperator Operators { get; set; }
        public List<ElseIf> ElseIfs { get; set; }
        public Else Else { get; set; }
        public sealed override string OperatorId { get; set; }
        
        public override void Execute()
        {
            var result = Expression.Eval().AsBoolean();
            if (result)
            {
                Operators.Execute();
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
                
                Else?.Execute();
            }
        }
    }
}
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class If : MultilineOperator
    {
        public If() => OperatorID = GenerateOperatorId();

        public IExpression Expression { get; set; }
        public BlockSemanticOperator IfBlock { get; set; }
        public List<ElseIf> ElseIfs { get; set; }
        public Else Else { get; set; }
        public sealed override string OperatorID { get; set; }
        
        public override void Execute()
        {
            var result = Expression.Eval().AsBoolean();
            if (result)
            {
                IfBlock.Execute();
            }
            else
            {
                if (ElseIfs != null)
                {
                    var beExecuted = false;
                    foreach (var elseIf in ElseIfs)
                    {
                        var elseIfResult = elseIf.Expression.Eval().AsBoolean();
                        if (elseIfResult)
                        {
                            elseIf.Execute();
                            beExecuted = true;
                            break;
                        }
                    }
                    
                    if (!beExecuted) Else.Execute();
                }
            }
        }
    }
}
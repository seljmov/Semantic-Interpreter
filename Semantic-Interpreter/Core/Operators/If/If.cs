﻿using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class If : SemanticOperator
    {
        public If(IExpression expression, BlockSemanticOperator ifBlock, List<ElseIf> elseIfs, Else @else)
        {
            Expression = expression;
            IfBlock = ifBlock;
            ElseIfs = elseIfs;
            Else = @else;
        }

        public IExpression Expression { get; }
        public BlockSemanticOperator IfBlock { get; }
        public List<ElseIf> ElseIfs { get; }
        public Else Else { get; }

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
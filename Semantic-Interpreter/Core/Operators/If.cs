using System.Security.Principal;

namespace Semantic_Interpreter.Core
{
    public class If : SemanticOperator
    {
        public If(IExpression expression, BlockSemanticOperator ifBlock, BlockSemanticOperator elseBlock)
        {
            Expression = expression;
            IfBlock = ifBlock;
            ElseBlock = elseBlock;
        }

        public IExpression Expression { get; set; }
        public BlockSemanticOperator IfBlock { get; set; }
        public BlockSemanticOperator ElseBlock { get; set; }

        public override void Execute()
        {
            var result = Expression.Eval().AsInteger();
            if (result != 0)
            {
                IfBlock.Execute();
            }
            else
            {
                ElseBlock?.Execute();
            }
        }
    }
}
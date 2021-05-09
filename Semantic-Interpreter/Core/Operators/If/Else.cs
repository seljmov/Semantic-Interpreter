namespace Semantic_Interpreter.Core
{
    public class Else : SemanticOperator
    {
        public Else(BlockSemanticOperator operators)
            => Operators = operators;

        public BlockSemanticOperator Operators { get; }
        public override void Execute() => Operators.Execute();
    }
}
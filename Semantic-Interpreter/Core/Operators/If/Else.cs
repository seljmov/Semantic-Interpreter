namespace Semantic_Interpreter.Core
{
    public class Else : MultilineOperator
    {
        public Else(BlockSemanticOperator operators)
        {
            Operators = operators;
            OperatorID = GenerateOperatorID();
        }

        public BlockSemanticOperator Operators { get; }
        public override string OperatorID { get; set; }
        public override void Execute() => Operators.Execute();
    }
}
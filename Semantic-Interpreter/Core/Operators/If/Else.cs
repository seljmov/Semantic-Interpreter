namespace Semantic_Interpreter.Core
{
    public class Else : SemanticOperator
    {
        public Else(BlockSemanticOperator operators)
        {
            Operators = operators;
        }
        
        private BlockSemanticOperator Operators { get; set; }
        public override void Execute()
        {
            Operators.Execute();
        }
    }
}
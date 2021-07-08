namespace Semantic_Interpreter.Core
{
    public class While : MultilineOperator
    {
        public While()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public IExpression Expression { get; set; }
        public sealed override BlockSemanticOperator Operators { get; set; }
        
        public sealed override string OperatorId { get; set; }
        
        public override void Execute()
        {
            var result = Expression.Eval().AsBoolean();
            while (result)
            {
                Operators.Execute();
                result = Expression.Eval().AsBoolean();
            }
        }
    }
}
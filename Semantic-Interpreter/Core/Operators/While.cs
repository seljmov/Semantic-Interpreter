namespace Semantic_Interpreter.Core
{
    public class While : MultilineOperator
    {
        public While() => OperatorID = GenerateOperatorId();
        
        public IExpression Expression { get; set; }
        public BlockSemanticOperator Operators { get; set; }
        
        public sealed override string OperatorID { get; set; }
        
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
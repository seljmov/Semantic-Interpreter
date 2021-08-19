namespace Semantic_Interpreter.Core
{
    public class Call : SemanticOperator
    {
        public Call(IExpression expression) => Expression = expression;
        
        public IExpression Expression { get; }
        
        public override void Execute()
        {
            Expression.Eval();
        }
    }
}
namespace Semantic_Interpreter.Core
{
    public class Assign : SemanticOperator, IHaveExpression
    {
        public Variable Variable { get; set; }
        public IExpression Expression { get; set; }

        public Assign(Variable variable, IExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override void Execute()
        {
            var result = Expression.Eval();
            VariablesStorage.Replace(Variable.Name, new Variable(Variable.Name, result));
        }
    }
}
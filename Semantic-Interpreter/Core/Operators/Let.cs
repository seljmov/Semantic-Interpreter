using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Let : SemanticOperator
    {
        public Let(string variable, IExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
        
        public string Variable { get; }
        public IExpression Expression { get; }

        public override void Execute()
        {
            var currVar = VariablesStorage.At(Variable);
            var value = Expression.Eval();
            currVar.Expression = new ValueExpression(value);
            VariablesStorage.Replace(Variable, currVar);
        }
    }
}
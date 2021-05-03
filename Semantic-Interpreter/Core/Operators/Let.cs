using Semantic_Interpreter.Core.Items;
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
        
        private string Variable { get; set; }
        private IExpression Expression { get; set; }

        public override void Execute()
        {
            var currVar = VariablesStorage.At(Variable);
            var value = Expression.Eval();
            currVar.Expression = new ValueExpression(value);
            VariablesStorage.Replace(Variable, currVar);
        }

        public override string ToString()
            => string.Format($"{Variable} := {Expression}");
    }
}
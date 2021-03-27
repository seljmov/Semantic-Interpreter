using Semantic_Interpreter.Parser.Expressions;

namespace Semantic_Interpreter.Parser.Operators
{
    public class Assign : IOperator
    {
        public Assign(string variable, IExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
        
        private string Variable { get; set; }
        private IExpression Expression { get; set; }

        public void Execute()
        {
            // TODO: Create vars storage and add this var
            throw new System.NotImplementedException();
        }

        public override string ToString()
            => string.Format($"{Variable} := {Expression}");
    }
}
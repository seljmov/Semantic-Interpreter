using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class Assign : SingleLineOperator
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
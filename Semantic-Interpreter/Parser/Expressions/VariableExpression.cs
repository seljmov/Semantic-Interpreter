using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Parser.Expressions
{
    public class VariableExpression : IExpression
    {
        public VariableExpression(string name) => Name = name;

        private string Name { get; set; }
        
        public IValue Eval()
        {
            // TODO: Create vars storage
            throw new System.NotImplementedException();
        }

        public override string ToString() => string.Format(Name);
    }
}
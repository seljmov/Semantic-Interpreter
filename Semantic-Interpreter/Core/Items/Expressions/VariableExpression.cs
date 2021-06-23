namespace Semantic_Interpreter.Core
{
    public class VariableExpression : IExpression
    {
        private readonly Variable _variable;
        public VariableExpression(Variable variable) => _variable = variable;

        public IValue Eval() => _variable.GetValue();
    }
}
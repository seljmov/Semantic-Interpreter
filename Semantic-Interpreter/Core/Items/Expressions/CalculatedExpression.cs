using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class CalculatedExpression : IExpression
    {
        private ICalculated _calculated;

        public CalculatedExpression(ICalculated calculated)
            => _calculated = calculated;

        public IValue Eval()
            => _calculated.GetValue();
    }
}
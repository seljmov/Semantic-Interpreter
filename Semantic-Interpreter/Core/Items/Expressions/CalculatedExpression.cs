using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class CalculatedExpression : IExpression
    {
        public ICalculated Calculated { get; }

        public CalculatedExpression(ICalculated calculated)
            => Calculated = calculated;

        public IValue Eval()
            => Calculated.Calculate();
    }
}
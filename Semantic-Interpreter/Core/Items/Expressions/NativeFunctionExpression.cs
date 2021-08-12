using System.Collections.Generic;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class NativeFunctionExpression : IExpression
    {
        public NativeFunctionExpression(List<IExpression> expressions, IFunction function)
        {
            Expressions = expressions;
            Function = function;
        }

        private List<IExpression> Expressions { get; }
        private IFunction Function { get; }
        
        public IValue Eval()
        {
            if (Expressions == null) return Function.Execute();

            List<IValue> values = new();
            Expressions.ForEach(x => values.Add(x.Eval()));
            var array = values.ToArray();
            return Function.Execute(array);
        }
    }
}
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class ArrayAccessExpression : IExpression
    {
        public ArrayAccessExpression(List<IExpression> indexes, ArrayExpression arrayExpression)
        {
            Indexes = indexes;
            ArrayExpression = arrayExpression;
        }
        
        private List<IExpression> Indexes { get; }
        private ArrayExpression ArrayExpression { get; }
        
        public Value Eval() => ArrayExpression.Get(Indexes);
    }
}
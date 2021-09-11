using System.Collections.Generic;
using System.Linq;

namespace Semantic_Interpreter.Core
{
    public class ArrayExpression : IExpression
    {
        public ArrayExpression(ArrayValue arrayValue)
        {
            ArrayValue = arrayValue;
        }
        private ArrayValue ArrayValue { get; }
        
        public Value Eval()
        {
            return ArrayValue;
        }

        public void Set(List<IExpression> indexes, Value value)
        {
            var array = ArrayValue;
            for (var i = 0; i < indexes.Count-1; i++)
            {
                var arrayIndex = indexes[i].Eval().AsInteger();
                array = (ArrayValue) ArrayValue.Get(arrayIndex);
            }

            var index = indexes.Last().Eval().AsInteger();
            array?.Set(index, value);
        }

        public Value Get(List<IExpression> indexes)
        {
            var array = ArrayValue;
            for (var i = 0; i < indexes.Count-1; i++)
            {
                var arrayIndex = indexes[i].Eval().AsInteger();
                array = (ArrayValue) ArrayValue.Get(arrayIndex);
            }

            var index = indexes.Last().Eval().AsInteger();
            return array?.Get(index);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Semantic_Interpreter.Core
{
    public class ArrayExpression : IExpression
    {
        public ArrayExpression(string name, VariableType type, ArrayValue arrayValue)
        {
            Name = name;
            Type = type;
            Size = arrayValue.Size;
            ArrayValue = arrayValue;
        }
        
        private string Name { get; }
        private int Size { get; }
        private VariableType Type { get; }
        private ArrayValue ArrayValue { get; }
        
        public IValue Eval()
        {
            return ArrayValue;
        }

        public void Set(List<IExpression> indexes, IValue value)
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

        public IValue Get(List<IExpression> indexes)
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
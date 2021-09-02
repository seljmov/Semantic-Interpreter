using System.Collections.Generic;
using System.Linq;

namespace Semantic_Interpreter.Core
{
    public class ArrayExpression : IExpression
    {
        public ArrayExpression(string name, SemanticType semanticType, ArrayValue arrayValue)
        {
            Name = name;
            SemanticType = semanticType;
            Size = arrayValue.Size;
            ArrayValue = arrayValue;
        }
        
        private string Name { get; }
        private long Size { get; }
        private SemanticType SemanticType { get; }
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
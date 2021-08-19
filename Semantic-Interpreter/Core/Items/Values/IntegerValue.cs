using System;

namespace Semantic_Interpreter.Core
{
    public class IntegerValue : IValue
    {
        public IntegerValue(long value) => Value = value;

        private long Value { get; }

        public long AsInteger() => Value;

        public double AsReal() => Convert.ToDouble(Value);

        public bool AsBoolean() => Convert.ToBoolean(Value);
        
        public char AsChar() => Convert.ToChar(Value);

        public string AsString() => Convert.ToString(Value);
        
        public IValue[] AsArray()
            => throw new Exception("Невозможно преобразовать целое число к массиву");

        public object AsObject() => Value;

        public override string ToString() => AsString();
    }
}
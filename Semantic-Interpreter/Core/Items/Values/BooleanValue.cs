using System;

namespace Semantic_Interpreter.Core
{
    public class BooleanValue : IValue
    {
        public BooleanValue(bool value) => Value = value;

        private bool Value { get; }

        public int AsInteger() => Convert.ToInt32(Value);

        public double AsReal() => Convert.ToDouble(Value);

        public bool AsBoolean() => Value;

        public char AsChar() => Convert.ToChar(Value);

        public string AsString() => Convert.ToString(Value);

        public IValue[] AsArray()
            => throw new Exception("Невозможно преобразовать булевское значение к массиву");

        public override string ToString() => AsString();
    }
}
using System;

namespace Semantic_Interpreter.Core
{
    public class CharValue : IValue
    {
        public CharValue(char value) => Value = value;

        private char Value { get; }

        public int AsInteger() => Convert.ToInt32(Value);

        public double AsReal() => Convert.ToDouble(Value);

        public bool AsBoolean() => Convert.ToBoolean(Value);

        public char AsChar() => Value;

        public string AsString() => Convert.ToString(Value);
        
        public IValue[] AsArray()
            => throw new Exception("Невозможно преобразовать символ к массиву");

        public override string ToString() => AsString();
    }
}
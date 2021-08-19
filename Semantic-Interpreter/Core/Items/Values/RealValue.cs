using System;
using System.Globalization;

namespace Semantic_Interpreter.Core
{
    public class RealValue : IValue
    {
        public RealValue(double value) => Value = value;

        private double Value { get; }

        public long AsInteger() => Convert.ToInt32(Value);

        public double AsReal() => Value;

        public bool AsBoolean() => Convert.ToBoolean(Value);
        
        public char AsChar() => Convert.ToChar(Value);

        public string AsString() => Convert.ToString(Value, CultureInfo.InvariantCulture);
        
        public IValue[] AsArray()
            => throw new Exception("Невозможно преобразовать вещественное число к массиву");

        public object AsObject() => Value;

        public override string ToString() => AsString();
    }
}
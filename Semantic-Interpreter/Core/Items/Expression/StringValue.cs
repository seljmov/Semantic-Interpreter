using System;

namespace Semantic_Interpreter.Core
{
    public class StringValue : IValue
    {
        private readonly string _value;

        public StringValue(string value) => _value = value;

        public int AsInteger() => Convert.ToInt32(_value);

        public double AsReal() => Convert.ToDouble(_value);

        public bool AsBoolean() => Convert.ToBoolean(_value);

        public string AsString() => _value;
    }
}
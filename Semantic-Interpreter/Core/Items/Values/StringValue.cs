using System;

namespace Semantic_Interpreter.Core
{
    public class StringValue : IValue
    {
        public StringValue(string value) => Value = value;
        
        private string Value { get; }

        public int AsInteger() => Convert.ToInt32(Value);

        public double AsReal() => Convert.ToDouble(Value);

        public bool AsBoolean() => Convert.ToBoolean(Value);
        
        public char AsChar() => Convert.ToChar(Value);

        public string AsString() => Value;

        public override string ToString() => AsString();
    }
}
using System;

namespace Semantic_Interpreter.Core
{
    public class StringValue : Value
    {
        public StringValue(string value) => Value = value;
        
        private string Value { get; }

        public override long AsInteger() => Convert.ToInt32(Value);

        public override double AsReal() => Convert.ToDouble(Value);

        public override bool AsBoolean() => Convert.ToBoolean(Value);
        
        public override char AsChar() => Convert.ToChar(Value);

        public override string AsString() => Value;

        public override object AsObject() => Value;
        
        public override string ToString() => AsString();
    }
}
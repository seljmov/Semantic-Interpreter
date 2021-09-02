using System;

namespace Semantic_Interpreter.Core
{
    public class IntegerValue : Value
    {
        public IntegerValue(long value) => Value = value;

        private long Value { get; }

        public override long AsInteger() => Value;

        public override double AsReal() => Convert.ToDouble(Value);

        public override bool AsBoolean() => Convert.ToBoolean(Value);
        
        public override char AsChar() => Convert.ToChar(Value);

        public override string AsString() => Convert.ToString(Value);

        public override object AsObject() => Value;

        public override string ToString() => AsString();
    }
}
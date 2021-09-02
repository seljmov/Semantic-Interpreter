using System;

namespace Semantic_Interpreter.Core
{
    public class BooleanValue : Value
    {
        public BooleanValue(bool value) => Value = value;

        private bool Value { get; }

        public override long AsInteger() => Convert.ToInt32(Value);

        public override double AsReal() => Convert.ToDouble(Value);

        public override bool AsBoolean() => Value;

        public override char AsChar() => Convert.ToChar(Value);

        public override string AsString() => Convert.ToString(Value);

        public override object AsObject() => Value;
        
        public override string ToString() => AsString();
    }
}
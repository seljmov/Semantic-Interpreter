using System;

namespace Semantic_Interpreter.Core
{
    public class CharValue : Value
    {
        public CharValue(char value) => Value = value;

        private char Value { get; }

        public override long AsInteger() => Convert.ToInt32(Value);

        public override double AsReal() => Convert.ToDouble(Value);

        public override bool AsBoolean() => Convert.ToBoolean(Value);

        public override char AsChar() => Value;

        public override string AsString() => Convert.ToString(Value);

        public override object AsObject() => Value;
        
        public override string ToString() => AsString();
    }
}
using System;
using System.Globalization;

namespace Semantic_Interpreter.Core
{
    public class RealValue : Value
    {
        public RealValue(double value) => Value = value;

        private double Value { get; }

        public override long AsInteger() => Convert.ToInt32(Value);

        public override double AsReal() => Value;

        public override bool AsBoolean() => Convert.ToBoolean(Value);
        
        public override char AsChar() => Convert.ToChar(Value);

        public override string AsString() => Convert.ToString(Value, CultureInfo.InvariantCulture);

        public override object AsObject() => Value;

        public override string ToString() => AsString();
    }
}
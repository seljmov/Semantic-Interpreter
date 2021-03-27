using System;

namespace Semantic_Interpreter.Library
{
    public class IntegerValue : IValue
    {
        public IntegerValue(int value) => Value = value;

        private int Value { get; set; }

        public int AsInteger() => Value;

        public double AsReal() => Convert.ToDouble(Value);

        public bool AsBoolean() => Convert.ToBoolean(Value);

        public string AsString() => Convert.ToString(Value);
        
        public override string ToString() => AsString();
    }
}
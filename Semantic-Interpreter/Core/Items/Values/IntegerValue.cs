﻿using System;

namespace Semantic_Interpreter.Core
{
    public class IntegerValue : IValue
    {
        public IntegerValue(int value) => Value = value;

        private int Value { get; }

        public int AsInteger() => Value;

        public double AsReal() => Convert.ToDouble(Value);

        public bool AsBoolean() => Convert.ToBoolean(Value);
        
        public char AsChar() => Convert.ToChar(Value);

        public string AsString() => Convert.ToString(Value);
        
        public override string ToString() => AsString();
    }
}
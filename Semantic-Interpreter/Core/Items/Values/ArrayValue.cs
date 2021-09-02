using System;

namespace Semantic_Interpreter.Core
{
    public class ArrayValue : Value
    {
        public ArrayValue(long size)
        {
            Size = size;
            Values = new Value[size];
        }

        public ArrayValue(Value[] values)
        {
            Values = new Value[values.Length];
            Array.Copy(values, Values, values.Length);
            Size = values.Length;
        }

        public long Size { get; }
        private Value[] Values { get; }

        public Value Get(long index) => Values[index];

        public void Set(long index, Value value) => Values[index] = value;

        public override bool AsBoolean() => Values.Length != 0;

        public override string AsString() => Values.ToString();

        public override Value[] AsArray() => Values;

        public override object AsObject() => Values;
        
        public override string ToString() => AsString();
    }
}
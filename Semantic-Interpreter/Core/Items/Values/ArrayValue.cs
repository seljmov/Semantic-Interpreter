using System;

namespace Semantic_Interpreter.Core
{
    public class ArrayValue : IValue
    {
        public ArrayValue(long size)
        {
            Size = size;
            Values = new IValue[size];
        }

        public ArrayValue(IValue[] values)
        {
            Values = new IValue[values.Length];
            Array.Copy(values, Values, values.Length);
            Size = values.Length;
        }

        public long Size { get; }
        private IValue[] Values { get; }

        public IValue Get(long index) => Values[index];

        public void Set(long index, IValue value) => Values[index] = value;

        public long AsInteger()
            => throw new Exception("Невозможно преобразовать массив в целое числу");

        public double AsReal()
            => throw new Exception("Невозможно преобразовать массив в вещественное числу");

        public bool AsBoolean() => Values.Length != 0;

        public char AsChar()
            => throw new Exception("Невозможно преобразовать массив в символ");

        public string AsString() => Values.ToString();

        public IValue[] AsArray() => Values;

        public object AsObject() => Values;
        
        public override string ToString() => AsString();
    }
}
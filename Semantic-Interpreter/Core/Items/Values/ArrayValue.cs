using System;
using Semantic_Interpreter.Core.Items.Types;

namespace Semantic_Interpreter.Core
{
    public class ArrayValue : Value
    {
        public long Size { get; set; }
        
        public ISemanticType Type { get; set; }
        
        public Value[] Values { get; set; }

        public Value Get(long index) => Values[index];

        public void Set(long index, Value value) => Values[index] = value;

        public override bool AsBoolean() => Values.Length != 0;

        public override string AsString() => Values.ToString();

        public override Value[] AsArray() => Values;

        public override object AsObject() => Values;
        
        public override string ToString() => AsString();
    }
}
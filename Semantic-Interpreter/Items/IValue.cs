using System;

namespace Semantic_Interpreter.Items
{
    public interface IValue : IComparable<IValue>, IEquatable<IValue>
    {
        public int Type { get; }
    }
}
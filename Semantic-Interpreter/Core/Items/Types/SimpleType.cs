using System;

namespace Semantic_Interpreter.Core.Items.Types
{
    public class SimpleType : ISemanticType
    {
        public SimpleType(ValueType type)
        {
            Type = type;
        }

        public ValueType Type { get; }
        public string FullType => ToString();

        public override string ToString()
        {
            return Type switch
            {
                ValueType.Integer => "integer",
                ValueType.Real => "real",
                ValueType.Boolean => "boolean",
                ValueType.Char => "char",
                ValueType.String => "string",
                ValueType.Object => "object",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
namespace Semantic_Interpreter.Core.Items.Types
{
    public class ArrayType : ISemanticType
    {
        public ISemanticType Type { get; set; }
        public IExpression Size { get; set; }
        public string FullType => ToString();
        public string Signature => $"array[] {Type.Signature}";

        public override string ToString()
        {
            var size = Size.Eval().AsInteger();
            var dim = size == 0 ? "" : $"{size}";
            return $"array[{dim}] {Type.FullType}";
        }
    }
}
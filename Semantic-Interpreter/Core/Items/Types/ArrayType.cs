namespace Semantic_Interpreter.Core.Items.Types
{
    public class ArrayType : ISemanticType
    {
        public ISemanticType Type { get; set; }
        public long Size { get; set; }
        public string FullType => ToString();

        public override string ToString()
        {
            return $"array[{Size}] {Type.FullType}";
        }
    }
}
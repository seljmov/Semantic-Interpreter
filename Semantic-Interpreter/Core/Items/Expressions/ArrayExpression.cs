namespace Semantic_Interpreter.Core
{
    public class ArrayExpression : IExpression
    {
        public ArrayExpression(string name, int size, VariableType type)
        {
            Name = name;
            Size = size;
            Type = type;
            ArrayValue = new ArrayValue(size);
        }
        
        private string Name { get; }
        private int Size { get; }
        private VariableType Type { get; }
        private ArrayValue ArrayValue { get; }
        
        public IValue Eval()
        {
            return ArrayValue;
        }

        public IValue Get(int index) => ArrayValue.Get(index);

        public void Set(int index, IValue value) => ArrayValue.Set(index, value);
    }
}
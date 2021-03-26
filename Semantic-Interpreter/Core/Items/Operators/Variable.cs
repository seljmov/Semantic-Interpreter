namespace Semantic_Interpreter.Core
{
    public class Variable : IExpression
    {
        public ConstantTypes Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public Variable(string name, object value)
        {
            SetType(value);
            Name = name;
            Value = value;
        }

        public IValue Eval()
            => (IValue) Value;

        private void SetType(object value)
        {
            Type = value switch
            {
                int _ => ConstantTypes.Integer,
                double _ => ConstantTypes.Real,
                bool _ => ConstantTypes.Boolean,
                _ => ConstantTypes.String
            };
        }
    }
}
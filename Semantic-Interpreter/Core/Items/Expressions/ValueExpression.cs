namespace Semantic_Interpreter.Core
{
    public class ValueExpression : IExpression
    {
        public ValueExpression(int value) => Value = new IntegerValue(value);

        public ValueExpression(double value) => Value = new RealValue(value);
        
        public ValueExpression(bool value) => Value = new BooleanValue(value);
        
        public ValueExpression(char value) => Value = new CharValue(value);
        
        public ValueExpression(string value) => Value = new StringValue(value);
        public ValueExpression(Class value) => Value = new ClassValue(value);

        public ValueExpression(Value value) => Value = value;

        private Value Value { get; }

        public Value Eval() => Value;

        public override string ToString() => Value.AsString();
    }
}
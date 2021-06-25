namespace Semantic_Interpreter.Core
{
    public class Parameter
    {
        public ParameterType ParameterType { get; set; }
        public VariableType VariableType { get; set; }
        public string Name { get; set; }
        public string VariableId { get; set; }
        public IExpression Expression { get; set; }
        public IExpression InitExpression { get; set; }
    }
}
namespace Semantic_Interpreter.Core
{
    public class Parameter : SemanticOperator
    {
        public ParameterType ParameterType { get; set; }
        public VariableType VariableType { get; set; }
        public string Name { get; set; }
        public IExpression Expression { get; set; }
        public string VariableId { get; set; }

        public override void Execute()
        {
            var module = FindRoot();
            var variable = module.VariableStorage.At(VariableId);
            Expression = variable.Expression;
        }
    }
}
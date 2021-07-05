using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Parameter : SemanticOperator, ICalculated
    {
        public ParameterType ParameterType { get; set; }
        public VariableType VariableType { get; set; }
        public string Name { get; set; }
        public string VariableId { get; set; }

        public IExpression Expression { get; set; }

        public IValue GetValue() => Expression.Eval();
        
        public override void Execute()
        {
            Expression = VariableStorage.At(VariableId).Expression;
        }
    }
}
using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Parameter : SemanticOperator, ICalculated
    {
        public ParameterType ParameterType { get; set; }
        public SemanticType SemanticType { get; set; }
        public string Name { get; set; }
        public string VariableId { get; set; }

        public IExpression Expression { get; set; }

        public Value Calculate() => Expression.Eval();

        public override void Execute()
        {
            if (VariableId != null)
            {
                Expression = GetRoot().Module.VariableStorage.At(VariableId).Expression;
            }
        }
    }
}
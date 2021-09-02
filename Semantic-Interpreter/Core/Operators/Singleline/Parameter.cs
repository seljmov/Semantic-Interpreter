using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class Parameter : SemanticOperator, ICalculated
    {
        public ParameterType ParameterType { get; set; }
        public SemanticType SemanticType { get; set; }
        public string Name { get; set; }
        public SemanticOperator Operator { get; set; }

        public IExpression Expression { get; set; }

        public Value Calculate() => Expression.Eval();

        public override void Execute()
        {
            if (Operator == null) return;

            if (Operator is Variable variable)
            {
                var module = GetRoot().Module;
                if (module.VariableStorage.IsExist(variable.Id))
                {
                    Expression = variable.Expression;
                    return;
                }
            }

            Expression = ((Parameter) Operator).Expression;
        }
    }
}
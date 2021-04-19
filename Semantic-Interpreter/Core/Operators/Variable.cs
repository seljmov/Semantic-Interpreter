using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Variable : SemanticOperator, IHaveExpression
    {
        public Variable(SemanticTypes type, string name, IExpression expression)
        {
            Type = type;
            Name = name;
            // Expression = expression;
            var value = expression.Eval();
            switch (type)
            {
                case SemanticTypes.String when value is StringValue:
                case SemanticTypes.Integer when value is IntegerValue:
                case SemanticTypes.Real when value is RealValue || value is IntegerValue:
                    Expression = expression;
                    break;
                case SemanticTypes.Boolean:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public SemanticTypes Type { get; set; }
        public string Name { get; set; }
        public IExpression Expression { get; set; }

        public IValue Eval()
            => VariablesStorage.IsExist(Name)
                ? Expression.Eval()
                : throw new Exception("Переменной с таким именем не существует!");

        public override string ToString() => Name;

        public override void Execute()
        {
        }
    }
}
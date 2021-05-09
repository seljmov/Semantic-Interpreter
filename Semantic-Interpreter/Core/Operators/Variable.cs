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
            
            if (expression != null)
            {
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
        }

        public SemanticTypes Type { get; }
        public string Name { get; }
        public IExpression Expression { get; set; }

        public IValue Eval()
            => !VariablesStorage.IsExist(Name)
                ? throw new Exception("Переменной с таким именем не существует!")
                : Expression != null
                    ? Expression.Eval()
                    : throw new Exception($"Переменная с именем {Name} не инициализированна!");
        
        public override void Execute() { }
    }
}
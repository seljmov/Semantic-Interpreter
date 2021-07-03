using System;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class Variable : SemanticOperator, ICalculated
    {
        public Variable(VariableType type, string name, string id, IExpression expression)
        {
            Type = type;
            Name = name;
            Id = id;
            Expression = expression;
        }

        public VariableType Type { get; }
        public string Name { get; }
        public string Id { get; }
        public IExpression Expression { get; set; }
        
        public IValue GetValue()
        {
            var module = FindRoot();
            return module.VariableStorage.IsExist(Id) 
                ? module.VariableStorage.At(Id).Expression.Eval() 
                : throw new Exception($"Переменная {Name} не инициализированна!");
        }

        public override void Execute()
        {
            if (Expression != null)
            {
                var value = Expression.Eval();
                var expression = new ValueExpression(value);
                var module = FindRoot();
                module.VariableStorage.Replace(Id, expression);
                Expression = new ValueExpression(value);
            }
        }

        /**
         * Проверка типа и значения переменной. Правила.
         * К переменной типа String можно присвоить только StringValue, 
         * К типа Integer можно присвоить только IntegerValue, 
         * К типа Boolean можно присвоить только BooleanValue, 
         * К типа Char можно присвоить только CharValue,
         * а вот к типа Real можно присвоить RealValue и IntegerValue.
         */
        private static bool TypeIsCorrect(VariableType type, IValue value)
        {
            switch (type)
            {
                case VariableType.String when value is StringValue:
                case VariableType.Integer when value is IntegerValue:
                case VariableType.Boolean when value is BooleanValue:
                case VariableType.Char when value is CharValue:
                case VariableType.Real when value is RealValue || value is IntegerValue:
                    return true;
                default:
                    return false;
            }
        }
    }
}
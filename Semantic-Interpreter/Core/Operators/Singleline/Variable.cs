using System;
using System.Linq.Expressions;
using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Core.Items.Types;

namespace Semantic_Interpreter.Core
{
    public class Variable : SemanticOperator, ICalculated, IHaveType, ICloneable
    {
        public Variable(ISemanticType type, string name, string id, IExpression expression)
        {
            Type = type;
            Name = name;
            Id = id;
            Expression = expression;
        }

        public Variable(IExpression expression) => Expression = expression;

        public ISemanticType Type { get; set; }
        public string Name { get; }
        public string Id { get; }
        public IExpression Expression { get; set; }

        public Value Calculate() =>
            GetRoot().Module.VariableStorage.IsExist(Id) 
                ? GetRoot().Module.VariableStorage.At(Id).Expression.Eval() 
                : Expression.Eval();

        public override void Execute()
        {
            if (Expression != null && Expression is not ArrayExpression)
            {
                var value = Expression.Eval();
                Expression = new ValueExpression(value);
            }

            var module = GetRoot().Module;
            var clone = (Variable) Clone();
            module.VariableStorage.Add(Id, clone);
        }

        /**
         * Проверка типа и значения переменной. Правила.
         * К переменной типа String можно присвоить только StringValue, 
         * К типа Integer можно присвоить только IntegerValue, 
         * К типа Boolean можно присвоить только BooleanValue, 
         * К типа Char можно присвоить только CharValue,
         * а вот к типа Real можно присвоить RealValue и IntegerValue.
         */
        private static bool TypeIsCorrect(ValueType valueType, Value value)
        {
            switch (valueType)
            {
                case ValueType.String when value is StringValue:
                case ValueType.Integer when value is IntegerValue:
                case ValueType.Boolean when value is BooleanValue:
                case ValueType.Char when value is CharValue:
                case ValueType.Real when value is RealValue or IntegerValue:
                    return true;
                default:
                    return false;
            }
        }

        public object Clone()
        {
            return new Variable(Type, Name, Id, Expression) {Parent = Parent};
        }
    }
}
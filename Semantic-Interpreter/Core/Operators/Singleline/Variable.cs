using System;
using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Variable : SemanticOperator, ICalculated, IHaveType
    {
        public Variable(SemanticType semanticType, string name, string id, IExpression expression)
        {
            SemanticType = semanticType;
            Name = name;
            Id = id;
            Expression = expression;
        }

        public SemanticType SemanticType { get; }
        public string Name { get; }
        public string Id { get; }
        public IExpression Expression { get; set; }

        public IValue Calculate() =>
            VariableStorage.IsExist(Id) 
                ? VariableStorage.At(Id).Expression.Eval() 
                : Expression.Eval();

        public override void Execute()
        {
            VariableStorage.Add(Id, new Variable(SemanticType, Name, Id, Expression));
            
            if (Expression != null && !(Expression is ArrayExpression) && !(Expression.Eval() is ClassValue))
            {
                var value = VariableStorage.At(Id).Expression.Eval();
                var expression = new ValueExpression(value);
                VariableStorage.Replace(Id, expression);
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
        private static bool TypeIsCorrect(SemanticType semanticType, IValue value)
        {
            switch (semanticType)
            {
                case SemanticType.String when value is StringValue:
                case SemanticType.Integer when value is IntegerValue:
                case SemanticType.Boolean when value is BooleanValue:
                case SemanticType.Char when value is CharValue:
                case SemanticType.Real when value is RealValue || value is IntegerValue:
                    return true;
                default:
                    return false;
            }
        }
    }
}
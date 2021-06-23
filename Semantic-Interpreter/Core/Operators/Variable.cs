using System;

namespace Semantic_Interpreter.Core
{
    public class Variable : SemanticOperator
    {
        public Variable(SemanticTypes type, string name, IExpression expression)
        {
            Type = type;
            Name = name;
            
            if (expression != null)
            {
                var value = expression.Eval();
                if (TypeIsCorrect(type, value))
                {
                    Expression = expression;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        public SemanticTypes Type { get; }
        public string Name { get; }
        public IExpression Expression { get; set; }
        
        public IValue GetValue()
        {
            var fullId = ((MultilineOperator) Parent).OperatorID;
            var curr = Parent;
            while (curr.Parent != null)
            {
                curr = curr.Parent;
                fullId = ((MultilineOperator) curr).OperatorID + "^" + fullId;
            }

            var module = (Module) curr;
            var subs = fullId.Split("^");
            
            for (var i = subs.Length-1; i >= 0; --i)
            {
                var varId = "";
                for (var j = 0; j <= i; ++j)
                {
                    varId += subs[j] + "^";
                }

                varId += Name;
                if (module.VariableStorage.IsExist(varId))
                {
                    var variable = module.VariableStorage.At(varId);
                    return variable.Expression != null 
                        ? variable.Expression.Eval() 
                        : throw new Exception($"Переменная с именем {Name} не инициализированна!");
                }
            }

            throw new Exception("Переменной с таким именем не существует!");
        }

        public override void Execute() { }

        /**
         * Проверка типа и значения переменной. Правила.
         * К переменной типа String можно присвоить только StringValue, 
         * К типа Integer можно присвоить только IntegerValue, 
         * К типа Boolean можно присвоить только BooleanValue, 
         * К типа Char можно присвоить только CharValue,
         * а вот к типа Real можно присвоить RealValue и IntegerValue.
         */
        private static bool TypeIsCorrect(SemanticTypes type, IValue value)
        {
            switch (type)
            {
                case SemanticTypes.String when value is StringValue:
                case SemanticTypes.Integer when value is IntegerValue:
                case SemanticTypes.Boolean when value is BooleanValue:
                case SemanticTypes.Char when value is CharValue:
                case SemanticTypes.Real when value is RealValue || value is IntegerValue:
                    return true;
                default:
                    return false;
            }
        }
    }
}
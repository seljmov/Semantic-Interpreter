using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class InterpolationExpression : IExpression
    {
        public InterpolationExpression(IExpression expression, SemanticOperator parent)
        {
            _expression = expression;
            _parent = parent;
        }

        private IExpression _expression;
        private SemanticOperator _parent;
        
        public IValue Eval()
        {
            var text = _expression.Eval().AsString();
            var result = "";
            
            for (var i = 0; i < text.Length; ++i)
            {
                if (text[i] != '$')
                {
                    result += text[i];
                }
                else
                {
                    ++i; // Skip $
                    if (i < text.Length && text[i] == '{')
                    {
                        ++i; // Skip {
                        // Узнаем имя переменной
                        var name = "";
                        while (i < text.Length && (char.IsLetterOrDigit(text[i]) || text[i] == '_'))
                        {
                            name += text[i];
                            ++i;
                        }
                        
                        // var value = _module.VariableStorage.At(name).Expression.Eval().AsString();
                        // var value = _parent is BaseFunction ? GetParameterValue(name) : GetVariableValue(name);
                        var value = GetValue(name).AsString();
                        result += value ?? throw new Exception($"Переменная с именем {name} не инициализированна!");
                    }
                }
            }
            
            return new StringValue(result);
        }

        private IValue GetValue(string name)
        {
            var curr = _parent;
            while (curr.Parent != null)
            {
                if (curr is BaseFunction function)
                {
                    var parameterValue = GetParameterValue(name, function);
                    if (parameterValue != null)
                    {
                        return parameterValue;
                    }
                }

                curr = curr.Parent;
            }

            var variableValue = GetVariableValue(name);
            if (variableValue != null)
            {
                return variableValue;
            }
            
            throw new Exception($"Параметра/переменной {name} не существует!");
        }
        
        private IValue GetParameterValue(string name, BaseFunction function)
        {
            var parameters = function.Parameters;
            foreach (var t in parameters)
            {
                if (t.Name == name)
                {
                    return t.Expression.Eval();
                }
            }

            return null;
        }

        private IValue GetVariableValue(string name)
        {
            var module = _parent.FindRoot();
            var curr = _parent;
            while (curr.Parent != null)
            {
                var parentId = ((MultilineOperator) curr).OperatorID;
                var variableId = $"{parentId}^{name}";

                if (VariableStorage.IsExist(variableId))
                {
                    var variable = VariableStorage.At(variableId);
                    return variable.Expression?.Eval();
                }

                curr = curr.Parent;
            }

            return null;
        }
    }
}
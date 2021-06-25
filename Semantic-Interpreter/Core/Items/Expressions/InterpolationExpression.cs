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
            if (_parent is BaseFunction)
            {
                var parameterValue = GetParameterValue(name);
                if (parameterValue != null)
                {
                    return parameterValue;
                }
            }
            
            var variableValue = GetVariableValue(name);
            if (variableValue != null)
            {
                return variableValue;
            }
            
            throw new Exception($"Параметра/переменной {name} не существует!");
        }
        
        private IValue GetParameterValue(string name)
        {
            var func = (BaseFunction) _parent;
            var parameters = func.Parameters;
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
            var fullId = ((MultilineOperator) _parent).OperatorID;
            var curr = _parent;
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

                varId += name;
                if (module.VariableStorage.IsExist(varId))
                {
                    var variable = module.VariableStorage.At(varId);
                    return variable.Expression?.Eval();
                }
            }

            return null;
        }
    }
}
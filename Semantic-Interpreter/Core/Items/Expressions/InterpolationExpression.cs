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
                        var value = GetVariableValue(name);
                        result += value ?? throw new Exception($"Переменная с именем {name} не инициализированна!");
                    }
                }
            }
            
            return new StringValue(result);
        }
        
        public IValue GetVariableValue(string name)
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
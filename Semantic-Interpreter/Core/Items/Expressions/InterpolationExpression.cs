using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class InterpolationExpression : IExpression
    {
        public InterpolationExpression(IExpression expression) => Expression = expression;
        
        private IExpression Expression { get; }
        
        public IValue Eval()
        {
            var text = Expression.Eval().AsString();
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

                        ++i; // Skip }

                        var value = VariablesStorage.At(name).Expression.Eval().AsString();
                        result += value;
                    }
                }
            }
            
            return new StringValue(result);
        }
    }
}
using System;
using System.Linq;

namespace Semantic_Interpreter.Core
{
    public class Let : SemanticOperator
    {
        public Let(string name, IExpression expression, IExpression bracketExpression)
        {
            Name = name;
            Expression = expression;
            BracketExpression = bracketExpression;
        }
        
        public string Name { get; }
        public IExpression Expression { get; }
        public IExpression BracketExpression { get; }

        public override void Execute()
        {
            var module = FindRoot();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);

            if (BracketExpression != null)
            {
                var arrayIndex = BracketExpression.Eval().AsInteger();

                if (module.VariableStorage.IsExist(Name))
                {
                    var arrayExpression = (ArrayExpression) module.VariableStorage.At(Name).Expression;
                    arrayExpression.Set(arrayIndex, Expression.Eval());
                    
                    module.VariableStorage.Replace(Name, arrayExpression);
                }
                
                return;
            }

            var curr = Parent;
            while (curr.Parent != null)
            {
                if (curr is BaseFunction function && function.ParameterIsExist(Name))
                {
                    function.Parameters.Single(t => t.Name == Name).Expression = expression;
                    return;
                }
                
                curr = curr.Parent;
            }
            
            if (module.VariableStorage.IsExist(Name))
            {
                module.VariableStorage.Replace(Name, expression);
                return;
            }

            throw new Exception("Переменной с таким именем не существует!");
        }
    }
}
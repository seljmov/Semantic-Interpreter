using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Let : SemanticOperator
    {
        public Let(string name, IExpression expression, List<IExpression> indexes)
        {
            Name = name;
            Expression = expression;
            Indexes = indexes;
        }
        
        public string Name { get; }
        public IExpression Expression { get; }
        public List<IExpression> Indexes { get; }

        public override void Execute()
        {
            var value = Expression.Eval();
            var expression = new ValueExpression(value);

            if (Indexes != null)
            {
                if (VariableStorage.IsExist(Name))
                {
                    var arrayExpression = (ArrayExpression) VariableStorage.At(Name).Expression;

                    arrayExpression.Set(Indexes, value);

                    VariableStorage.Replace(Name, arrayExpression);
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
            
            if (VariableStorage.IsExist(Name))
            {
                VariableStorage.Replace(Name, expression);
                return;
            }

            throw new Exception("Переменной с таким именем не существует!");
        }
    }
}
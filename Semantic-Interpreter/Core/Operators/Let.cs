using System;
using System.Collections.Generic;
using System.Linq;

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
            var module = FindRoot();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);

            if (Indexes != null)
            {
                if (module.VariableStorage.IsExist(Name))
                {
                    var arrayExpression = (ArrayExpression) module.VariableStorage.At(Name).Expression;

                    arrayExpression.Set(Indexes, value);

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
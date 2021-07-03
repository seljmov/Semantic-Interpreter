using System;
using System.Linq;

namespace Semantic_Interpreter.Core
{
    public class Let : SemanticOperator
    {
        public Let(string name, IExpression expression)
        {
            Name = name;
            Expression = expression;
        }
        
        public string Name { get; }
        public IExpression Expression { get; }

        public override void Execute()
        {
            var module = FindRoot();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);

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
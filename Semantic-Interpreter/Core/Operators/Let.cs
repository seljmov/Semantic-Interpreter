using System;

namespace Semantic_Interpreter.Core
{
    public class Let : SemanticOperator
    {
        public Let(string name, IExpression expression)
        {
            Name = name;
            Expression = expression;
        }
        
        public string Name { get; set; }
        public IExpression Expression { get; }

        public override void Execute()
        {
            var module = FindRoot();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);

            if (Parent is BaseFunction function)
            {
                foreach (var t in function.Parameters)
                {
                    if (t.Name == Name)
                    {
                        t.Expression = expression;
                        return;
                    }
                }
            }
            else
            {
                if (module.VariableStorage.IsExist(Name))
                {
                    module.VariableStorage.Replace(Name, expression);
                    return;
                }
            }

            throw new Exception("Переменной с таким именем не существует!");
        }
    }
}
﻿using System;

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

            if (module.VariableStorage.IsExist(Name))
            {
                module.VariableStorage.Replace(Name, expression);
                return;
            }

            throw new Exception("Переменной с таким именем не существует!");
        }
    }
}
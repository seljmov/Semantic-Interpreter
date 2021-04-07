using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Variable : SemanticOperator, IHaveExpression
    {
        public Variable(SemanticTypes type, string name, IExpression expression)
        {
            Type = type;
            Name = name;
            Expression = expression;
        }

        public SemanticTypes Type { get; set; }
        public string Name { get; set; }
        public IExpression Expression { get; set; }

        public IValue Eval()
            => VariablesStorage.IsExist(Name)
                ? Expression.Eval()
                : throw new Exception("Переменной с таким именем не существует!");

        public override string ToString() => Name;

        public override void Execute()
        {
        }
    }
}
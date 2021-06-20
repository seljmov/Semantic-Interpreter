using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Input : SemanticOperator
    {
        public Input(string name)
            => Name = name;

        public string Name { get; }

        public override void Execute()
        {
            var value = Console.ReadLine();
            var variable = VariablesStorageOld.At(Name);
            var expression = variable.Type switch
            {
                SemanticTypes.Integer => new ValueExpression(Convert.ToInt32(value)),
                SemanticTypes.Real => new ValueExpression(Convert.ToDouble(value)),
                SemanticTypes.String => new ValueExpression(value),
                SemanticTypes.Char => value!.Length == 1 
                    ? new ValueExpression(Convert.ToChar(value!)) 
                    : throw new Exception("Неправильная литера!"),
                _ => throw new ArgumentOutOfRangeException()
            };
            variable.Expression = expression;
            VariablesStorageOld.Replace(Name, variable);
        }
    }
}
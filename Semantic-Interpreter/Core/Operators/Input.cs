using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class Input : SemanticOperator
    {
        public Input(string name)
        {
            Name = name;
        }
        
        public string Name { get; set; }

        public override void Execute()
        {
            var value = Console.ReadLine();
            var variable = VariablesStorage.At(Name);
            var expression = variable.Type switch
            {
                SemanticTypes.Integer => new ValueExpression(Convert.ToInt32(value)),
                SemanticTypes.Real => new ValueExpression(Convert.ToDouble(value)),
                SemanticTypes.Boolean => new ValueExpression(Convert.ToInt32(value)),
                SemanticTypes.String => new ValueExpression(value),
                _ => throw new ArgumentOutOfRangeException()
            };
            variable.Expression = expression;
            VariablesStorage.Replace(Name, variable);
        }
        
        public override string ToString() => $"input: {Name}";
    }
}
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
            var type = Parent is BaseFunction function
                ? function.GetParameterWithName(Name).VariableType
                : VariableStorage.At(Name).Type;
            
            var expression = type switch
            {
                VariableType.Integer => new ValueExpression(Convert.ToInt32(value)),
                VariableType.Real => new ValueExpression(Convert.ToDouble(value)),
                VariableType.String => new ValueExpression(value),
                VariableType.Char => value!.Length == 1 
                    ? new ValueExpression(Convert.ToChar(value!)) 
                    : throw new Exception("Неправильная литера!"),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            if (Parent is BaseFunction function2)
            {
                function2.GetParameterWithName(Name).Expression = expression;
            }
            else
            {
                VariableStorage.Replace(Name, expression);   
            }
        }
    }
}
using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class VariableExpression : IExpression
    {
        public VariableExpression(string name)
        {
            Name = name;
        }
        
        private string Name { get; }
        public IValue Eval() 
            => !VariablesStorage.IsExist(Name)
                ? throw new Exception("Переменной с таким именем не существует!")
                : VariablesStorage.At(Name).GetValue();
    }
}
using System;
using System.Linq;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public abstract class MultilineOperator : SemanticOperator
    {
        private readonly uint _idLength = 12;
        
        public abstract string OperatorId { get; set; }
        
        public abstract BlockSemanticOperator Operators { get; set; }

        protected string GenerateOperatorId()
        { 
            Random rng = new Random();
            var letters = new char[_idLength];
            for (var i = 0; i < _idLength; i++)
            {
                letters[i] = (char) (rng.Next('A', 'Z' + 1));
            }
            return new string(letters);
        }
        
        // Удаление использованных переменных блока из хранилища 
        protected void ClearVariableStorage()
        {
            var operators = Operators.Operators
                .Where(x => x is Variable)
                .Cast<Variable>()
                .ToList();

            operators.ForEach(v => VariableStorage.Remove(v.Id));
        }
    }
}
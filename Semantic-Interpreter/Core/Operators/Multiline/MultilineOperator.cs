using System;
using System.Linq;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public abstract class MultilineOperator : SemanticOperator
    {
        private const uint IdLength = 12;

        public abstract string OperatorId { get; }

        protected static string GenerateOperatorId()
        { 
            var rng = new Random();
            var letters = new char[IdLength];
            for (var i = 0; i < IdLength; i++)
            {
                letters[i] = (char) rng.Next('A', 'Z' + 1);
            }
            return new string(letters);
        }
    }
}
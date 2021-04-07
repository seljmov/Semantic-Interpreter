using System;
using System.Collections.Generic;
using System.Text;

namespace Semantic_Interpreter.Core
{
    public class BlockSemanticOperator
    {
        private readonly List<SemanticOperator> _operators;

        public BlockSemanticOperator()
        {
            _operators = new List<SemanticOperator>();
        }

        public void Add(SemanticOperator semanticOperator) => _operators.Add(semanticOperator);

        public void Execute()
        {
            foreach (var @operator in _operators)
            {
                @operator.Execute();
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var @operator in _operators)
            {
                result.Append(@operator.ToString()).Append("\n");
            }

            return result.ToString();
        }
    }
}
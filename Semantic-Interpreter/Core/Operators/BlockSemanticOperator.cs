using System;
using System.Collections.Generic;
using System.Text;

namespace Semantic_Interpreter.Core
{
    public class BlockSemanticOperator
    {
        private readonly List<ISemanticOperator> _operators;

        public BlockSemanticOperator()
        {
            _operators = new List<ISemanticOperator>();
        }

        public void Add(ISemanticOperator semanticOperator) => _operators.Add(semanticOperator);

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
using System;
using System.Collections.Generic;
using System.Text;

namespace Semantic_Interpreter.Core
{
    public class BlockSemanticOperator : SemanticOperator
    {
        public List<SemanticOperator> Operators { get; set; }

        public BlockSemanticOperator()
        {
            Operators = new List<SemanticOperator>();
        }

        public void Add(SemanticOperator semanticOperator) => Operators.Add(semanticOperator);

        public override void Execute()
        {
            foreach (var @operator in Operators)
            {
                @operator.Execute();
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var @operator in Operators)
            {
                result.Append(@operator.ToString()).Append("\n");
            }

            return result.ToString();
        }
    }
}
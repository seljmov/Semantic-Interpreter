using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semantic_Interpreter.Core
{
    public class BlockSemanticOperator : SemanticOperator
    {
        public BlockSemanticOperator()
            => Operators = new List<SemanticOperator>();

        public List<SemanticOperator> Operators { get; }

        public void Add(SemanticOperator semanticOperator) 
            => Operators.Add(semanticOperator);
        
        public override void Execute()
        {
            foreach (var @operator in Operators)
            {
                @operator.Execute();
            }
        }
    }
}
using System.Collections.Generic;

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
            Operators.ForEach(t => t.Execute());
        }
    }
}
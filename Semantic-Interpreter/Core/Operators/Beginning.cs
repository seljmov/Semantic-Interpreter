using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Core
{
    public class Beginning : SemanticOperator
    {
        public Beginning(SemanticOperator parent)
        {
            Parent = parent;
        }
        
        public override void Execute()
        {
        }
    }
}
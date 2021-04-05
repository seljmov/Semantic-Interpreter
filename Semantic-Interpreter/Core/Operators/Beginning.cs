using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Core
{
    public class Beginning : MultilineOperator
    {
        public Beginning(ISemanticOperator parent)
        {
            Parent = parent;
        }
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
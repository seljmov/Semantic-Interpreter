using Semantic_Interpreter.Core.Items.Types;

namespace Semantic_Interpreter.Core
{
    public interface IHaveType
    {
        public ISemanticType Type { get; set; }
    }
}
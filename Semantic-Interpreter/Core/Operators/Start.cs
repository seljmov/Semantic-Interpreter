using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Core
{
    public class Start : MultilineOperator
    {
        public Start() 
            => OperatorID = GenerateOperatorID();
        
        public override string OperatorID { get; set; }
        
        public override void Execute() { }
    }
}
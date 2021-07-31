using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class Root : MultilineOperator
    {
        public Root() => OperatorId = GenerateOperatorId();
        
        public override string OperatorId { get; }
        public List<Module> Imports = new();
        public Module Module { get; set; }
        
        public override void Execute() {}
    }
}
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class Class : MultilineOperator
    {
        public Class() => OperatorId = GenerateOperatorId();
        
        public VisibilityType VisibilityType { get; set; }
        public string Name { get; set; }
        public string BaseClass { get; set; }
        public List<Field> Fields { get; set; }
        public List<DefineFunction> Methods  { get; set; }
        public sealed override string OperatorId { get; }
        public override void Execute() { }
    }
}
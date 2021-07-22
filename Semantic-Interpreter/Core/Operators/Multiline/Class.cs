using System.Collections.Generic;
using Semantic_Interpreter.Core.Operators;

namespace Semantic_Interpreter.Core
{
    public class Class : MultilineOperator, IHaveVisibility, IHaveName
    {
        public Class()
        {
            OperatorId = GenerateOperatorId();
            Operators = new BlockSemanticOperator();
        }
        
        public VisibilityType VisibilityType { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public List<DefineFunction> Methods  { get; set; }
        public sealed override string OperatorId { get; set; }
        public sealed override BlockSemanticOperator Operators { get; set; }
        public override void Execute() { }
    }
}
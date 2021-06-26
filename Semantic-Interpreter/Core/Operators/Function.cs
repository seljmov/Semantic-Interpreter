using System;

namespace Semantic_Interpreter.Core
{
    public class Function : BaseFunction
    {
        public Function() => OperatorID = GenerateOperatorId();
        
        public VariableType ReturnType { get; set; }
        public Return Return { get; set; }
        public sealed override string OperatorID { get; set; }
        
        public override void Execute()
        {
            foreach (var t in Operators.Operators)
            {
                try
                {
                    t.Execute();
                }
                catch (Exception e)
                {
                    Return = (Return) t;
                    throw new Exception();
                }
            }
        }
    }
}
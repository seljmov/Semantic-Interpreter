namespace Semantic_Interpreter.Core
{
    public class Call : SemanticOperator
    {
        public Call(DefineFunction defineFunction)
        {
            DefineFunction = defineFunction;
        }
        
        public DefineFunction DefineFunction { get; set; }
        public override void Execute()
        {
            DefineFunction.Execute();
        }
    }
}
namespace Semantic_Interpreter.Core
{
    public class Call : SemanticOperator
    {
        public Call(string functionName)
        {
            FunctionName = functionName;
        }
        
        public string FunctionName { get; set; }
        public override void Execute()
        {
            var module = FindRoot();
            module.FunctionStorage.At(FunctionName).Execute();
        }
    }
}
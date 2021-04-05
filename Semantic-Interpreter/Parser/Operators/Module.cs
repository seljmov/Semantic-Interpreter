namespace Semantic_Interpreter.Parser.Operators
{
    public class Module : IOperator
    {
        public Module(string name, BlockOperator blockOperator)
        {
            Name = name;
            BlockOperator = blockOperator;
        }
        
        private string Name { get; set; }
        private BlockOperator BlockOperator { get; set; }


        public void Execute()
        {
        }

        public override string ToString()
            => "module: " + Name;
    }
}
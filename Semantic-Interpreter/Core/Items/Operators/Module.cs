namespace Semantic_Interpreter.Core
{
    public class Module : SemanticOperator, IEndNameable
    {
        public Module(string name)
        {
            Beginning = null;
            Name = name;
            EndName = name;
        }
        
        public Beginning Beginning { get; set; }
        public string Name { get; set; }
        public string EndName { get; set; }

        public void SetBeginning(Beginning beginning)
        {
            Beginning = beginning;
            Beginning.Parent = this;
        }
    }
}
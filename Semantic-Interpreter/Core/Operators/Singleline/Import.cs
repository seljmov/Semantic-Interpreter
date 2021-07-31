using System;

namespace Semantic_Interpreter.Core
{
    public class Import : SemanticOperator
    {
        public Import(string name) => Name = name; 
        
        public string Name { get; set; }
        
        public override void Execute()
        {
            Console.WriteLine($"Import {Name} module");
        }
    }
}
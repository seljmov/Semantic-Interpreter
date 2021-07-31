using System;
using Math = Semantic_Interpreter.Modules.Math;

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

        public Module GetImportedModule()
        {
            var module = Name switch
            {
                "Math" => new Math(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            module.Execute();
            return module;
        }
    }
}
using System;
using Semantic_Interpreter.Modules;

namespace Semantic_Interpreter.Core
{
    public class Import : SemanticOperator
    {
        public Import(string name) => Name = name; 
        
        public string Name { get; }
        
        public override void Execute() { }

        public Module GetImportedModule()
        {
            return Name switch
            {
                "MathBase" => new MathBase(),
                "FilesBase" => new FilesBase(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
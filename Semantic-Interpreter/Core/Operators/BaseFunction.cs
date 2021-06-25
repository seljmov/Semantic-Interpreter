using System;
using System.Collections.Generic;
using System.Linq;

namespace Semantic_Interpreter.Core
{
    public abstract class BaseFunction : MultilineOperator
    {
        public VisibilityType VisibilityType { get; set; }
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
        public BlockSemanticOperator Operators { get; set; }

        public Parameter GetParameterWithName(string name)
        {
            foreach (var t in Parameters)
            {
                if (t.Name == name)
                {
                    return t;
                }
            }

            throw new Exception($"Параметра {name} не существует!");
        }

        public bool ParameterIsExist(string name)
        {
            return Parameters.Any(t => t.Name == name);
        }
    }
}
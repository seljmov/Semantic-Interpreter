using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Library;

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
            return ParameterIsExist(name) 
                ? Parameters.Single(x => x.Name == name) 
                : throw new Exception($"Параметра {name} не существует!");
        }

        public bool ParameterIsExist(string name)
        {
            return Parameters.Any(t => t.Name == name);
        }

        protected void VerifyParametersExpressions()
        {
            if (Parameters != null)
            {
                foreach (var t in Parameters.Where(x => x.ParameterType == ParameterType.Var))
                {
                    var variable = VariableStorage.At(t.VariableId);
                    if (t.Expression != variable.Expression)
                    {
                        VariableStorage.Replace(t.VariableId, t.Expression);
                    }
                }
            }
        }
    }
}
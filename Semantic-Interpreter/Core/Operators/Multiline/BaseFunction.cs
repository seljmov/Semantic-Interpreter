using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public abstract class BaseFunction : MultilineOperator, IHaveBlock
    {
        public VisibilityType VisibilityType { get; set; }
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
        
        public List<SemanticOperator> Block { get; set; }

        public Parameter GetParameterWithName(string name)
            => ParameterIsExist(name) 
                ? Parameters.Single(x => x.Name == name) 
                : throw new Exception($"Параметра {name} не существует!");

        public bool ParameterIsExist(string name)
            => Parameters.Any(t => t.Name == name);

        protected void VerifyParametersExpressions()
        {
            if (Parameters != null)
            {
                foreach (var t in Parameters.Where(x => x.ParameterType == ParameterType.Var))
                {
                    if (t.Operator is Variable variable)
                    {
                        if (t.Expression != variable.Expression)
                        {
                            var module = GetRoot().Module;
                            module.VariableStorage.Replace(variable.Id, t.Expression);
                        }
                    }

                    if (t.Operator is Parameter parameter)
                    {
                        if (t.Expression != parameter.Expression)
                        {
                            parameter.Expression = t.Expression;
                        }
                    }
                }
            }
        }
    }
}
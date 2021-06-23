using System;
using System.Collections.Generic;
using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Library
{
    public class VariableStorage
    {
        private readonly Dictionary<string, Variable> _variables = new();

        public bool IsExist(string name) => _variables.ContainsKey(name);

        public Variable At(string name)
            => IsExist(name) 
                ? _variables[name] 
                : throw new Exception("Переменной с таким именем не существует!");

        public string AtId(Variable variable)
        {
            foreach (var (key, value) in _variables)
            {
                if (value == variable)
                {
                    return key;
                }
            }

            return null;
        }
        
        public void Add(string name, Variable variable)
        {
            if (IsExist(name))
            {
                throw new Exception("Переменная с таким именем уже есть!");
            }

            _variables.Add(name, variable);
        }

        public void Replace(string name, Variable variable)
        {
            if (!IsExist(name))
            {
                throw new Exception("Переменной с таким именем не существует!");
            }

            _variables[name] = variable;
        }
        
        public void Replace(string name, IExpression expression)
        {
            if (!IsExist(name))
            {
                throw new Exception("Переменной с таким именем не существует!");
            }

            _variables[name].Expression = expression;
        }

        public void RenameKey(string oldKey, string newKey)
        {
            var value = _variables[oldKey];
            _variables.Remove(oldKey);
            _variables.Add(newKey, value);
        }

        public void Clear() => _variables.Clear();
    }
}
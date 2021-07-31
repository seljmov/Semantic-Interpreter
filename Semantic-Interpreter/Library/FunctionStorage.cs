using System;
using System.Collections.Generic;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Library
{
    public class FunctionStorage
    {
        private readonly Dictionary<string, IFunction> _functions = new();

        public bool IsExist(string name) => _functions.ContainsKey(name);

        public IFunction At(string name)
            => IsExist(name) 
                ? _functions[name] 
                : throw new Exception("Функции/Процедуры с таким именем не существует!");
        
        public void Add(string name, IFunction function)
        {
            if (IsExist(name))
            {
                throw new Exception("Функция/Процедура с таким именем уже существует!");
            }

            _functions.Add(name, function);
        }
    }
}
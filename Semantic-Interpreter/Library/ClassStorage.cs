using System;
using System.Collections.Generic;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Library
{
    public class ClassStorage
    {
        private readonly Dictionary<string, Class> _classes = new();

        public bool IsExist(string name) => _classes.ContainsKey(name);

        public Class At(string name)
            => IsExist(name) 
                ? _classes[name] 
                : throw new Exception($"Класс {name} не был объявлен!");
        
        public void Add(string name, Class cClass)
        {
            if (IsExist(name))
            {
                throw new Exception($"Класс {name} уже объявлен!");
            }

            _classes.Add(name, cClass);
        }
    }
}
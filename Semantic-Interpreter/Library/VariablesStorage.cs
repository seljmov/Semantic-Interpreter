using System;
using System.Collections.Generic;
using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Library
{
    // TODO: Перенести хранилище переменных в модуль
    public static class VariablesStorage
    {
        private static readonly Dictionary<string, Variable> Variables = new();

        public static bool IsExist(string name) => Variables.ContainsKey(name);

        public static Variable At(string name)
            => IsExist(name) 
                ? Variables[name] 
                : throw new Exception("Переменной с таким именем не существует!");

        public static void Add(string name, Variable variable)
        {
            if (IsExist(name))
            {
                throw new Exception("Переменная с таким именем уже есть!");
            }

            Variables.Add(name, variable);
        }

        public static void Replace(string name, Variable variable)
        {
            if (!IsExist(name))
            {
                throw new Exception("Переменной с таким именем не существует!");
            }

            Variables[name] = variable;
        }

        public static void Clear() => Variables.Clear();
    }
}
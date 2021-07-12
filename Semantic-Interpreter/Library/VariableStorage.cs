using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Library
{
    public static class VariableStorage
    {
        public static readonly Dictionary<string, Variable> Variables = new();

        public static bool IsExist(string id) => Variables.ContainsKey(id);

        public static Variable At(string id)
            => IsExist(id) 
                ? Variables[id] 
                : throw new Exception($"Переменная {GetName(id)} не определена в модуле!");
        
        public static void Add(string id, Variable variable)
        {
            if (IsExist(id))
            {
                throw new Exception($"Переменная {GetName(id)} уже определена в этом блоке!");
            }

            Variables.Add(id, variable);
        }

        public static void Remove(string id)
        {
            if (!IsExist(id))
            {
                throw new Exception($"Переменная {GetName(id)} не определена в модуле!");
            }

            Variables.Remove(id);
        }
        
        public static void Replace(string id, IExpression expression)
        {
            if (!IsExist(id))
            {
                throw new Exception($"Переменная {GetName(id)} не определена в модуле!");
            }

            Variables[id].Expression = expression;
        }

        public static void Clear() => Variables.Clear();

        private static string GetName(string id)
            => id.Split("^").Last();
    }
}
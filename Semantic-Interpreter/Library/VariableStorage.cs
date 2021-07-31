using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Library
{
    public class VariableStorage
    {
        public readonly Dictionary<string, Variable> Variables = new();

        public bool IsExist(string id) => Variables.ContainsKey(id);

        public Variable At(string id)
            => IsExist(id) 
                ? Variables[id] 
                : throw new Exception($"Переменная {GetName(id)} не определена в модуле!");
        
        public void Add(string id, Variable variable)
        {
            if (IsExist(id))
            {
                throw new Exception($"Переменная {GetName(id)} уже определена в этом блоке!");
            }

            Variables.Add(id, variable);
        }

        public void Remove(string id)
        {
            if (!IsExist(id))
            {
                throw new Exception($"Переменная {GetName(id)} не определена в модуле!");
            }

            Variables.Remove(id);
        }
        
        public void Replace(string id, IExpression expression)
        {
            if (!IsExist(id))
            {
                throw new Exception($"Переменная {GetName(id)} не определена в модуле!");
            }

            Variables[id].Expression = expression;
        }

        public void Clear() => Variables.Clear();

        private string GetName(string id) => id.Split("^").Last();
    }
}
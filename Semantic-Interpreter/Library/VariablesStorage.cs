using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Parser.Expressions;

namespace Semantic_Interpreter.Library
{
    public static class VariablesStorage
    {
        private static readonly Dictionary<string, VariableExpression> Variables = new();

        public static bool IsExist(string name) => Variables.ContainsKey(name);

        public static VariableExpression At(string name)
            => IsExist(name) 
                ? Variables[name] 
                : throw new Exception("Переменной с таким именем не существует!");

        public static void Add(string name, VariableExpression variable)
        {
            if (IsExist(name))
            {
                throw new Exception("Переменная с таким именем уже есть!");
            }

            Variables.Add(name, variable);
        }

        public static void Replace(string name, VariableExpression variable)
        {
            if (!IsExist(name))
            {
                throw new Exception("Переменной с таким именем не существует!");
            }

            Variables[name] = variable;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Semantic_Interpreter.Core
{
    public static class VariablesStorage
    {
        private static ArrayList _vars;

        public static void Add(Variable variable)
            => _vars.Add(variable);

        public static void Replace(string name, Variable variable)
        {
            foreach (var _var in _vars)
            {
                if (((Variable) _var).Name != name) continue;
                
                ((Variable)_var).Type = variable.Type;
                ((Variable)_var).Value = variable.Value;
                break;
            }
        }
    }
}
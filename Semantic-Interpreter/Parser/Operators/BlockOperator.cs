using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Semantic_Interpreter.Parser.Operators
{
    public class BlockOperator : IOperator
    {
        private readonly List<IOperator> _operators;

        public BlockOperator()
        {
            _operators = new List<IOperator>();
        }

        public void Add(IOperator @operator) => _operators.Add(@operator);

        public void Execute()
        {
            foreach (var @operator in _operators)
            {
                @operator.Execute();
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var @operator in _operators)
            {
                result.Append(@operator.ToString()).Append("\n");
            }

            return result.ToString();
        }
    }
}
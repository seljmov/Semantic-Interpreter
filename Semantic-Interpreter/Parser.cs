using System;
using Semantic_Interpreter.Core;

namespace Semantic_Interpreter
{
    public class Parser
    {
        private readonly SemanticTree _tree;
        private readonly string _program;
        private int _pos = 0;
        private SemanticOperator _lastOperator;

        public Parser(SemanticTree tree, string program)
        {
            _tree = tree;
            _program = program;
        }

        public void BuildTree()
        {
            while (_pos < _program.Length)
            {
                var ch = PeekSymbol(0);
                if (!IsNormalSymbol(ch))
                {
                    _pos++;
                    continue;
                }
                
                if (char.IsLetter(ch))
                {
                    ReadWord();
                }
            }
        }

        private void ReadWord()
        {
            SemanticOperator currentOperator = null;
            var word = "";
            
            var ch = PeekSymbol(0);
            while (IsNormalSymbol(ch))
            {
                word += ch;
                _pos++;
                ch = PeekSymbol(0);
            }

            switch (word)
            {
                case "module":
                {
                    currentOperator = new Module("mod");
                    _tree.InsertOperator(_lastOperator, currentOperator, false);
                    break;
                }
                case "start":
                {
                    currentOperator = new Beginning(_lastOperator);
                    _tree.InsertOperator(_lastOperator, currentOperator, true);
                    break;
                }
                case "output":
                {
                    currentOperator = new Output("Hi");
                    _tree.InsertOperator(_lastOperator, currentOperator, true);
                    break;
                }
                default: return;
            }

            _lastOperator = currentOperator;
        }

        private char PeekSymbol(int i) => _program[_pos + i];

        private static bool IsNormalSymbol(char ch)
            => !(ch == ' ' || ch == '\r' || ch == '\n' || ch == '"' || ch == ';' || ch == '.');
    }
}
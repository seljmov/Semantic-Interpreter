using System;
using System.Collections.Generic;
using System.Globalization;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Parser
{
    public class Parser
    {
        private static readonly Token Eof = new(TokenType.Eof, "");

        private readonly List<Token> _tokens;
        private readonly int _length;

        private int _pos;
        private readonly SemanticTree _semanticTree = new();

        private SemanticOperator _lastOperator;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _length = tokens.Count;
            _pos = 0;
        }

        public SemanticTree Parse()
        {
            Stack<SemanticOperator> operatorsStack = new();
            while (!Match(TokenType.Eof))
            {
                if (Match(TokenType.Module))
                {
                    var module = ModuleOperator();
                    _semanticTree.InsertOperator(module);
                    operatorsStack.Push(module);
                    _lastOperator = module;
                }
                else if (Match(TokenType.Beginning))
                {
                    var parent = operatorsStack.Pop();
                    var beginning = new Beginning(parent);
                    _semanticTree.InsertOperator(beginning, parent);
                    operatorsStack.Push(beginning);
                    _lastOperator = beginning;
                }
                else if (Match(TokenType.Variable))
                {
                    var parent = operatorsStack.Peek();
                    var variable = VariableOperator();
                    _semanticTree.InsertOperator(variable, parent, true);
                    _lastOperator = variable;
                }
                else if (Match(TokenType.Output))
                {
                    var parent = operatorsStack.Peek();
                    var output = new Output(ParseExpression());
                    Consume(TokenType.Semicolon);
                    var asChild = parent.Child == null;
                    _semanticTree.InsertOperator(output, _lastOperator, asChild);
                    _lastOperator = output;
                }
                else if (Match(TokenType.End))
                {
                    Consume(TokenType.Word);
                    Consume(TokenType.Dot);
                }
            }

            return _semanticTree;
        }

        private SemanticOperator ModuleOperator()
        {
            var name = Consume(TokenType.Word).Text;
            return new Module(name);
        }

        private SemanticOperator VariableOperator()
        {
            Consume(TokenType.Dash);  // Scip -
            var type = Consume(TokenType.Word).Text switch
            {
                "integer" => SemanticTypes.Integer,
                "real" => SemanticTypes.Real,
                "boolean" => SemanticTypes.Boolean,
                _ => SemanticTypes.String,
            };
            var name = Get().Text;
            IExpression expression = null;
            if (Match(TokenType.Word) && Get().Type == TokenType.Assing)
            {
                Consume(TokenType.Assing);
                expression = ParseExpression();
            }
            var variable = new Variable(type, name, expression);
            VariablesStorage.Add(name, variable);
            Consume(TokenType.Semicolon);
            return variable;
        }
        
        
        private IExpression ParseExpression()
        {
            return Primary();
        }

        private IExpression Primary()
        {
            var current = Get();
            if (Match(TokenType.Number))
            {
                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                return new ValueExpression(Convert.ToDouble(current.Text, formatter));
            }

            if (Match(TokenType.Word))
            {
                return VariablesStorage.At(current.Text);
            }
            
            if (Match(TokenType.Text))
            {
                return new ValueExpression(current.Text);
            }

            throw new Exception("Неизвестный оператор.");
        }
        
        private Token Consume(TokenType type)
        {
            var current = Get();
            if (type != current.Type) throw new Exception($"Токен '{current}' не найден ({type}).");
            _pos++;
            return current;
        }

        private bool Match(TokenType type)
        {
            var current = Get();
            if (type != current.Type) return false;
            _pos++;
            return true;
        }

        private Token Get(int i = 0)
        {
            var position = _pos + i;
            if (position >= _length) return Eof;
            return _tokens[position];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Parser
{
    public class Parser
    {
        private static readonly Token Eof = new Token(TokenType.Eof, "");

        private readonly List<Token> _tokens;
        private readonly int _length;

        private int _pos;
        private readonly SemanticTree _semanticTree = new SemanticTree();

        private SemanticOperator _lastOperator = null;
        private bool _asChild = false;
        
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

            // while (!Match(TokenType.Eof))
            // {
            //     var @operator = ParseOperator();
            //     _semanticTree.InsertOperator(@operator, _lastOperator, _asChild);
            //     _lastOperator = @operator;
            //     _asChild = false;
            // }

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
        
        private SemanticOperator ParseOperator()
        {
            if (Match(TokenType.Module))
            {
                var name = Consume(TokenType.Word).Text;
                return new Module(name);
            }
            else if (Match(TokenType.Beginning))
            {
                return new Beginning(_lastOperator);
            }
            else if (Match(TokenType.Output))
            {
                _asChild = true;
                return new Output(ParseExpression());
            }

            return null;
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

        /*
        private BlockSemanticOperator ParseBlock()
        {
            var block = new BlockSemanticOperator();
            var token = Get(-1);
            var endtype = TokenType.End;
            if (Get(0).Type == TokenType.Module || Get(-1).Type == TokenType.Module)
            {
                endtype = TokenType.End;
            }
            
            // Consume(token.Type);
            while (!Match(endtype))
            {
                block.Add(ParseOperator());
            }

            return block;
        }
        
        private SemanticOperator OperatorOrBlock()
        {
            var type = Get().Type;
            if (type == TokenType.Module || type == TokenType.Beginning)
            {
                return ParseBlock();
            }

            return ParseOperator();
        }
        
        private SemanticOperator ParseOperator()
        {
            if (Match(TokenType.Module))
            {
                return ModuleOperator();
            }
            else if (Match(TokenType.Beginning))
            {
                
            }
            else if (Match(TokenType.Output))
            {
                var @operator = new Output(ParseExpression());
                Consume(TokenType.Semicolon);
                return @operator;
            }
            else if (Match(TokenType.Variable))
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

            return AssignOperator();
        }

        private SemanticOperator AssignOperator()
        {
            var current = Get();
            if (Match(TokenType.Word) && Get().Type == TokenType.Assing)
            {
                var variable = current.Text;
                Consume(TokenType.Assing);
                return new Assign(variable, ParseExpression());
            }

            throw new Exception("Неизвестный оператор.");
        }

        private SemanticOperator ModuleOperator()
        {
            var name = Get(0).Text;
            var blockOperator = ParseBlock();
            return new Module(name, blockOperator);
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
        */
        
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
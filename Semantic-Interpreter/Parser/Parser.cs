﻿using System;
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

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _length = tokens.Count;
            _pos = 0;
        }

        public List<ISemanticOperator> Parse()
        {
            var result = new List<ISemanticOperator>();
            while (!Match(TokenType.Eof))
            {
                result.Add(ParseOperator());
            }

            return result;
        }

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
        
        private ISemanticOperator OperatorOrBlock()
        {
            var type = Get().Type;
            if (type == TokenType.Module || type == TokenType.Beginning)
            {
                return ParseBlock();
            }

            return ParseOperator();
        }
        
        private ISemanticOperator ParseOperator()
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

        private ISemanticOperator AssignOperator()
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

        private ISemanticOperator ModuleOperator()
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
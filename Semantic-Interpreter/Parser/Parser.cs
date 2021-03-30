using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using Semantic_Interpreter.Library;
using Semantic_Interpreter.Parser.Expressions;
using Semantic_Interpreter.Parser.Operators;

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

        public List<IOperator> Parse()
        {
            var result = new List<IOperator>();
            while (!Match(TokenType.Eof))
            {
                result.Add(ParseOperator());
            }

            return result;
        }

        private IOperator ParseOperator()
        {
            if (Match(TokenType.Output))
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
                var variable = new VariableExpression(type, name, expression);
                VariablesStorage.Add(name, variable);
                Consume(TokenType.Semicolon);
                return variable;
            }

            return AssignOperator();
        }

        private IOperator AssignOperator()
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
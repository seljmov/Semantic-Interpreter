using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualBasic;

namespace Semantic_Interpreter.Parser
{
    public class Lexer
    {
        private const string OperatorsChars = "+-*/";
        private Dictionary<string, TokenType> _operators = new()
        {
            {"+", TokenType.Plus},
            {"-", TokenType.Minus},
            {"*", TokenType.Multiply},
            {"/", TokenType.Divide},
        };

        private readonly string _program;
        private readonly int _lenght;
        
        private readonly List<Token> _tokens;
        private int _pos;

        public Lexer(string program)
        {
            _program = program;
            _lenght = program.Length;

            _tokens = new List<Token>();
            _pos = 0;
        }

        public List<Token> Tokenize()
        {
            while (_pos < _lenght)
            {
                var curr = Peek();
                if (char.IsDigit(curr))
                {
                    TokenizeNumber();
                }
                else if (char.IsLetter(curr))
                {
                    TokenizeWord();
                }
                else if (curr == '"')
                {
                    TokenizeText();
                }
                // Example ,./\\;:=+-_*'\"#@!&|<>[]{}
                else if (IsNotLetterOrDigit(curr))
                {
                    var token = curr switch
                    {
                        '.' => TokenType.Dot,
                        '-' => TokenType.Dash,
                        ';' => TokenType.Semicolon,
                        ':' => TokenizeSymbol(curr),
                        _ => TokenType.NotFound,
                    };
                    AddToken(token);
                    Next();
                }
                else Next();
            }
            AddToken(TokenType.Eof);
            
            return _tokens;
        }

        private void TokenizeNumber()
        {
            var buffer = "";
            var current = Peek();
            while (char.IsDigit(current) || current == '.')
            {
                if (current == '.')
                {
                    if (buffer.IndexOf(".", StringComparison.Ordinal) != -1)
                    {
                        throw new Exception("Неправильное вещественное число!");
                    }
                }

                buffer += current;
                current = Next();
            }
            AddToken(TokenType.Number, buffer);
        }
        
        private void TokenizeWord()
        {
            var buffer = new StringBuilder();
            var curr = Peek();
            while (char.IsLetterOrDigit(curr))
            {
                buffer.Append(curr);
                curr = Next();
            }

            var word = buffer.ToString();
            switch (word)
            {
                case "module": AddToken(TokenType.Module); break;
                case "beginning": AddToken(TokenType.Beginning); break;
                case "variable": AddToken(TokenType.Variable); break;
                case "assign": AddToken(TokenType.Assing); break;
                case "output": AddToken(TokenType.Output); break;
                case "end": AddToken(TokenType.End); break;
                default: AddToken(TokenType.Word, word); break;
            }
        }

        private void TokenizeText()
        {
            Next(); // Scip "
            var buffer = new StringBuilder();
            var curr = Peek();
            while (true)
            {
                if (curr == '\\')
                {
                    curr = Next();
                    switch (curr)
                    {
                        case '"': curr = Next(); buffer.Append('"'); continue;
                        case 'n': curr = Next(); buffer.Append('\n'); continue;
                        case 't': curr = Next(); buffer.Append('\t'); continue;
                    }

                    buffer.Append('\\');
                    continue;
                }
                
                if (curr == '"') break;
                buffer.Append(curr);
                curr = Next();
            }

            Next(); // Scip closing "
            
            AddToken(TokenType.Text, buffer.ToString());
        }

        private TokenType TokenizeSymbol(char ch)
        {
            var curr = Next();
            switch (ch)
            {
                case ':':
                {
                    if (curr == '=')
                    {
                        return TokenType.Assing;
                    }
                    break;
                }

                case '<':
                {
                    if (curr == '=')
                    {
                        // token = 
                    }
                    break;
                }
            }

            return TokenType.NotFound;
        }
        
        private char Peek(int i = 0)
        {
            var position = _pos + i;
            return position >= _lenght ? '\0' : _program[position];
        }

        private char Next()
        {
            _pos++;
            return Peek();
        }

        private void AddToken(TokenType type, string text = "")
            => _tokens.Add(new Token(type, text));

        private bool IsNotLetterOrDigit(char ch)
            => ",./\\;:=+-*'\"#&|<>[]".Contains(ch);
    }
}
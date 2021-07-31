using System;
using System.Collections.Generic;
using System.Text;

namespace Semantic_Interpreter.Parser
{
    public class Lexer
    {
        private readonly Dictionary<string, TokenType> _operators = new()
        {
            {"import", TokenType.Import},
            {"module", TokenType.Module},
            {"start", TokenType.Start},
            {"class", TokenType.Class},
            {"inherits", TokenType.Inherits},
            {"field", TokenType.Field},
            {"method", TokenType.Method},
            {"function", TokenType.Function},
            {"procedure", TokenType.Procedure},
            {"call", TokenType.Call},
            {"while", TokenType.While},
            {"if", TokenType.If},
            {"else", TokenType.Else},
            {"variable", TokenType.Variable},
            {"let", TokenType.Let},
            {"input", TokenType.Input},
            {"output", TokenType.Output},
            {"end", TokenType.End},
            {"return", TokenType.Return},

            {"+", TokenType.Plus},
            {"-", TokenType.Minus},
            {"*", TokenType.Multiply},
            {"/", TokenType.Divide},
            {":=", TokenType.Assign},
            {"<", TokenType.Less},
            {">", TokenType.Greater},
            {"(", TokenType.LParen},
            {")", TokenType.RParen},
            {"[", TokenType.LBracket},
            {"]", TokenType.RBracket},
            {".", TokenType.Dot},
            {";", TokenType.Semicolon},
            {":", TokenType.Colon},
            {",", TokenType.Comma},
            
            {"!=", TokenType.NotEqual},
            {"==", TokenType.Equal},
            {"<=", TokenType.LessOrEqual},
            {">=", TokenType.GreaterOrEqual},
            
            {"&&", TokenType.AndAnd},
            {"||", TokenType.OrOr},
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
                else if (curr == '\'')
                {
                    TokenizeChar();
                }
                // Example ,./\\;:=+-_*#@!&|<>()[]
                else if (IsNotLetterOrDigit(curr))
                {
                    TokenizeSymbol(curr);
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
            var cond = _operators.ContainsKey(word);

            if (word == "true" || word == "false")
            {
                AddToken(TokenType.Boolean, word);
            }
            else if (word == "private" || word == "public")
            {
                AddToken(TokenType.VisibilityType, word);
            }
            else
            {
                AddToken(
                    cond ? _operators[word] : TokenType.Word, 
                    cond ? "" : word
                );
            }
        }

        private void TokenizeText()
        {
            Next(); // Skip "
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

                if (curr == '$')
                {
                    if (Next() == '{')
                    {
                        buffer.Append(curr);
                        AddToken(TokenType.Text, buffer.ToString());

                        curr = Next();
                        while (curr != '}')
                        {
                            curr = Peek();
                            if (char.IsDigit(curr))
                            {
                                TokenizeNumber();
                            }
                            else if (char.IsLetter(curr))
                            {
                                TokenizeWord();
                            }
                            // Example ,./\\;:=+-_*#@!&|<>()[]
                            else if (IsNotLetterOrDigit(curr))
                            {
                                TokenizeSymbol(curr);
                            }
                            else Next();
                        }

                        curr = Peek(); // Peek closing "
                        buffer.Clear();
                    }
                }
                
                if (curr == '"') break;
                buffer.Append(curr);
                curr = Next();
            }

            Next(); // Skip closing "
            
            AddToken(TokenType.Text, buffer.ToString());
        }

        private void TokenizeChar()
        {
            Next(); // Skip '
            var curr = Peek();
            Next(); // Skip char symbol
            if (Peek() != '\'')
            {
                throw new Exception("Неправильная литера!");
            }

            Next(); // Skip '
            
            AddToken(TokenType.Char, curr.ToString());
        }

        private void TokenizeSymbol(char ch)
        {
            var next = Peek(1);
            var symbol = ch.ToString();
            if (next == '=')
            {
                symbol += next;
                Next(); // Skip =
            }


            var token = _operators.ContainsKey(symbol) 
                ? _operators[symbol] 
                : TokenType.NotFound;
                    
            AddToken(token);
            Next();
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

        private static bool IsNotLetterOrDigit(char ch)
            => ",./\\;:=+-_*#@!&|<>()[]".Contains(ch);
    }
}
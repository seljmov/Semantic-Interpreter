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
        
        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _length = tokens.Count;
            _pos = 0;
        }

        public SemanticTree Parse()
        {
            Stack<SemanticOperator> operatorsStack = new();
            SemanticOperator lastOperator = null;
            
            while (!Match(TokenType.Eof))
            {
                if (Match(TokenType.End)) return _semanticTree;

                var prevOperator = lastOperator;
                var newOperator = ParseOperator();
                var asChild = false;
                // Операторы Module и Beginning имеют индивидуальные правила, по которым они вставляются
                // в дерево, а остальные операторы вставляются по общим правилам (кейс default)
                switch (newOperator)
                {
                    case Module:
                        operatorsStack.Push(newOperator);
                        break;
                    case Start:
                        asChild = operatorsStack.Pop().Child == null;
                        operatorsStack.Push(newOperator);
                        break;
                    default:
                        asChild = operatorsStack.Peek().Child == null;
                        break;
                }
                
                _semanticTree.InsertOperator(prevOperator, newOperator, asChild);
                lastOperator = newOperator;
            }

            return _semanticTree;
        }

        private SemanticOperator ParseOperator()
        {
            var token = Get();
            _pos++;
            return token.Type switch
            {
                TokenType.Module => ParseModuleOperator(),
                TokenType.Start => ParseStartOperator(),
                TokenType.While => ParseWhileOperator(),
                TokenType.If => ParseIfOperator(),
                TokenType.Variable => ParseVariableOperator(),
                TokenType.Let => ParseLetOperator(),
                TokenType.Input => ParseInputOperator(),
                TokenType.Output => ParseOutputOperator(),
                _ => null
            };
        }
        
        private SemanticOperator ParseModuleOperator()
        {
            var name = Consume(TokenType.Word).Text;
            return new Module(name);
        }

        private SemanticOperator ParseStartOperator() => new Start();
        
        private SemanticOperator ParseWhileOperator()
        {
            var expression = ParseExpression();
            Consume(TokenType.Word); // Skip repeat
            var block = new BlockSemanticOperator();
            while (!Match(TokenType.End))
            {
                block.Add(ParseOperator());
            }

            Consume(TokenType.While);   // Skip while word
            Consume(TokenType.Semicolon);   // Skip ;
            return new While(expression, block);
        }

        private SemanticOperator ParseIfOperator()
        {
            var expression = ParseExpression();
            Consume(TokenType.Word);    // Skip then
            
            BlockSemanticOperator @if = new();
            List<ElseIf> elseIfs = null;
            Else @else = null;
            
            var currentToken = Get();
            while (!Match(TokenType.End))
            {
                if (currentToken.Type != TokenType.Else)
                {
                    @if.Add(ParseOperator());
                }
                else
                {
                    Consume(TokenType.Else);
                    if (Match(TokenType.If))
                    {
                        elseIfs ??= new List<ElseIf>();
                        var elseIfExpr = ParseExpression();
                        Consume(TokenType.Word);    // Skip then
                        BlockSemanticOperator elseIfBlock = new();
                        while (Get().Type != TokenType.Else && Get().Type != TokenType.End)
                        {
                            elseIfBlock.Add(ParseOperator());
                        }

                        var elseIf = new ElseIf(elseIfExpr, elseIfBlock);
                        elseIfs.Add(elseIf);
                    }
                    else
                    {
                        BlockSemanticOperator elseBlock = new();
                        while (Get().Type != TokenType.End)
                        {
                            elseBlock.Add(ParseOperator());
                        }

                        @else = new Else(elseBlock);
                    }
                }

                currentToken = Get();
            }
            
            Consume(TokenType.If);      // Skip if word
            Consume(TokenType.Semicolon);   // Skip ;
            return new If(expression, @if, elseIfs, @else);
        }
        
        private SemanticOperator ParseVariableOperator()
        {
            Consume(TokenType.Minus);  // Skip -
            var type = Consume(TokenType.Word).Text switch
            {
                "integer" => SemanticTypes.Integer,
                "real" => SemanticTypes.Real,
                "boolean" => SemanticTypes.Boolean,
                "char" => SemanticTypes.Char,
                _ => SemanticTypes.String
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

        private SemanticOperator ParseLetOperator()
        {
            var variable = Consume(TokenType.Word).Text;
            Consume(TokenType.Assing);
            var expression = ParseExpression();
            Consume(TokenType.Semicolon);
                
            return new Let(variable, expression);
        }

        private SemanticOperator ParseInputOperator()
        {
            var name = Consume(TokenType.Word).Text;
            Consume(TokenType.Semicolon);

            return new Input(name);
        }

        private SemanticOperator ParseOutputOperator()
        {
            var expression = ParseExpression();
            Consume(TokenType.Semicolon);

            return new Output(expression);
        }
        
        private IExpression ParseExpression()
        {
            return LogicalOr();
        }

        private IExpression LogicalOr()
        {
            var result = LogicalAnd();

            while (true)
            {
                if (Match(TokenType.OrOr))
                {
                    result = new ConditionalExpression(TokenType.OrOr, result, LogicalAnd());
                    continue;
                }
                break;
            }

            return result;
        }

        private IExpression LogicalAnd()
        {
            var result = Equality();

            while (true)
            {
                if (Match(TokenType.AndAnd))
                {
                    result = new ConditionalExpression(TokenType.AndAnd, result, Equality());
                    continue;
                }
                
                break;
            }

            return result;
        }

        private IExpression Equality()
        {
            var result = Conditional();

            if (Match(TokenType.Equal))
            {
                return new ConditionalExpression(TokenType.Equal, result, Conditional());
            }

            if (Match(TokenType.NotEqual))
            {
                return new ConditionalExpression(TokenType.NotEqual, result, Conditional());
            }

            return result;
        }

        private IExpression Conditional()
        {
            var result = Additive();

            while (true)
            {
                if (Match(TokenType.Less))
                {
                    result = new ConditionalExpression(TokenType.Less, result, Additive());
                    continue;
                }

                if (Match(TokenType.LessOrEqual))
                {
                    result = new ConditionalExpression(TokenType.LessOrEqual, result, Additive());
                    continue;
                }

                if (Match(TokenType.Greater))
                {
                    result = new ConditionalExpression(TokenType.Greater, result, Additive());
                    continue;
                }

                if (Match(TokenType.GreaterOrEqual))
                {
                    result = new ConditionalExpression(TokenType.GreaterOrEqual, result, Additive());
                    continue;
                }
                
                break;
            }

            return result;
        }

        private IExpression Additive()
        {
            var result = Multiplicative();
            
            while (true)
            {
                if (Match(TokenType.Plus))
                {
                    result = new BinaryExpression(Operations.Plus, result, Multiplicative());
                    continue;
                }

                if (Match(TokenType.Minus))
                {
                    result = new BinaryExpression(Operations.Minus, result, Multiplicative());
                    continue;
                }
                
                break;
            }

            return result;
        }

        private IExpression Multiplicative()
        {
            var result = Unary();

            while (true)
            {
                if (Match(TokenType.Multiply))
                {
                    result = new BinaryExpression(Operations.Multiply, result, Unary());
                    continue;
                }

                if (Match(TokenType.Divide))
                {
                    result = new BinaryExpression(Operations.Divide, result, Unary());
                    continue;
                }
                
                break;
            }

            return result;
        }

        private IExpression Unary()
        {
            return Match(TokenType.Minus) 
                ? new UnaryExpression(Operations.Minus, Primary()) 
                : Primary();
        }
        
        private IExpression Primary()
        {
            var current = Get();
            _pos++;
            switch (current.Type)
            {
                case TokenType.Word: return VariablesStorage.At(current.Text);
                case TokenType.Boolean: return new ValueExpression(current.Text == "true");
                case TokenType.Char: return new ValueExpression(Convert.ToChar(current.Text));
                case TokenType.Text: return new ValueExpression(current.Text);
                case TokenType.Number:
                    // Если точки нет, то число целое, иначе - вещественное
                    if (!current.Text.Contains('.'))
                        return new ValueExpression(Convert.ToInt32(current.Text));
                
                    IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                    return new ValueExpression(Convert.ToDouble(current.Text, formatter));
                case TokenType.LParen:
                    var result = ParseExpression();
                    Match(TokenType.RParen);
                    return result;
            }
            
            throw new Exception("Неизвестный оператор.");
        }
        
        private Token Consume(TokenType type)
        {
            var current = Get();
            if (type != current.Type) 
                throw new Exception($"Токен '{current}' не найден ({type}).");
            
            _pos++;
            return current;
        }

        private bool Match(TokenType type)
        {
            var current = Get();
            if (type != current.Type) 
                return false;
            
            _pos++;
            return true;
        }

        private Token Get(int i = 0)
        {
            var position = _pos + i;
            return position >= _length 
                ? Eof 
                : _tokens[position];
        }
    }
}
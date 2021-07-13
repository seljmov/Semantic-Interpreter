using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Parser
{
    public class Parser
    {
        private static readonly Token Eof = new(TokenType.Eof, "");

        private readonly List<Token> _tokens;
        private readonly int _length;

        private int _pos;
        private readonly SemanticTree _semanticTree = new();
        private readonly Stack<MultilineOperator> _operatorsStack = new();
        
        private List<Variable> _variables = new();
        private List<Parameter> _parameters = new();
        
        private Func<bool> IsFunctionParsed => () => _operatorsStack.Any(x => x is BaseFunction);

        private delegate bool ParseBlockPredicate();

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _length = tokens.Count;
            _pos = 0;
        }

        public SemanticTree Parse()
        {
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
                    case Module module:
                        _operatorsStack.Push(module);
                        break;
                    case Start start:
                        asChild = _operatorsStack.Peek().Child == null;
                        _operatorsStack.Push(start);
                        break;
                    default:
                        asChild = _operatorsStack.Peek().Child == null;
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
            var @operator = token.Type switch
            {
                TokenType.Module => ParseModuleOperator(),
                TokenType.Start => ParseStartOperator(),
                TokenType.VisibilityType => ParseVisibilityOperator(),
                TokenType.Return => ParseReturnOperator(),
                TokenType.While => ParseWhileOperator(),
                TokenType.If => ParseIfOperator(),
                TokenType.Call => ParseCallOperator(),
                TokenType.Variable => ParseVariableOperator(),
                TokenType.Let => ParseLetOperator(),
                TokenType.Input => ParseInputOperator(),
                TokenType.Output => ParseOutputOperator(),
                _ => null
            };
            @operator.Parent = _operatorsStack.Count > 0 ? _operatorsStack.Peek() : null;
            return @operator;
        }
        
        private SemanticOperator ParseModuleOperator()
        {
            var name = Consume(TokenType.Word).Text;
            return new Module(name);
        }

        private SemanticOperator ParseStartOperator() => new Start();

        private SemanticOperator ParseVisibilityOperator()
        {
            SemanticOperator @operator = null;
            if (Match(TokenType.Function)) @operator = ParseFunctionOperator();
            if (Match(TokenType.Procedure)) @operator = ParseProcedureOperator();

            return @operator;
        }

        private SemanticOperator ParseReturnOperator()
        {
            var expression = ParseExpression();
            Consume(TokenType.Semicolon);

            return new Return(expression);
        }

        private List<Parameter> GetFunctionParameters()
        {
            List<Parameter> parameters = null;

            Consume(TokenType.LParen);
            while (!Match(TokenType.RParen))
            {
                parameters ??= new List<Parameter>();

                parameters.Add(new Parameter
                {
                    ParameterType = Consume(TokenType.Word).Text == "in" ? ParameterType.In : ParameterType.Var,
                    SemanticType = GetSemanticType(Consume(TokenType.Word).Text),
                    Name = Consume(TokenType.Word).Text
                });
                Match(TokenType.Comma);
            }
            
            return parameters;
        }

        private SemanticType GetSemanticType(string text)
        {
            return text switch
            {
                "integer" => SemanticType.Integer,
                "real" => SemanticType.Real,
                "boolean" => SemanticType.Boolean,
                "char" => SemanticType.Char,
                "string" => SemanticType.String,
                "array" => SemanticType.Array,
                _ => throw new Exception($"{text} - неизвестный тип данных!")
            };
        }
        
        private SemanticOperator ParseFunctionOperator()
        {
            var function = new Function();
            var visibilityToken = Get(-2);
            var visibilityType = visibilityToken.Text == "public" ? VisibilityType.Public : VisibilityType.Private;
            var name = Get().Text;
            _pos++;
            
            _parameters = GetFunctionParameters();
            
            Consume(TokenType.Colon);
            var returnType = GetSemanticType(Consume(TokenType.Word).Text);

            function.Parent = _operatorsStack.Peek();
            function.Parameters = _parameters;
            
            ParseBlock(function, () => !Match(TokenType.End));
            
            Consume(TokenType.Word);   // Skip function name
            Consume(TokenType.Semicolon);   // Skip ;
            
            function.VisibilityType = visibilityType;
            function.Name = name;
            function.Parameters = _parameters;
            function.ReturnSemanticType = returnType;
            ((Module) _semanticTree.Root).FunctionStorage.Add(name, new DefineFunction(function));
            return function;
        }

        private SemanticOperator ParseProcedureOperator()
        {
            var procedure = new Procedure();
            var visibilityToken = Get(-2);
            var visibilityType = visibilityToken.Text == "public" ? VisibilityType.Public : VisibilityType.Private;
            var name = Get().Text;
            _pos++;
            
            _parameters = GetFunctionParameters();
            
            procedure.Parent = _operatorsStack.Peek();
            procedure.Parameters = _parameters;

            ParseBlock(procedure, () => !Match(TokenType.End));
            
            Consume(TokenType.Word);   // Skip procedure name
            Consume(TokenType.Semicolon);   // Skip ;

            procedure.VisibilityType = visibilityType;
            procedure.Name = name;
            procedure.Parameters = _parameters;
            ((Module) _semanticTree.Root).FunctionStorage.Add(name, new DefineFunction(procedure));
            return procedure;
        }

        private void ParseBlock(MultilineOperator multiline, ParseBlockPredicate predicate)
        {
            multiline.Operators.Parent = multiline;
            _operatorsStack.Push(multiline);
            while (predicate())
            {
                multiline.Operators.Add(ParseOperator());
            }
            _operatorsStack.Pop();
        }
        
        private SemanticOperator ParseWhileOperator()
        {
            var whileOperator = new While();
            var expression = ParseExpression();
            Consume(TokenType.Word); // Skip repeat
            
            ParseBlock(whileOperator, () => !Match(TokenType.End));

            Consume(TokenType.While);   // Skip while word
            Consume(TokenType.Semicolon);   // Skip ;

            whileOperator.Expression = expression;
            return whileOperator;
        }
        
        private SemanticOperator ParseIfOperator()
        {
            var ifOperator = new If();
            var expression = ParseExpression();
            Consume(TokenType.Word);    // Skip then
            
            List<ElseIf> elseIfs = null;
            Else elseOperator = null;
            
            var currentToken = Get();
            while (!Match(TokenType.End))
            {
                if (currentToken.Type != TokenType.Else)
                {
                    ParseBlock(ifOperator, () => !Next(TokenType.Else) && !Next(TokenType.End));
                }
                else
                {
                    Consume(TokenType.Else);
                    if (Match(TokenType.If))
                    {
                        elseIfs ??= new List<ElseIf>();
                        var elseIfOperator = ParseElseIfOperator();
                        elseIfs.Add(elseIfOperator);
                    }
                    else
                    {
                        elseOperator = ParseElseOperator();
                    }
                }
                
                currentToken = Get();
            }
            
            Consume(TokenType.If);      // Skip if word
            Consume(TokenType.Semicolon);   // Skip ;
            
            ifOperator.Expression = expression;
            ifOperator.ElseIfs = elseIfs;
            ifOperator.Else = elseOperator;
            return ifOperator;
        }

        private ElseIf ParseElseIfOperator()
        {
            var elseIfOperator = new ElseIf();
            
            var elseIfExpr = ParseExpression();
            Consume(TokenType.Word);    // Skip then
                        
            ParseBlock(elseIfOperator, () => !Next(TokenType.Else) && !Next(TokenType.End));
                        
            elseIfOperator.Expression = elseIfExpr;
            elseIfOperator.Parent = _operatorsStack.Peek();

            return elseIfOperator;
        }
        
        private Else ParseElseOperator()
        {
            var elseOperator = new Else();
                        
            ParseBlock(elseOperator, () => !Next(TokenType.End));
                        
            elseOperator.Parent = _operatorsStack.Peek();
            return elseOperator;
        }
        
        private SemanticOperator ParseCallOperator()
        {
            var functionName = Consume(TokenType.Word).Text;
            ParseFunctionArguments(functionName);
            Consume(TokenType.Semicolon);
            
            return new Call(functionName);
        }

        private void ParseFunctionArguments(string functionName)
        {
            var module = (Module) _semanticTree.Root;
            var function = module.FunctionStorage.At(functionName).BaseFunction;
            
            var arguments = new List<string>();
            Consume(TokenType.LParen);
            while (!Match(TokenType.RParen))
            {
                var name = Consume(TokenType.Word).Text;
                arguments.Add(name);
                Match(TokenType.Comma);
            }

            if (function.Parameters != null)
            {
                if (function.Parameters.Count != arguments.Count)
                {
                    // Проверяем не указанные параметры
                    var message = $"Для {functionName} не были указаны аргументы - ";
                    for (var i = arguments.Count; i < function.Parameters.Count; i++)
                    {
                        message += $"{function.Parameters[i].Name}";
                        if (i != function.Parameters.Count - 1)
                            message += ", ";
                    }
                    
                    throw new Exception($"{message}.");
                }

                for (var i = 0; i < arguments.Count; i++)
                {
                    var id = GetVariableScopeId(arguments[i]);
                    function.Parameters[i].VariableId = id;
                    function.Parameters[i].Parent = function;
                }
            }
        }
        
        private SemanticOperator ParseVariableOperator()
        {
            Consume(TokenType.Minus);  // Skip -
            var type = GetSemanticType(Consume(TokenType.Word).Text);

            var variable = type switch
            {
                SemanticType.Array => ParseArray(),
                _ => ParseSimpleVariable(type)
            };

            _variables.Add(variable);
            
            return variable;
        }

        private Variable ParseArray()
        {
            var type = SemanticType.Array;
            var list = new List<ArrayValue>();

            while (type == SemanticType.Array)
            {
                Consume(TokenType.LBracket);
                var expression = ParseExpression();
                
                var size = expression.Eval() is IntegerValue value && value.AsInteger() >= 1 
                    ? value.AsInteger() 
                    : throw new Exception("Только натуральное число может быть размером массива.");
                Consume(TokenType.RBracket);

                var array = new ArrayValue(size);
                list.Add(array);
                
                type = GetSemanticType(Consume(TokenType.Word).Text);
            }

            for (var i = 0; i < list.Count-1; i++)
            {
                for (var j = 0; j < list[i].Size; j++)
                {
                    var copyArr = new ArrayValue(list[i + 1].AsArray());
                    list[i].Set(j, copyArr);
                }
            }

            var name = Consume(TokenType.Word).Text;
            Consume(TokenType.Semicolon);
                
            var parentId = _operatorsStack.Peek().OperatorId;
            var variableId = $"{parentId}^{name}";

            var arrayExpression = new ArrayExpression(name, type, list.First());
            return new Variable(type, name, variableId, arrayExpression);
        }
        
        private Variable ParseSimpleVariable(SemanticType type)
        {
            var name = Get().Text;
            IExpression expression = null;
            if (Match(TokenType.Word) && Get().Type == TokenType.Assign)
            {
                Consume(TokenType.Assign);
                expression = ParseExpression();
            }
            Consume(TokenType.Semicolon);
                
            var parentId = _operatorsStack.Peek().OperatorId;
            var variableId = $"{parentId}^{name}";
            
            return new Variable(type, name, variableId, expression);
        }
        
        private SemanticOperator ParseLetOperator()
        {
            var name = Consume(TokenType.Word).Text;
            var scopeId = GetVariableScopeId(name);
            List<IExpression> indexes = null;
            while (Next(TokenType.LBracket))
            {
                indexes ??= new List<IExpression>();
                Consume(TokenType.LBracket);
                var index = ParseExpression();
                indexes.Add(index);
                Consume(TokenType.RBracket);
            }
            Consume(TokenType.Assign);
            var expression = ParseExpression();
            Consume(TokenType.Semicolon);
            
            return new Let(scopeId, expression, indexes);
        }

        private SemanticOperator ParseInputOperator()
        {
            var name = Consume(TokenType.Word).Text;
            var scopeId = GetVariableScopeId(name);
            Consume(TokenType.Semicolon);

            return new Input(scopeId);
        }

        private SemanticOperator ParseOutputOperator()
        {
            var expression = ParseExpression();
            Consume(TokenType.Semicolon);

            return new Output(expression);
        }
        
        private string GetVariableScopeId(string name)
        {
            if (IsFunctionParsed())
            {
                var parameter = _parameters.FirstOrDefault(x => x.Name == name);
                if (parameter != null)
                {
                    return name;
                }
            }

            var stack = new Stack<MultilineOperator>(_operatorsStack.Reverse());
            while (stack.Count > 0)
            {
                var t = stack.Pop();
                var parentId = t.OperatorId;
                var variableId = $"{parentId}^{name}";

                var variable = _variables.FirstOrDefault(x => x.Id == variableId);
                if (variable != null)
                {
                    return variableId;
                }
            }
            
            throw new Exception($"Переменной/параметра {name} не существует!");
        }
        
        private SemanticOperator GetVariableOrParameterByName(string name)
        {
            if (IsFunctionParsed())
            {
                var parameter = _parameters.FirstOrDefault(x => x.Name == name);
                if (parameter != null)
                {
                    return parameter;
                }
            }

            var stack = new Stack<MultilineOperator>(_operatorsStack.Reverse());
            while (stack.Count > 0)
            {
                var t = stack.Pop();
                var parentId = t.OperatorId;
                var variableId = $"{parentId}^{name}";

                var variable = _variables.FirstOrDefault(x => x.Id == variableId);
                if (variable != null)
                {
                    return variable;
                }
            }
            
            throw new Exception($"Переменной/параметра {name} не существует!");
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
                case TokenType.Boolean: return new ValueExpression(current.Text == "true");
                case TokenType.Char: return new ValueExpression(Convert.ToChar(current.Text));
                case TokenType.Text: return new ValueExpression(current.Text);
                case TokenType.Word:
                    if (Next(TokenType.LParen))
                    {
                        var functionName = current.Text;
                        ParseFunctionArguments(functionName);
                        
                        var functionDefine = ((Module) _semanticTree.Root).FunctionStorage.At(functionName);

                        return new CalculatedExpression(functionDefine);
                    }
                    else if (Next(TokenType.LBracket))
                    {
                        var arrayName = current.Text;
                        var array = GetVariableOrParameterByName(arrayName);

                        var arrayExpression = array switch
                        {
                            Variable variable => (ArrayExpression) variable.Expression,
                            Parameter parameter => (ArrayExpression) parameter.Expression,
                            _ => throw new Exception("Что-то пошло не так при парсинге индексации")
                        };
                        
                        List<IExpression> indexes = null;
                        while (Next(TokenType.LBracket))
                        {
                            indexes ??= new List<IExpression>();
                            Consume(TokenType.LBracket);
                            var index = ParseExpression();
                            indexes.Add(index);
                            Consume(TokenType.RBracket);
                        }
                        
                        return new ArrayAccessExpression(indexes, arrayExpression);
                    }

                    var name = current.Text;
                    var variableOrParameter = (ICalculated) GetVariableOrParameterByName(name);

                    return new CalculatedExpression(variableOrParameter);
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
        
        private bool Next(TokenType type)
        {
            return Get().Type == type;
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Core.Operators;

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
        private List<Class> _classes = new();
        
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
            if (Match(TokenType.Class)) @operator = ParseClassOperator();
            // if (Match(TokenType.Field)) @operator = ParseFieldOperator();

            return @operator;
        }

        private SemanticOperator ParseClassOperator()
        {
            var classOperator = new Class();
            List<Field> fields = null;
            List<DefineFunction> methods = null;
            
            var visibilityToken = Get(-2);
            var visibilityType = visibilityToken.Text == "public" ? VisibilityType.Public : VisibilityType.Private;
            var name = Get().Text;
            _pos++;

            Consume(TokenType.Inherits);
            var baseClassName = Consume(TokenType.Word).Text;
            var baseClass = _classes.FirstOrDefault(x => x.Name == baseClassName);
            if (baseClass == null && baseClassName != "Object")
            {
                throw new Exception($"Класс {baseClassName} не объявлен!");
            }

            if (baseClass?.Fields != null)
            {
                fields = new List<Field>(baseClass.Fields);
            }

            if (baseClass?.Methods != null)
            {
                methods = new List<DefineFunction>(baseClass.Methods);
            }

            _operatorsStack.Push(classOperator);
            while (Get(1).Type == TokenType.Field)
            {
                fields ??= new List<Field>();
                var field = ParseFieldOperator();
                fields.Add(field);
            }
            
            while (Get(1).Type == TokenType.Method)
            {
                methods ??= new List<DefineFunction>();
                var method = ParseMethodOperator();
                methods.Add(new DefineFunction(method));
            }
            _operatorsStack.Pop();

            Consume(TokenType.End);
            Consume(TokenType.Word);
            Consume(TokenType.Semicolon);

            classOperator.Parent = _operatorsStack.Peek();
            classOperator.VisibilityType = visibilityType;
            classOperator.Name = name;
            classOperator.Fields = fields;
            classOperator.Methods = methods;
            
            _classes.Add(classOperator);
            return classOperator;
        }

        private Field ParseFieldOperator()
        {
            var visibilityToken = Consume(TokenType.VisibilityType);
            var visibilityType = visibilityToken.Text == "public" ? VisibilityType.Public : VisibilityType.Private;
            
            Consume(TokenType.Field);
            Consume(TokenType.Minus);
            
            var type = GetSemanticType(Consume(TokenType.Word).Text);
            var name = Get().Text;
            
            IExpression expression = null;
            if (Match(TokenType.Word) && Get().Type == TokenType.Assign)
            {
                Consume(TokenType.Assign);
                expression = ParseExpression();
            }
            Consume(TokenType.Semicolon);
            
            var field = new Field(visibilityType, type, name, expression) {Parent = _operatorsStack.Peek()};
            return field;
        }

        private BaseFunction ParseMethodOperator()
        {
            var visibilityToken = Consume(TokenType.VisibilityType);
            var visibilityType = visibilityToken.Text == "public" ? VisibilityType.Public : VisibilityType.Private;

            Consume(TokenType.Method);
            
            var name = Get().Text;
            _pos++;
            
            Consume(TokenType.LParen);
            var classParameterType = Consume(TokenType.Word).Text;
            var classParameterName = Consume(TokenType.Word).Text;
            Consume(TokenType.RParen);
            
            _parameters = GetFunctionParameters();

            BaseFunction method;
            if (Next(TokenType.Colon))
            {
                Consume(TokenType.Colon);
                method = new MethodFunction {SemanticType = GetSemanticType(Consume(TokenType.Word).Text)};

            }
            else method = new MethodProcedure();

            method.Parent = _operatorsStack.Peek();
            method.Parameters = _parameters;
            
            ParseBlock(method, () => !Match(TokenType.End));
            
            Consume(TokenType.Word);   // Skip function name
            Consume(TokenType.Semicolon);   // Skip ;

            method.VisibilityType = visibilityType;
            method.Name = name;
            method.Parameters = _parameters;
            ((IMethod) method).ClassParameter = new ClassParameter(classParameterName);
            return method;
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
                _ => SemanticType.Object
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
            function.SemanticType = returnType;
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
            var name = Consume(TokenType.Word).Text;
            // Если вызывается просто процедура
            if (!Next(TokenType.Dot))
            {
                var function = ((Module) _semanticTree.Root).FunctionStorage.At(name);
                ParseFunctionArguments(function.BaseFunction);
                Consume(TokenType.Semicolon);

                return new Call(function);
            }

            Consume(TokenType.Dot);
            var methodName = Consume(TokenType.Word).Text;
            
            var classVariable = _variables.FirstOrDefault(x => x.Name == name);
            if (classVariable == null)
            {
                throw new Exception($"Переменная {name} не объявлена!");
            }
            var value = (ClassValue) classVariable.Expression.Eval();
            
            var method = value.Class.Methods.FirstOrDefault(x => x.BaseFunction.Name == methodName);
            if (method == null)
            {
                throw new Exception($"В классе {name} не объявлен метод {methodName}");
            }
            
            ParseFunctionArguments(method.BaseFunction);
            Consume(TokenType.Semicolon);
            
            return new Call(method);
        }

        private void ParseFunctionArguments(BaseFunction function)
        {
            var arguments = new List<IExpression>();
            Consume(TokenType.LParen);
            while (!Match(TokenType.RParen))
            {
                var expression = ParseExpression();
                arguments.Add(expression);
                Match(TokenType.Comma);
            }

            if (function.Parameters != null)
            {
                if (function.Parameters.Count != arguments.Count)
                {
                    // Проверяем не указанные параметры
                    var message = $"Для {function.Name} не были указаны аргументы - ";
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
                    if (arguments[i] is CalculatedExpression calculatedExpression)
                    {
                        var id = calculatedExpression.Calculated switch
                        {
                            Variable variable => variable.Id,
                            // Parameter parameter => parameter.Name,
                            _ => throw new Exception("Что-то пошло не так при парсинге аргументов!")
                        };
                        // var id = GetVariableScopeId(name);
                        function.Parameters[i].VariableId = id;
                    }
                    else
                    {
                        function.Parameters[i].Expression = arguments[i];
                    }
                    function.Parameters[i].Parent = function;
                }
            }
        }


        #region ParseVariableFunctions
        private SemanticOperator ParseVariableOperator()
        {
            Consume(TokenType.Minus);  // Skip -
            var type = GetSemanticType(Consume(TokenType.Word).Text);

            var variable = type switch
            {
                SemanticType.Array => ParseArray(),
                SemanticType.Object => ParseObject(Get(-1).Text),
                _ => ParseSimpleVariable(type)
            };

            variable.Parent = _operatorsStack.Peek();
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

        private Variable ParseObject(string className)
        {
            var type = SemanticType.Object;
            var classType = _classes.FirstOrDefault(x => x.Name == className);
            if (className == null)
            {
                throw new Exception($"Класс {className} не объявлен!");
            }
            var name = Consume(TokenType.Word).Text;
            var expression = new ValueExpression(classType);

            Consume(TokenType.Semicolon);
            
            var parentId = _operatorsStack.Peek().OperatorId;
            var variableId = $"{parentId}^{name}";

            return new Variable(type, name, variableId, expression);
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
        #endregion ParseVariableFunctions
        
        private SemanticOperator ParseLetOperator()
        {
            var name = Consume(TokenType.Word).Text;
            var scopeId = GetVariableScopeId(name);

            if (Next(TokenType.LBracket))
            {
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
                var expression1 = ParseExpression();
                Consume(TokenType.Semicolon);
            
                return new Let(scopeId, indexes, expression1) {LetType = LetType.AssignToArrayIndex};
            }

            if (Next(TokenType.Dot))
            {
                Consume(TokenType.Dot);
                var fieldName = Consume(TokenType.Word).Text;
                Consume(TokenType.Assign);
                var expression2 = ParseExpression();
                Consume(TokenType.Semicolon);

                return new Let(scopeId, fieldName, expression2) {LetType = LetType.AssignToClassField};
            }
            
            Consume(TokenType.Assign);
            var expression3 = ParseExpression();
            Consume(TokenType.Semicolon);

            return new Let(scopeId, expression3) {LetType = LetType.AssignToVariable};
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


        #region RecursiveDescentMethod
        private IExpression ParseExpression()
        {
            return ParseInterpolationExpression();
        }

        private IExpression ParseInterpolationExpression()
        {
            var result = LogicalOr();

            // Если это выражение, которое может содержать строку.
            // PS. Только ValueExpression может содержать в себе строку,
            // в которой возможно наличие символа $, добавляемого для идентификации интерполяции.
            if (result is ValueExpression valueExpression && valueExpression.Eval().AsString().Contains("$"))
            {
                List<IExpression> expressions = new();
                var value = valueExpression.Eval().AsString();
                while (value.Contains("$"))
                {
                    var prefix = value.Remove(value.Length - 1);
                    var expression = LogicalOr();
                    
                    expressions.Add(new ValueExpression(prefix));
                    expressions.Add(expression);

                    value = LogicalOr().Eval().AsString();
                }
                
                expressions.Add(new ValueExpression(value));
                result = new InterpolationExpression(expressions);
            }

            return result;
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
                        var functionDefine = ((Module) _semanticTree.Root).FunctionStorage.At(functionName);
                        ParseFunctionArguments(functionDefine.BaseFunction);
                        
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
                    else if (Match(TokenType.Dot))
                    {
                        var operatorName = current.Text;
                        // Если выбираем у модуля
                        if (((Module) _semanticTree.Root).Name == operatorName)
                        {
                            
                        }
                        
                        var classVariable = _variables.FirstOrDefault(x => x.Name == operatorName);
                        if (classVariable == null)
                        {
                            throw new Exception($"Переменная {operatorName} не объявлена!");
                        }
                        var value = (ClassValue) classVariable.Expression.Eval();
                        
                        var classItemName = Consume(TokenType.Word).Text;
                        // Если выбираем функцию
                        if (Next(TokenType.LParen))
                        {
                            var method = value.Class.Methods.FirstOrDefault(x => x.BaseFunction.Name == classItemName);
                            if (method == null)
                            {
                                throw new Exception($"В классе {operatorName} не объявлен метод {classItemName}");
                            }

                            ParseFunctionArguments(method.BaseFunction);
                            return new CalculatedExpression(method);
                        }
                        
                        var field = value.Class.Fields.FirstOrDefault(x => x.Name == classItemName);
                        if (field == null)
                        {
                            throw new Exception($"В классе {operatorName} не объявлено поле {classItemName}");
                        }

                        return new CalculatedExpression(field);
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
        
        #endregion RecursiveDescentMethod
        
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
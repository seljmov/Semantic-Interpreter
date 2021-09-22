using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Core.Items.Types;
using Semantic_Interpreter.Library;
using BinaryExpression = Semantic_Interpreter.Core.BinaryExpression;
using ConditionalExpression = Semantic_Interpreter.Core.ConditionalExpression;
using UnaryExpression = Semantic_Interpreter.Core.UnaryExpression;
using ValueType = Semantic_Interpreter.Core.ValueType;

namespace Semantic_Interpreter.Parser
{
    public class Parser
    {
        private static readonly Token Eof = new(TokenType.Eof, "");

        private readonly List<Token> _tokens;
        private readonly int _length;

        private int _pos;
        private readonly SemanticTree _semanticTree = new();
        private Root _treeRoot;
        private readonly Stack<MultilineOperator> _operatorsStack = new();
        
        private List<Variable> _variables = new();
        private List<Parameter> _parameters = new();
        private List<Class> _classes = new();
        private List<string> _standartModules = new() {"MathBase", "FilesBase", "SystemBase"};

        private Func<bool> IsFunctionParsed => () => _operatorsStack.Any(x => x is BaseFunction);
        private Func<BaseFunction> GetParentBaseFunction => () => (BaseFunction) _operatorsStack.Single(x => x is BaseFunction);

        private delegate bool ParseBlockPredicate();

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _length = tokens.Count;
            _pos = 0;
        }

        public SemanticTree Parse()
        {
            _treeRoot = new Root();
            _semanticTree.InsertOperator(null, _treeRoot);
            _operatorsStack.Push(_treeRoot);
            SemanticOperator lastOperator = _treeRoot;
            
            while (!Match(TokenType.Eof))
            {
                if (Match(TokenType.End)) return _semanticTree;

                var prevOperator = lastOperator;
                var newOperator = ParseOperator();
                bool asChild;
                // Операторы Module и Beginning имеют индивидуальные правила, по которым они вставляются
                // в дерево, а остальные операторы вставляются по общим правилам (кейс default)
                switch (newOperator)
                {
                    case Module module:
                        module.GetRoot().Module = module;
                        asChild = _operatorsStack.Peek().Child == null;
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
                TokenType.Import => ParseImportOperator(),
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

        private SemanticOperator ParseImportOperator()
        {
            var name = Consume(TokenType.Word).Text;
            Consume(TokenType.Semicolon);
            var import = new Import(name);

            if (_standartModules.Contains(name))
            {
                var module = import.GetImportedModule();
                var root = (Root) _semanticTree.Root;
                root.Imports.Add(module);
            }
            else
            {
                using var reader = new StreamReader($"{name}.slang");
                var program = reader.ReadToEnd();
            
                var lexer = new Lexer(program);
                var tokens = lexer.Tokenize();
                var tree = new Parser(tokens).Parse();
                tree.TraversalTree();

                var module = ((Root) tree.Root).Module;
                _treeRoot.Imports.Add(module);
            }
            
            
            return import;
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

            return @operator;
        }

        private SemanticOperator ParseClassOperator()
        {
            var classOperator = new Class();
            
            var visibilityToken = Get(-2);
            var visibilityType = visibilityToken.Text == "public" ? VisibilityType.Public : VisibilityType.Private;
            var name = Get().Text;
            _pos++;

            Consume(TokenType.Inherits);
            var baseClassName = Consume(TokenType.Word).Text;
            if (baseClassName != "Object" && baseClassName != "Объект")
            {
                var any = _classes.Any(x => x.Name == baseClassName);
                if (!any)
                {
                    throw new Exception($"Класс {baseClassName} не объявлен!");
                }
                
                var baseClass = _classes.Single(x => x.Name == baseClassName);
                classOperator.Fields = new List<Field>(baseClass.Fields);
                classOperator.Methods = new List<DefineFunction>(baseClass.Methods);
                
                classOperator.Fields.ForEach(field => field.Parent = classOperator);
                classOperator.Methods.ForEach(function => function.BaseFunction.Parent = classOperator);
            }

            classOperator.Parent = _operatorsStack.Peek();
            classOperator.VisibilityType = visibilityType;
            classOperator.Name = name;
            classOperator.BaseClass = baseClassName;
            
            _operatorsStack.Push(classOperator);
            while (Get(1).Type == TokenType.Field)
            {
                classOperator.Fields ??= new List<Field>();
                var field = ParseFieldOperator();
                classOperator.Fields.Add(field);
            }
            
            while (Get(1).Type == TokenType.Method)
            {
                classOperator.Methods ??= new List<DefineFunction>();
                var method = ParseMethodOperator();
                classOperator.Methods.Add(new DefineFunction(method));
            }
            _operatorsStack.Pop();

            Consume(TokenType.End);
            Consume(TokenType.Word);
            Consume(TokenType.Semicolon);

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
            var parent = (Class) _operatorsStack.Peek();
            if (classParameterType != parent.Name)
            {
                throw new Exception($"В методе {name} указан неправильный тип параметра класса ({classParameterType})");
            }
            
            var classParameterName = Consume(TokenType.Word).Text;
            Consume(TokenType.RParen);
            
            _parameters = GetFunctionParameters();

            BaseFunction method;
            if (Next(TokenType.Colon))
            {
                Consume(TokenType.Colon);
                method = new MethodFunction {Type = GetSemanticType(Consume(TokenType.Word).Text, false)};

            }
            else method = new MethodProcedure();

            method.Parent = _operatorsStack.Peek();
            method.Parameters = _parameters;
            method.VisibilityType = visibilityType;
            method.Name = name;
            ((IHaveClassParameter) method).ClassParameter = classParameterName;
            
            ParseBlock(method, () => !Match(TokenType.End));
            
            Consume(TokenType.Word);   // Skip function name
            Consume(TokenType.Semicolon);   // Skip ;
            
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
                    Type = GetSemanticType(Consume(TokenType.Word).Text, false),
                    Name = Consume(TokenType.Word).Text
                });
                Match(TokenType.Comma);
            }
            
            return parameters;
        }

        private ValueType GetValueType(string text)
        {
            return text switch
            {
                "integer" => ValueType.Integer,
                "real" => ValueType.Real,
                "boolean" => ValueType.Boolean,
                "char" => ValueType.Char,
                "string" => ValueType.String,
                "array" => ValueType.Array,
                _ => ValueType.Object
            };
        }

        private ISemanticType GetSemanticType(string text, bool withSize = true)
        {
            if (text != "array")
            {
                var semanticType = text switch
                {
                    "integer" => ValueType.Integer,
                    "real" => ValueType.Real,
                    "boolean" => ValueType.Boolean,
                    "char" => ValueType.Char,
                    "string" => ValueType.String,
                    _ => ValueType.Object
                };

                return new SimpleType(semanticType);
            }

            var stack = new Stack<ArrayType>();
            while (text == "array")
            {
                Consume(TokenType.LBracket);
                var arrayType = new ArrayType();
                if (withSize)
                {
                    // TODO: Сделать проверку размера при выполнении
                    var size = ParseExpression();

                    /*
                    var size = expression.Eval() is IntegerValue value && value.AsInteger() >= 1
                        ? value.AsInteger()
                        : throw new Exception("Только натуральное число может быть размером массива.");
                    */

                    arrayType.Size = size;
                }
                Consume(TokenType.RBracket);
                stack.Push(arrayType);

                text = Consume(TokenType.Word).Text;
            }
            
            var last = text switch
            {
                "integer" => ValueType.Integer,
                "real" => ValueType.Real,
                "boolean" => ValueType.Boolean,
                "char" => ValueType.Char,
                "string" => ValueType.String,
                _ => ValueType.Object
            };

            ISemanticType type = new SimpleType(last);

            while (stack.Count > 0)
            {
                var array = stack.Pop();
                array.Type = type;
                type = array;
            }

            return type;
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
            var returnType = GetSemanticType(Consume(TokenType.Word).Text, false);

            function.Parent = _operatorsStack.Peek();
            function.Parameters = _parameters;
            
            ParseBlock(function, () => !Match(TokenType.End));
            
            Consume(TokenType.Word);   // Skip function name
            Consume(TokenType.Semicolon);   // Skip ;
            
            function.VisibilityType = visibilityType;
            function.Name = name;
            function.Parameters = _parameters;
            function.Type = returnType;
            ((Root) _semanticTree.Root).Module.FunctionStorage.Add(name, new DefineFunction(function));
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
            ((Root) _semanticTree.Root).Module.FunctionStorage.Add(name, new DefineFunction(procedure));
            return procedure;
        }

        private void ParseBlock(IHaveBlock haveBlock, ParseBlockPredicate predicate)
        {
            haveBlock.Block = new List<SemanticOperator>();
            _operatorsStack.Push((MultilineOperator) haveBlock);
            while (predicate())
            {
                haveBlock.Block.Add(ParseOperator());
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
                var function = (DefineFunction) ((Root) _semanticTree.Root).Module.FunctionStorage.At(name);
                ParseFunctionArguments(function.BaseFunction);
                Consume(TokenType.Semicolon);

                return new Call(new CalculatedExpression(function));
            }

            Consume(TokenType.Dot);
            var methodName = Consume(TokenType.Word).Text;

            if (_treeRoot.Imports.Any(x => x.Name == name))
            {
                var module = _treeRoot.Imports.Single(x => x.Name == name);
                
                List<IExpression> arguments = null;
                if (Next(TokenType.LParen))
                {
                    arguments = new List<IExpression>();
                    Consume(TokenType.LParen);
                    while (!Match(TokenType.RParen))
                    {
                        var expression = ParseExpression();
                        arguments.Add(expression);
                        Match(TokenType.Comma);
                    }
                }
                            
                var function = module.FunctionStorage.At(methodName);
                Consume(TokenType.Semicolon);

                return new Call(new NativeFunctionExpression(arguments, function));
            }
            
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
            
            return new Call(new CalculatedExpression(method));
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
                    switch (arguments[i])
                    {
                        case CalculatedExpression {Calculated: Variable variable}:
                            function.Parameters[i].Operator = variable;
                            break;
                        case CalculatedExpression {Calculated: Parameter parameter}:
                            function.Parameters[i].Operator = parameter;
                            break;
                        default:
                            function.Parameters[i].Expression = arguments[i];
                            break;
                    }

                    function.Parameters[i].Parent = function;
                }
            }
        }


        #region ParseVariableFunctions
        private SemanticOperator ParseVariableOperator()
        {
            Consume(TokenType.Minus);  // Skip -
            var type = GetValueType(Consume(TokenType.Word).Text);

            var variable = type switch
            {
                ValueType.Array => ParseArray(),
                ValueType.Object => ParseObject(Get(-1).Text),
                _ => ParseSimpleVariable(type)
            };

            variable.Parent = _operatorsStack.Peek();
            _variables.Add(variable);
            
            return variable;
        }

        private Variable ParseArray()
        {
            _pos--; // Возвращаемся на токен array
            var semanticType = GetSemanticType(Consume(TokenType.Word).Text);

            var value = new ArrayValue {Type = semanticType};
            
            var name = Consume(TokenType.Word).Text;

            var expression = Match(TokenType.Assign) 
                ? ParseExpression() 
                : new ArrayExpression(value);

            Consume(TokenType.Semicolon);
                
            var parentId = _operatorsStack.Peek().OperatorId;
            var variableId = $"{parentId}^{name}";
            
            return new Variable(semanticType, name, variableId, expression);
        }

        private Variable ParseObject(string className)
        {
            const ValueType type = ValueType.Object;

            Class classType;
            if (!Match(TokenType.Dot))
            {
                classType = _classes.FirstOrDefault(x => x.Name == className);
                if (classType == null)
                {
                    throw new Exception($"Класс {className} не объявлен!");
                }
            }
            else
            {
                var moduleName = className;
                className = Consume(TokenType.Word).Text;

                var module = _treeRoot.Imports.FirstOrDefault(x => x.Name == moduleName);
                if (module == null)
                {
                    throw new Exception($"Модуль {moduleName} не объявлен!");
                }

                classType = module.ClassStorage.At(className);
            }
            
            var name = Consume(TokenType.Word).Text;

            IExpression expression;
            if (!Match(TokenType.Assign))
            {
                expression = new ValueExpression(classType);
            }
            else
            {
                var expr = ParseExpression();
                var value = expr.Eval();
                expression = new ValueExpression(value);
            }

            Consume(TokenType.Semicolon);
            
            var parentId = _operatorsStack.Peek().OperatorId;
            var variableId = $"{parentId}^{name}";

            return new Variable(new SimpleType(type), name, variableId, expression);
        }
        
        private Variable ParseSimpleVariable(ValueType type)
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
            
            return new Variable(new SimpleType(type), name, variableId, expression);
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
                var parameter = _parameters?.FirstOrDefault(x => x.Name == name);
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
                var parameter = _parameters?.FirstOrDefault(x => x.Name == name);
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
                        var functionDefine = (DefineFunction) ((Root) _semanticTree.Root).Module.FunctionStorage.At(functionName);
                        ParseFunctionArguments(functionDefine.BaseFunction);
                        
                        return new CalculatedExpression(functionDefine);
                    }
                    else if (Next(TokenType.LBracket))
                    {
                        var arrayName = current.Text;
                        var array = (IHaveExpression) GetVariableOrParameterByName(arrayName);

                        var expression = array switch
                        {
                            Variable variable => variable.Expression,
                            Parameter parameter => parameter.Expression,
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
                        
                        return new ArrayAccessExpression(indexes, array);
                    }
                    else if (Match(TokenType.Dot))
                    {
                        var operatorName = current.Text;
                        // Если выбираем у модуля
                        var root = (Root) _semanticTree.Root;
                        if (root.Module.Name == operatorName)
                        {
                            throw new Exception("Подобное обращение к модулю невозможно!");
                        }
                        
                        if (root.Imports.Any(x => x.Name == operatorName))
                        {
                            var module = root.Imports.Single(x => x.Name == operatorName);

                            var childName = Consume(TokenType.Word).Text;
                            var function = module.FunctionStorage.At(childName);

                            if (function is DefineFunction defineFunction)
                            {
                                ParseFunctionArguments(defineFunction.BaseFunction);
                        
                                return new CalculatedExpression(defineFunction);
                            }
                            
                            List<IExpression> arguments = null;
                            if (Next(TokenType.LParen))
                            {
                                arguments = new List<IExpression>();
                                Consume(TokenType.LParen);
                                while (!Match(TokenType.RParen))
                                {
                                    var expression = ParseExpression();
                                    arguments.Add(expression);
                                    Match(TokenType.Comma);
                                }
                            }

                            return new NativeFunctionExpression(arguments, function);
                        }
                        
                        var classVariable = _variables.FirstOrDefault(x => x.Name == operatorName);
                        if (classVariable == null)
                        {
                            var baseFunction = GetParentBaseFunction();
                            if (((IHaveClassParameter) baseFunction).ClassParameter == operatorName)
                            {
                                var parentClass = (Class) baseFunction.Parent;
                                classVariable = new Variable(new ValueExpression(parentClass));
                            }
                            else
                            {
                                throw new Exception($"Переменная {operatorName} не объявлена!");
                            }
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
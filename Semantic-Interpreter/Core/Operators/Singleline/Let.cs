using System;
using System.Collections.Generic;
using System.Linq;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public enum LetType
    {
        AssignToVariable,
        AssignToArrayIndex,
        AssignToClassField,
    }
    public class Let : SemanticOperator
    {
        public Let(string variableName, IExpression expression)
        {
            VariableName = variableName;
            Expression = expression;
        }
        
        public Let(string variableName, List<IExpression> indexes, IExpression expression)
        {
            VariableName = variableName;
            Indexes = indexes;
            Expression = expression;
        }

        public Let(string className, string fieldName, IExpression expression)
        {
            ClassName = className;
            FieldName = fieldName;
            Expression = expression;
        }
        
        public string VariableName { get; set; }
        public string ClassName { get; set; }
        public string FieldName { get; set; }
        
        public IExpression Expression { get; }
        public List<IExpression> Indexes { get; }
        
        public LetType LetType { get; set; }

        public override void Execute()
        {
            switch (LetType)
            {
                case LetType.AssignToVariable:
                    AssignToVariable();
                    break;
                case LetType.AssignToArrayIndex:
                    AssignToArrayIndex();
                    break;
                case LetType.AssignToClassField:
                    AssignToClassField();
                    break;
            }
        }

        private void AssignToVariable()
        {
            var variableName = VariableName.Split("^").Last();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);
            
            var curr = Parent;
            while (curr.Parent != null)
            {
                if (curr is BaseFunction {Parameters: { }} function && function.ParameterIsExist(VariableName))
                {
                    function.Parameters.Single(t => t.Name == VariableName).Expression = expression;
                    return;
                }
                
                curr = curr.Parent;
            }

            var module = GetRoot().Module;
            if (module.VariableStorage.IsExist(VariableName))
            {
                module.VariableStorage.Replace(VariableName, expression);
                return;
            }

            throw new Exception($"Переменная {variableName} не объявлена!");
        }

        private void AssignToArrayIndex()
        {
            var module = GetRoot().Module;
            var value = Expression.Eval();

            if (Indexes != null)
            {
                if (module.VariableStorage.IsExist(VariableName))
                {
                    var arrayExpression = (ArrayExpression) module.VariableStorage.At(VariableName).Expression;

                    arrayExpression.Set(Indexes, value);

                    module.VariableStorage.Replace(VariableName, arrayExpression);
                }
                else
                {
                    var curr = Parent;
                    while (curr.Parent != null)
                    {
                        if (curr is BaseFunction function && function.ParameterIsExist(VariableName))
                        {
                            var arrayExpression = (ArrayExpression) function.Parameters.Single(t => t.Name == VariableName).Expression;
                            arrayExpression.Set(Indexes, value);
                            function.Parameters.Single(t => t.Name == VariableName).Expression = arrayExpression;
                            return;
                        }
                
                        curr = curr.Parent;
                    }
                }
            }
        }

        private void AssignToClassField()
        {
            var className = ClassName.Split("^").Last();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);
            
            // TODO: Проверить класс-параметр 
            if (GetRoot().Module.VariableStorage.IsExist(ClassName))
            {
                var classVariable = GetRoot().Module.VariableStorage.At(ClassName);
                var classValue = (ClassValue) classVariable.Calculate();

                var field = classValue.Class.Fields.FirstOrDefault(x => x.Name == FieldName);
                if (field == null)
                {
                    throw new Exception($"В классе {className} не объявлено поле {FieldName}");
                }

                field.Expression = expression;
                return;
            }

            throw new Exception($"Переменная {className} не объявлена!");
        }
    }
}
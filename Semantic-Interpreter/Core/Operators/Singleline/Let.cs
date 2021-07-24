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
                if (curr is BaseFunction function && function.ParameterIsExist(VariableName))
                {
                    function.Parameters.Single(t => t.Name == VariableName).Expression = expression;
                    return;
                }
                
                curr = curr.Parent;
            }
            
            if (VariableStorage.IsExist(VariableName))
            {
                VariableStorage.Replace(VariableName, expression);
                return;
            }

            throw new Exception($"Переменная {variableName} не объявлена!");
        }

        private void AssignToArrayIndex()
        {
            var value = Expression.Eval();

            if (Indexes != null)
            {
                if (VariableStorage.IsExist(VariableName))
                {
                    var arrayExpression = (ArrayExpression) VariableStorage.At(VariableName).Expression;

                    arrayExpression.Set(Indexes, value);

                    VariableStorage.Replace(VariableName, arrayExpression);
                }
            }
        }

        private void AssignToClassField()
        {
            var className = ClassName.Split("^").Last();
            var value = Expression.Eval();
            var expression = new ValueExpression(value);
            
            if (VariableStorage.IsExist(ClassName))
            {
                var classVariable = VariableStorage.At(ClassName);
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
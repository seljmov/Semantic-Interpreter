using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Core.Operators;

namespace Semantic_Interpreter.Core
{
    public class Field : SemanticOperator, ICalculated, IHaveVisibility, IHaveType, IHaveName
    {
        public Field(VisibilityType visibilityType, SemanticType semanticType, string name, IExpression expression)
        {
            VisibilityType = visibilityType;
            SemanticType = semanticType;
            Name = name;
            Expression = expression;
        }
        
        public VisibilityType VisibilityType { get; set; }
        public SemanticType SemanticType { get; }
        public string Name { get; set; }
        public IExpression Expression { get; set; }
        
        public IValue Calculate() => Expression.Eval();
        
        public override void Execute()
        {
            if (Expression != null)
            {
                var value = Expression.Eval();
                Expression = new ValueExpression(value);
            }
        }
        
    }
}
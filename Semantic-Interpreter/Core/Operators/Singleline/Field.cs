using Semantic_Interpreter.Core.Items;
using Semantic_Interpreter.Core.Items.Types;

namespace Semantic_Interpreter.Core
{
    public class Field : SemanticOperator, ICalculated, IHaveType
    {
        public Field(VisibilityType visibilityType, ISemanticType type, string name, IExpression expression)
        {
            VisibilityType = visibilityType;
            Type = type;
            Name = name;
            Expression = expression;
        }
        
        public VisibilityType VisibilityType { get; set; }
        public ISemanticType Type { get; set; }
        public string Name { get; set; }
        public IExpression Expression { get; set; }
        
        public Value Calculate() => Expression.Eval();
        
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
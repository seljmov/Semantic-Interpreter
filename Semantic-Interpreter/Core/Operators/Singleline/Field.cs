using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Core
{
    public class Field : SemanticOperator, ICalculated
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
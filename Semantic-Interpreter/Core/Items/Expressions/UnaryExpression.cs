using System;

namespace Semantic_Interpreter.Core
{
    public class UnaryExpression : IExpression
    {
        public UnaryExpression(Operations operation, IExpression expression)
        {
            Operation = operation;
            Expression = expression;
        }
        
        private IExpression Expression { get; }
        private Operations Operation { get; }
        
        public IValue Eval()
        {
            var value1 = Expression.Eval();
            
            return value1 switch
            {
                 IntegerValue => Operation switch
                 {
                     Operations.Minus => new IntegerValue(-Expression.Eval().AsInteger()),
                     Operations.Plus => new IntegerValue(Expression.Eval().AsInteger()),
                 },
                 RealValue => Operation switch
                 {
                     Operations.Minus => new RealValue(-Expression.Eval().AsReal()),
                     Operations.Plus => new RealValue(Expression.Eval().AsReal()),
                 },
                 _ => throw new Exception("Неопределенный тип!")
            };
        }
    }
}
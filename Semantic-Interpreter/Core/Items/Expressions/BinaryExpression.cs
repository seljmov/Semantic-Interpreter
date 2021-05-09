using System;
using Semantic_Interpreter.Library;

namespace Semantic_Interpreter.Core
{
    public class BinaryExpression : IExpression
    {
        public BinaryExpression(Operations operation, IExpression expression1, IExpression expression2)
        {
            Operation = operation;
            Expression1 = expression1;
            Expression2 = expression2;
        }
        
        private IExpression Expression1 { get; }
        private IExpression Expression2 { get; }
        private Operations Operation { get; }
        
        public IValue Eval()
        {
            var value1 = Expression1.Eval();
            var value2 = Expression2.Eval();

            return value1 switch
            {
                IntegerValue => Operation switch
                {
                    Operations.Plus => new IntegerValue(value1.AsInteger() + value2.AsInteger()),
                    Operations.Minus => new IntegerValue(value1.AsInteger() - value2.AsInteger()),
                    Operations.Multiply => new IntegerValue(value1.AsInteger() * value2.AsInteger()),
                    Operations.Divide => new IntegerValue(value1.AsInteger() / value2.AsInteger()),
                    _ => throw new ArgumentOutOfRangeException()
                },
                RealValue => Operation switch
                {
                    Operations.Plus => new RealValue(value1.AsReal() + value2.AsReal()),
                    Operations.Minus => new RealValue(value1.AsReal() - value2.AsReal()),
                    Operations.Multiply => new RealValue(value1.AsReal() * value2.AsReal()),
                    Operations.Divide => new RealValue(value1.AsReal() / value2.AsReal()),
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new Exception("Неопределенный тип!")
            };
        }

        public override string ToString()
            => $"{Expression1} {Operation} {Expression2}";
    }
}
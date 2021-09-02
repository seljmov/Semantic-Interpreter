using System;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter.Core
{
    public class ConditionalExpression : IExpression
    {
        public ConditionalExpression(TokenType @operator, IExpression expression1, IExpression expression2)
        {
            Expression1 = expression1;
            Expression2 = expression2;
            Operator = @operator;
        }
        
        private IExpression Expression1 { get; set; }
        private IExpression Expression2 { get; set; }
        private TokenType Operator { get; set; }
        
        public Value Eval()
        {
            var value1 = Expression1.Eval();
            var value2 = Expression2.Eval();
            
            switch (value1)
            {
                case IntegerValue:
                {
                    var number1 = value1.AsInteger();
                    var number2 = value2.AsInteger();

                    var result = Conditional(number1, number2);
                    return new BooleanValue(result);
                }
                case RealValue:
                {
                    var number1 = value1.AsReal();
                    var number2 = value2.AsReal();

                    var result = Conditional(number1, number2);
                    return new BooleanValue(result);

                }
                default: return new BooleanValue(false);
            }
        }

        public override string ToString()
            => $"{Expression1} {Operator} {Expression2}";
        
        private bool Conditional(dynamic number1, dynamic number2)
        {
            return Operator switch
            {
                TokenType.Less => number1 < number2,
                TokenType.LessOrEqual => number1 <= number2,
                TokenType.Greater => number1 > number2,
                TokenType.GreaterOrEqual => number1 >= number2,
                TokenType.Equal => number1 == number2,
                TokenType.NotEqual => number1 != number2,
                TokenType.OrOr => number1 || number2,
                TokenType.AndAnd => number1 && number2
            };
        }
    }
}
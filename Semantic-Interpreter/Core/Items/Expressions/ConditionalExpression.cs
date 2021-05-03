using System;
using Semantic_Interpreter.Library;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter.Core
{
    public class ConditionalExpression : Expression
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
        
        public override IValue Eval()
        {
            var value1 = Expression1.Eval();
            var value2 = Expression2.Eval();

            int result;
            switch (value1)
            {
                case IntegerValue:
                {
                    var number1 = value1.AsInteger();
                    var number2 = value2.AsInteger();

                    // TODO: Добавить тип Boolean и переписать
                    result = Conditional(number1, number2);
                    break;
                }
                case RealValue:
                {
                    var number1 = value1.AsReal();
                    var number2 = value2.AsReal();

                    result = Conditional(number1, number2);
                    break;
                }
                default: result = 0; break;
            }

            return new IntegerValue(result);
        }

        private int Conditional(dynamic number1, dynamic number2)
        {
            bool result = Operator switch
            {
                TokenType.Less => number1 < number2,
                TokenType.LessOrEqual => number1 <= number2,
                TokenType.Greater => number1 > number2,
                TokenType.GreaterOrEqual => number1 >= number2,
                TokenType.Equal => number1 == number2,
                TokenType.NotEqual => number1 != number2,
                TokenType.OrOr => number1 || number2,
                TokenType.AndAnd => number1 && number2,
            };
            
            return Convert.ToInt32(result);
        }
    }
}
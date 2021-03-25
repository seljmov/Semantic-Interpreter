using System;

namespace Semantic_Interpreter.Core
{
    public class Output : SemanticOperator, IHaveExpression
    {
        public Output(IExpression expression)
        {
            Expression = expression;
        }

        public Output(string text) => Text = text;
        
        public string Text { get; set; }
        public IExpression Expression { get; set; }

        public override void Execute()
        {
            Console.WriteLine(Expression.Eval());
        }
    }
}
using System;

namespace Semantic_Interpreter.Core
{
    public class Output : SemanticOperator, IHaveExpression
    {
        // TODO: delete
        /*
        public Output(string text) => Text = text;
        public string Text { get; set; }
        */
        
        public Output(IExpression expression) 
            => Expression = expression;

        public IExpression Expression { get; set; }

        public override void Execute()
            => Console.WriteLine(Expression.Eval());
    }
}
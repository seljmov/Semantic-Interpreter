namespace Semantic_Interpreter.Core
{
    public class ArrayAccessExpression : IExpression
    {
        public ArrayAccessExpression(IExpression indexExpression, ArrayExpression arrayExpression)
        {
            IndexExpression = indexExpression;
            ArrayExpression = arrayExpression;
        }
        
        private IExpression IndexExpression { get; }
        private ArrayExpression ArrayExpression { get; }
        
        public IValue Eval()
        {
            return ArrayExpression.Get(IndexExpression.Eval().AsInteger());
        }
    }
}
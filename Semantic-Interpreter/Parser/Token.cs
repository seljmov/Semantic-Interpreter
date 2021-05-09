namespace Semantic_Interpreter.Parser
{
    public class Token
    {
        public Token(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }
        
        public TokenType Type { get; }
        public string Text { get; }
    }
}
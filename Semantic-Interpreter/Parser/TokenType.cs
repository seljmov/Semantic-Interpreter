namespace Semantic_Interpreter.Parser
{
    public enum TokenType
    {
        Eof,
        Word,
        Text,
        
        Module,
        Beginning,
        Output,
        Variable,
        Assing,
        
        Plus,
        Minus,
        Multiply,
        Divide,
        
        End,
        
        Semicolon,
        Dash,
        Dot,
        NotFound,
    }
}
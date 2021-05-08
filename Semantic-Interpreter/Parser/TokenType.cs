namespace Semantic_Interpreter.Parser
{
    public enum TokenType
    {
        Eof,
        Word,
        Text,
        Number,
        
        Module,
        Beginning,
        While,
        If,
        Else,
        Output,
        Input,
        Variable,
        Assing,
        Let,
        
        Plus,
        Minus,
        Multiply,
        Divide,
        
        LParen,
        RParen,
        Equal,
        NotEqual,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        AndAnd,
        OrOr,
        End,
        
        Semicolon,
        Dot,
        NotFound,
    }
}
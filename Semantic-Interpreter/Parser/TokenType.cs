namespace Semantic_Interpreter.Parser
{
    public enum TokenType
    {
        Eof,
        Word,
        Char,
        Boolean,
        Text,
        Number,
        
        Module,
        Start,
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
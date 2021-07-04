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
        Function,
        Procedure,
        Call,
        While,
        If,
        Else,
        Output,
        Input,
        Variable,
        Assign,
        Let,
        Return,
        
        Plus,
        Minus,
        Multiply,
        Divide,
        
        LParen,
        RParen,
        LBracket,
        RBracket,
        Equal,
        NotEqual,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        AndAnd,
        OrOr,
        End,
        
        VisibilityType,
        ParameterType,
        Semicolon,
        Colon,
        Comma,
        Dot,
        NotFound,
    }
}
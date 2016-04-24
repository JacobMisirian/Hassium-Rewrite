using System;

namespace Hassium.Lexer
{
    public class Token
    {
        public TokenType TokenType { get; private set; }
        public string Value { get; private set; }
        public SourceLocation SourceLocation { get; private set; }

        public Token(TokenType tokenType, string value, SourceLocation location)
        {
            TokenType = tokenType;
            Value = value;
            SourceLocation = location;
        }
    }

    public enum TokenType
    {
        Assignment,
        Identifier,
        Int64,
        Double,
        String,
        Char,
        Comma,
        Dot,
        LeftBrace,
        RightBrace,
        LeftParentheses,
        RightParentheses,
        LeftSquare,
        RightSquare,
        Semicolon,
        Colon,
        BinaryOperation,
        UnaryOperation,
        Comparison
    }
}


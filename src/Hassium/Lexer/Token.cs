using System;

namespace Hassium.Lexer
{
    public class Token
    {
        public TokenType TokenType { get; private set; }
        public string Value { get; private set; }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }
    }

    public enum TokenType
    {
        Assignment,
        Identifier,
        Number,
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


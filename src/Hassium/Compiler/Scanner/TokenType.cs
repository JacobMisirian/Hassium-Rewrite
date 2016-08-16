using System;

namespace Hassium.Compiler.Scanner
{
    public enum TokenType
    {
        Identifier,
        String,
        Integer,
        Float,
        Char,
        OpenParentheses,
        CloseParentheses,
        OpenBracket,
        CloseBracket,
        OpenSquare,
        CloseSquare,
        Comma,
        Dot,
        Semicolon,
        Colon,
        Operation,
        Comparison,
        Assignment
    }
}


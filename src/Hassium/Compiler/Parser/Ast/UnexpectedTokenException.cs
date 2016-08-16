using System;

using Hassium.Compiler.Scanner;

namespace Hassium.Compiler.Parser
{
    public class UnexpectedTokenException : Exception
    {
        public new string Message { get { return string.Format("Unexpected token of type {0} value {1}!", Token.TokenType, Token.Value); } }
        public Token Token { get; private set; }

        public UnexpectedTokenException(Token token)
        {
            Token = token;
        }
    }
}


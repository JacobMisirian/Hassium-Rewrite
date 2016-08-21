using System;

using Hassium.Compiler.Scanner;

namespace Hassium.Compiler.Parser
{
    public class ExpectedTokenException : Exception
    {
        public new string Message { get { return string.Format("Expected type {0} value {1}. Got {2} value {3}!", ExpectedType, ExpectedValue, EncounteredToken.TokenType, EncounteredToken.Value); } }
        public SourceLocation SourceLocation { get; private set; }
        public TokenType ExpectedType { get; private set; }
        public string ExpectedValue { get; private set; }
        public Token EncounteredToken { get; private set; }

        public ExpectedTokenException(SourceLocation location, TokenType expectedType, string expectedValue, Token got)
        {
            SourceLocation = location;
            ExpectedType = expectedType;
            ExpectedValue = expectedValue;
            EncounteredToken = got;
        }
    }
}


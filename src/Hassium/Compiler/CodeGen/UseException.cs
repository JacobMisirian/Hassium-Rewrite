using System;

namespace Hassium.Compiler.CodeGen
{
    public class UseException : Exception
    {
        public new string Message { get; private set; }
        public SourceLocation SourceLocation { get; private set; }

        public UseException(SourceLocation location, string message)
        {
            Message = message;
            SourceLocation = location;
        }
    }
}


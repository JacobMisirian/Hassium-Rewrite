using System;

namespace Hassium.Runtime
{
    public class InternalException : Exception
    {
        public static string ATTRIBUTE_NOT_FOUND =  "Could not find attribute {0} in {1}!";
        public static string CONVERSION_ERROR =     "Could not convert {0} to {1}!";
        public static string OPERATOR_ERROR =       "Could not apply operator {0} to {1}!";

        public new string Message { get; private set; }

        public InternalException(string messageFormat, params object[] args)
        {
            Message = string.Format(messageFormat, args);
        }
    }
}


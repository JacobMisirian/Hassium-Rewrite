using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumChar: HassiumObject
    {
        public new char Value { get; private set; }
        public HassiumChar(char value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}


using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumBool: HassiumObject
    {
        public new bool Value { get; private set; }
        public HassiumBool(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}


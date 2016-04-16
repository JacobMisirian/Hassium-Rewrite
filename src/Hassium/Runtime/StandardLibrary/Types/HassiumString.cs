using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumString: HassiumObject
    {
        public new string Value { get; private set; }
        public HassiumString(string value)
        {
            Value = value;
            Attributes.Add("toUpper", new HassiumFunction(toUpper, 0));
        }

        public static HassiumString operator + (HassiumString left, HassiumString right)
        {
            return new HassiumString(left.Value + right.Value);
        }

        private HassiumObject toUpper(HassiumObject[] args)
        {
            return new HassiumString(Value.ToUpper());
        }

        public override string ToString()
        {
            return Value;
        }
    }
}


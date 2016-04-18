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

        public HassiumObject __add__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return Value + (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value + (HassiumDouble)obj;
            throw new Exception("Cannot operate double on " + obj);
        }

        public static HassiumChar operator + (char left, HassiumChar right)
        {
            return new HassiumChar(Convert.ToChar((int)left + (int)right.Value));
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}


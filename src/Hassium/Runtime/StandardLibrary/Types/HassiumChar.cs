using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumChar: HassiumObject
    {
        public new char Value { get; private set; }
        public HassiumChar(char value)
        {
            Value = value;
            Attributes.Add(HassiumObject.ADD_FUNCTION, new HassiumFunction(__add__, 1));
            Attributes.Add(HassiumObject.SUB_FUNCTION, new HassiumFunction(__sub__, 1));
            Attributes.Add(HassiumObject.EQUALS_FUNCTION, new HassiumFunction(__equals__, 1));
            Attributes.Add(HassiumObject.NOT_EQUAL_FUNCTION, new HassiumFunction(__notequals__, 1));
        }

        private HassiumObject __add__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return Value + (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value + (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __sub__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return Value - (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value - (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __equals__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return Value == (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value == (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __notequals__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return Value != (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value != (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }

        public static HassiumChar operator + (char left, HassiumChar right)
        {
            return new HassiumChar(Convert.ToChar((int)left + (int)right.Value));
        }
        public static HassiumChar operator - (char left, HassiumChar right)
        {
            return new HassiumChar(Convert.ToChar((int)left - (int)right.Value));
        }
        public static HassiumBool operator == (char left, HassiumChar right)
        {
            return new HassiumBool(left == right.Value);
        }
        public static HassiumBool operator != (char left, HassiumChar right)
        {
            return new HassiumBool(left != right.Value);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}


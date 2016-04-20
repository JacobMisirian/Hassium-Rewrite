using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumBool: HassiumObject
    {
        public new bool Value { get; private set; }
        public HassiumBool(bool value)
        {
            Value = value;
            Attributes.Add("toBool", new HassiumFunction(toBool, 0));
            Attributes.Add("toChar", new HassiumFunction(toChar, 0));
            Attributes.Add("toDouble", new HassiumFunction(toDouble, 0));
            Attributes.Add(HassiumObject.EQUALS_FUNCTION, new HassiumFunction(__equals__, 1));
            Attributes.Add(HassiumObject.NOT_EQUAL_FUNCTION, new HassiumFunction(__notequals__, 1));
            Attributes.Add(HassiumObject.TOSTRING_FUNCTION, new HassiumFunction(__tostring__, 0));
            Types.Add(this.GetType().Name);
        }

        private HassiumBool toBool(HassiumObject[] args)
        {
            return this;
        }
        private HassiumChar toChar(HassiumObject[] args)
        {
            return new HassiumChar((char)(Value ? 1 : 0));
        }
        private HassiumDouble toDouble(HassiumObject[] args)
        {
            return new HassiumDouble(Value ? 1 : 0);
        }

        private HassiumObject __equals__ (HassiumObject[] args)
        {
            return Value == (HassiumBool)args[0];
        }
        private HassiumObject __notequals__ (HassiumObject[] args)
        {
            return Value != (HassiumBool)args[0];
        }
        private HassiumString __tostring__ (HassiumObject[] args)
        {
            return new HassiumString(Value.ToString());
        }

        public static HassiumBool operator == (bool left, HassiumBool right)
        {
            return new HassiumBool(left == right.Value);
        }
        public static HassiumBool operator != (bool left, HassiumBool right)
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


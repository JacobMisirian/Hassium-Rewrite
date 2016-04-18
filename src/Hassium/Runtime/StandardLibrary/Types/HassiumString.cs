using System;
using System.Text;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumString: HassiumObject
    {
        public new string Value { get; private set; }
        public HassiumString(string value)
        {
            Value = value;
            Attributes.Add("toUpper", new HassiumFunction(toUpper, 0));
            Attributes.Add("toLower", new HassiumFunction(toLower, 0));
            Attributes.Add("toDouble", new HassiumFunction(toDouble, 0));
            Attributes.Add("toChar", new HassiumFunction(toChar, 0));
            Attributes.Add("toList", new HassiumFunction(toList, 0));
            Attributes.Add(HassiumObject.ADD_FUNCTION, new HassiumFunction(__add__, 1));
            Attributes.Add(HassiumObject.EQUALS_FUNCTION, new HassiumFunction(__equals__, 1));
            Attributes.Add(HassiumObject.NOT_EQUAL_FUNCTION, new HassiumFunction(__notequal__, 1));
            Attributes.Add(HassiumObject.INDEX_FUNCTION, new HassiumFunction(__index__, 1));
        }

        private HassiumObject toUpper(HassiumObject[] args)
        {
            return new HassiumString(Value.ToUpper());
        }
        private HassiumObject toLower(HassiumObject[] args)
        {
            return new HassiumString(Value.ToLower());
        }
        private HassiumObject toDouble(HassiumObject[] args)
        {
            return new HassiumDouble(Convert.ToDouble(Value));
        }
        private HassiumObject toChar(HassiumObject[] args)
        {
            return new HassiumChar(Convert.ToChar(Value));
        }
        private HassiumObject toList(HassiumObject[] args)
        {
            HassiumObject[] items = new HassiumObject[Value.Length];
            for (int i = 0; i < items.Length; i++)
                items[i] = new HassiumChar(Value[i]);
            return new HassiumList(items);
        }

        private HassiumObject __add__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumString)
                return this + (HassiumString)obj;
            throw new Exception("Cannot operate string on " + obj);
        }
        private HassiumObject __equals__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumString)
                return this == (HassiumString)obj;
            throw new Exception("Cannot compare string to " + obj);
        }
        private HassiumObject __notequal__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumString)
                return this != (HassiumString)obj;
            throw new Exception("Cannot compare string to " + obj);
        }
        private HassiumObject __index__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return new HassiumChar(Value[((HassiumDouble)obj).ValueInt]);
            throw new Exception("Cannot index string with " + obj);
        }

        public static HassiumString operator + (HassiumString left, HassiumString right)
        {
            return new HassiumString(left.Value + right.Value);
        }
        public static HassiumString operator * (HassiumString left, HassiumDouble right)
        {
            StringBuilder sb = new StringBuilder();
            for (double i = 0; i < right.Value; i++)
                sb.Append(left.Value);
            return new HassiumString(sb.ToString());
        }
        public static HassiumString operator * (HassiumDouble left, HassiumString right)
        {
            StringBuilder sb = new StringBuilder();
            for (double i = 0; i < left.Value; i++)
                sb.Append(right.Value);
            return new HassiumString(sb.ToString());
        }
        public static HassiumBool operator == (HassiumString left, HassiumString right)
        {
            return new HassiumBool(left.Value == right.Value);
        }
        public static HassiumBool operator != (HassiumString left, HassiumString right)
        {
            return new HassiumBool(left.Value != right.Value);
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
            return Value;
        }
    }
}


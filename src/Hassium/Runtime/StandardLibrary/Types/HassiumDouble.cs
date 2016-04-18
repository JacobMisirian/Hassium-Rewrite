using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumDouble: HassiumObject
    {
        public new double Value { get; private set; }
        public int ValueInt { get { return Convert.ToInt32(Value); } }
        public HassiumDouble(double value)
        {
            Value = value;
            Attributes.Add(HassiumObject.ADD_FUNCTION, new HassiumFunction(__add__, 1));
            Attributes.Add(HassiumObject.SUB_FUNCTION, new HassiumFunction(__sub__, 1));
            Attributes.Add(HassiumObject.MUL_FUNCTION, new HassiumFunction(__mul__, 1));
            Attributes.Add(HassiumObject.DIV_FUNCTION, new HassiumFunction(__div__, 1));
            Attributes.Add(HassiumObject.MOD_FUNCTION, new HassiumFunction(__mod__, 1));
            Attributes.Add(HassiumObject.EQUALS_FUNCTION, new HassiumFunction(__equals__, 1));
            Attributes.Add(HassiumObject.NOT_EQUAL_FUNCTION, new HassiumFunction(__notequal__, 1));
            Attributes.Add(HassiumObject.GREATER_FUNCTION, new HassiumFunction(__greater__, 1));
            Attributes.Add(HassiumObject.GREATER_OR_EQUAL_FUNCTION, new HassiumFunction(__greaterorequal__, 1));
            Attributes.Add(HassiumObject.LESSER_FUNCTION, new HassiumFunction(__lesser__, 1));
            Attributes.Add(HassiumObject.LESSER_OR_EQUAL_FUNCTION, new HassiumFunction(__lesserorequal__, 1));
        }

        private HassiumObject __add__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this + (HassiumDouble)obj;
            throw new Exception("Cannot operate double on " + obj);
        }
        private HassiumObject __sub__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this - (HassiumDouble)obj;
            throw new Exception("Cannot operate double on " + obj);
        }
        private HassiumObject __mul__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this * (HassiumDouble)obj;
            throw new Exception("Cannot operate double on " + obj);
        }
        private HassiumObject __div__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this / (HassiumDouble)obj;
            throw new Exception("Cannot operate double on " + obj);
        }
        private HassiumObject __mod__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this % (HassiumDouble)obj;
            throw new Exception("Cannot operate double on " + obj);
        }
        private HassiumObject __equals__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this == (HassiumDouble)obj;
            throw new Exception("Cannot compare double to " + obj);
        }
        private HassiumObject __notequal__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this != (HassiumDouble)obj;
            throw new Exception("Cannot compare double to " + obj);
        }
        private HassiumObject __greater__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this > (HassiumDouble)obj;
            throw new Exception("Cannot compare double to " + obj);
        }
        private HassiumObject __greaterorequal__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this >= (HassiumDouble)obj;
            throw new Exception("Cannot compare double to " + obj);
        }
        private HassiumObject __lesser__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this < (HassiumDouble)obj;
            throw new Exception("Cannot compare double to " + obj);
        }
        private HassiumObject __lesserorequal__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return this <= (HassiumDouble)obj;
            throw new Exception("Cannot compare double to " + obj);
        }

        public static HassiumDouble operator + (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value + right.Value);
        }
        public static HassiumChar operator + (char left, HassiumDouble right)
        {
            return new HassiumChar(Convert.ToChar((int)left + right.ValueInt));
        }
        public static HassiumDouble operator - (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value - right.Value);
        }
        public static HassiumChar operator - (char left, HassiumDouble right)
        {
            return new HassiumChar(Convert.ToChar((int)left - right.ValueInt));
        }
        public static HassiumDouble operator * (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value * right.Value);
        }
        public static HassiumDouble operator / (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value / right.Value);
        }
        public static HassiumDouble operator % (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value % right.Value);
        }
        public static HassiumBool operator == (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value == right.Value);
        }
        public static HassiumBool operator == (char left, HassiumDouble right)
        {
            return new HassiumBool(left == right.Value);
        }
        public static HassiumBool operator != (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value != right.Value);
        }
        public static HassiumBool operator != (char left, HassiumDouble right)
        {
            return new HassiumBool(left != right.Value);
        }
        public static HassiumBool operator > (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value > right.Value);
        }
        public static HassiumBool operator >= (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value >= right.Value);
        }
        public static HassiumBool operator < (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value < right.Value);
        }
        public static HassiumBool operator <= (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value <= right.Value);
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


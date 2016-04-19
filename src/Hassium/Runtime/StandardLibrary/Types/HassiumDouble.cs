using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumDouble: HassiumObject
    {
        public new double Value { get; private set; }
        public int ValueInt { get { return Convert.ToInt32(Value); } }

        public static HassiumDouble Create(HassiumObject obj)
        {
            if (!(obj is HassiumDouble))
                throw new Exception(string.Format("Cannot convert from {0} to HassiumDouble!", obj.GetType()));
            return (HassiumDouble)obj;
        }

        public HassiumDouble(double value)
        {
            Value = value;
            Attributes.Add(HassiumObject.ADD_FUNCTION, new HassiumFunction(__add__, 1));
            Attributes.Add(HassiumObject.SUB_FUNCTION, new HassiumFunction(__sub__, 1));
            Attributes.Add(HassiumObject.MUL_FUNCTION, new HassiumFunction(__mul__, 1));
            Attributes.Add(HassiumObject.DIV_FUNCTION, new HassiumFunction(__div__, 1));
            Attributes.Add(HassiumObject.MOD_FUNCTION, new HassiumFunction(__mod__, 1));
            Attributes.Add(HassiumObject.XOR_FUNCTION, new HassiumFunction(__xor__, 1));
            Attributes.Add(HassiumObject.OR_FUNCTION, new HassiumFunction(__or__, 1));
            Attributes.Add(HassiumObject.XAND_FUNCTION, new HassiumFunction(__xand__, 1));
            Attributes.Add(HassiumObject.EQUALS_FUNCTION, new HassiumFunction(__equals__, 1));
            Attributes.Add(HassiumObject.NOT_EQUAL_FUNCTION, new HassiumFunction(__notequal__, 1));
            Attributes.Add(HassiumObject.GREATER_FUNCTION, new HassiumFunction(__greater__, 1));
            Attributes.Add(HassiumObject.GREATER_OR_EQUAL_FUNCTION, new HassiumFunction(__greaterorequal__, 1));
            Attributes.Add(HassiumObject.LESSER_FUNCTION, new HassiumFunction(__lesser__, 1));
            Attributes.Add(HassiumObject.LESSER_OR_EQUAL_FUNCTION, new HassiumFunction(__lesserorequal__, 1));
            Attributes.Add(HassiumObject.TOSTRING_FUNCTION, new HassiumFunction(__tostring__, 0));
            Types.Add(this.GetType().Name);
        }

        private HassiumObject __add__ (HassiumObject[] args)
        {
            return this + Create(args[0]);
        }
        private HassiumObject __sub__ (HassiumObject[] args)
        {
            return this - Create(args[0]);
        }
        private HassiumObject __mul__ (HassiumObject[] args)
        {
            return this * Create(args[0]);
        }
        private HassiumObject __div__ (HassiumObject[] args)
        {
            return this / Create(args[0]);
        }
        private HassiumObject __mod__ (HassiumObject[] args)
        {
            return this % Create(args[0]);
        }
        private HassiumObject __xor__ (HassiumObject[] args)
        {
            return this ^ Create(args[0]);
        }
        private HassiumObject __or__ (HassiumObject[] args)
        {
            return this | Create(args[0]);
        }
        private HassiumObject __xand__ (HassiumObject[] args)
        {
            return this & Create(args[0]);
        }
        private HassiumObject __equals__ (HassiumObject[] args)
        {
            return this == Create(args[0]);
        }
        private HassiumObject __notequal__ (HassiumObject[] args)
        {
            return this != Create(args[0]);
        }
        private HassiumObject __greater__ (HassiumObject[] args)
        {
            return this > Create(args[0]);
        }
        private HassiumObject __greaterorequal__ (HassiumObject[] args)
        {
            return this >= Create(args[0]);
        }
        private HassiumObject __lesser__ (HassiumObject[] args)
        {
            return this < Create(args[0]);
        }
        private HassiumObject __lesserorequal__ (HassiumObject[] args)
        {
            return this <= Create(args[0]);
        }
        private HassiumString __tostring__ (HassiumObject[] args)
        {
            return new HassiumString(Value.ToString());
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
        public static HassiumDouble operator ^ (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble((byte)(left.ValueInt ^ right.ValueInt));
        }
        public static HassiumDouble operator | (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble((byte)(left.ValueInt | right.ValueInt));
        }
        public static HassiumDouble operator & (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble((byte)(left.ValueInt & right.ValueInt));
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


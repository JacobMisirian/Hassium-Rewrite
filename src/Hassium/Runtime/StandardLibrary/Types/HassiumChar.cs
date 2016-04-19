using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumChar: HassiumObject
    {
        public new char Value { get; private set; }

        public static HassiumChar Create(HassiumObject obj)
        {
            if (!(obj is HassiumChar))
                throw new Exception(string.Format("Cannot convert from {0} to HassiumChar!", obj.GetType()));
            return (HassiumChar)obj;
        }

        public HassiumChar(char value)
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
            Attributes.Add(HassiumObject.NOT_EQUAL_FUNCTION, new HassiumFunction(__notequals__, 1));
            Types.Add(this.GetType().Name);
        }

        private HassiumObject __add__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this + (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this + (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __sub__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this - (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this - (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __mul__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this * (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this * (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __div__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this / (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this / (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __mod__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this % (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this % (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __xor__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this ^ (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this ^ (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __or__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this | (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this | (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __xand__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this & (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this & (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __equals__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this == (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value == (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __notequals__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this != (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return Value != (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __greater__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this > (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this > (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __lesser__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this < (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this < (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __greaterorequal__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this >= (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this >= (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }
        private HassiumObject __lesserorequal__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumChar)
                return this <= (HassiumChar)obj;
            else if (obj is HassiumDouble)
                return this <= (HassiumDouble)obj;
            throw new Exception("Cannot operate char on " + obj);
        }

        public static HassiumChar operator + (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value + right.Value));
        }
        public static HassiumChar operator + (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value + right.ValueInt));
        }
        public static HassiumChar operator - (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value - right.Value));
        }
        public static HassiumChar operator - (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value - right.ValueInt));
        }
        public static HassiumChar operator * (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value * right.Value));
        }
        public static HassiumChar operator * (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value * right.ValueInt));
        }
        public static HassiumChar operator / (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value / right.Value));
        }
        public static HassiumChar operator / (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value / right.ValueInt));
        }
        public static HassiumChar operator % (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value % right.Value));
        }
        public static HassiumChar operator % (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value % right.ValueInt));
        }
        public static HassiumChar operator ^ (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value ^ right.Value));
        }
        public static HassiumChar operator ^ (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value ^ right.ValueInt));
        }
        public static HassiumChar operator | (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value | right.Value));
        }
        public static HassiumChar operator | (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value | right.ValueInt));
        }
        public static HassiumChar operator & (HassiumChar left, HassiumChar right)
        {
            return new HassiumChar((char)(left.Value & right.Value));
        }
        public static HassiumChar operator & (HassiumChar left, HassiumDouble right)
        {
            return new HassiumChar((char)(left.Value & right.ValueInt));
        }
        public static HassiumBool operator == (HassiumChar left, HassiumChar right)
        {
            return new HassiumBool(left.Value == right.Value);
        }
        public static HassiumBool operator == (HassiumChar left, HassiumDouble right)
        {
            return new HassiumBool((int)left.Value == right.ValueInt);
        }
        public static HassiumBool operator != (HassiumChar left, HassiumChar right)
        {
            return new HassiumBool(left.Value != right.Value);
        }
        public static HassiumBool operator != (HassiumChar left, HassiumDouble right)
        {
            return new HassiumBool((int)left.Value != right.ValueInt);
        }
        public static HassiumBool operator > (HassiumChar left, HassiumChar right)
        {
            return new HassiumBool(left.Value > right.Value);
        }
        public static HassiumBool operator > (HassiumChar left, HassiumDouble right)
        {
            return new HassiumBool((int)left.Value > right.ValueInt);
        }
        public static HassiumBool operator < (HassiumChar left, HassiumChar right)
        {
            return new HassiumBool(left.Value < right.Value);
        }
        public static HassiumBool operator < (HassiumChar left, HassiumDouble right)
        {
            return new HassiumBool((int)left.Value < right.Value);
        }
        public static HassiumBool operator >= (HassiumChar left, HassiumChar right)
        {
            return new HassiumBool(left.Value >= right.Value);
        }
        public static HassiumBool operator >= (HassiumChar left, HassiumDouble right)
        {
            return new HassiumBool((int)left.Value >= right.ValueInt);
        }
        public static HassiumBool operator <= (HassiumChar left, HassiumChar right)
        {
            return new HassiumBool(left.Value <= right.Value);
        }
        public static HassiumBool operator <= (HassiumChar left, HassiumDouble right)
        {
            return new HassiumBool((int)left.Value <= right.ValueInt);
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


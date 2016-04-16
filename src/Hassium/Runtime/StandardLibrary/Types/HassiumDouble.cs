using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumDouble: HassiumObject
    {
        public new double Value { get; private set; }
        public HassiumDouble(double value)
        {
            Value = value;
        }

        private HassiumObject toString(HassiumObject[] args)
        {
            return new HassiumString(Value.ToString());
        }

        public static HassiumDouble operator + (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value + right.Value);
        }
        public static HassiumDouble operator - (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumDouble(left.Value - right.Value);
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
        public static HassiumBool operator != (HassiumDouble left, HassiumDouble right)
        {
            return new HassiumBool(left.Value != right.Value);
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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}


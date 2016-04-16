using System;
using System.Collections.Generic;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public abstract class HassiumObject
    {
        public const string EQUALS_FUNCTION = "__equals__";
        public const string NOT_EQUAL_FUNCTION = "__notequal__";
        public const string GREATER_FUNCTION = "__greater__";
        public const string LESSER_FUNCTION = "__lesser__";
        public const string GREATER_OR_EQUAL_FUNCTION = "__greaterorequal__";
        public const string LESSER_OR_EQUAL_FUNCTION = "__lesserorequal__";

        public Dictionary<string, HassiumObject> Attributes = new Dictionary<string, HassiumObject>();
        public object Value { get; private set; }

        public virtual HassiumBool Equals(HassiumObject obj)
        {
            return ((HassiumBool)((HassiumFunction)Attributes[EQUALS_FUNCTION]).Invoke(null, new HassiumObject[] { obj }));
        }
        public virtual HassiumBool NotEquals(HassiumObject obj)
        {
            return ((HassiumBool)((HassiumFunction)Attributes[NOT_EQUAL_FUNCTION]).Invoke(null, new HassiumObject[] { obj }));
        }
        public virtual HassiumBool GreaterThan(HassiumObject obj)
        {
            return ((HassiumBool)((HassiumFunction)Attributes[GREATER_FUNCTION]).Invoke(null, new HassiumObject[] { obj }));
        }
        public virtual HassiumBool GreaterThanOrEqual(HassiumObject obj)
        {
            return ((HassiumBool)((HassiumFunction)Attributes[GREATER_OR_EQUAL_FUNCTION]).Invoke(null, new HassiumObject[] { obj }));
        }
        public virtual HassiumBool LesserThan(HassiumObject obj)
        {
            return ((HassiumBool)((HassiumFunction)Attributes[LESSER_FUNCTION]).Invoke(null, new HassiumObject[] { obj }));
        }
        public virtual HassiumBool LesserThanOrEqual(HassiumObject obj)
        {
            return ((HassiumBool)((HassiumFunction)Attributes[LESSER_OR_EQUAL_FUNCTION]).Invoke(null, new HassiumObject[] { obj }));
        }
    }
}
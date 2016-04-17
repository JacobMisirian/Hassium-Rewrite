using System;
using System.Collections.Generic;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumObject : ICloneable
    {
        public const string ADD_FUNCTION = "__add__";
        public const string SUB_FUNCTION = "__sub__";
        public const string MUL_FUNCTION = "__mul__";
        public const string DIV_FUNCTION = "__div__";
        public const string MOD_FUNCTION = "__mod__";
        public const string EQUALS_FUNCTION = "__equals__";
        public const string NOT_EQUAL_FUNCTION = "__notequal__";
        public const string GREATER_FUNCTION = "__greater__";
        public const string LESSER_FUNCTION = "__lesser__";
        public const string GREATER_OR_EQUAL_FUNCTION = "__greaterorequal__";
        public const string LESSER_OR_EQUAL_FUNCTION = "__lesserorequal__";
        public const string INVOKE_FUNCTION = "__invoke__";

        public Dictionary<string, HassiumObject> Attributes = new Dictionary<string, HassiumObject>();
        public object Value { get; private set; }

        public virtual HassiumObject Add(HassiumObject obj)
        {
            return ((HassiumFunction)Attributes[ADD_FUNCTION]).Invoke(null, new HassiumObject[] { obj });
        }
        public virtual HassiumObject Sub(HassiumObject obj)
        {
            return ((HassiumFunction)Attributes[SUB_FUNCTION]).Invoke(null, new HassiumObject[] { obj });
        }
        public virtual HassiumObject Mul(HassiumObject obj)
        {
            return ((HassiumFunction)Attributes[MUL_FUNCTION]).Invoke(null, new HassiumObject[] { obj });
        }
        public virtual HassiumObject Div(HassiumObject obj)
        {
            return ((HassiumFunction)Attributes[DIV_FUNCTION]).Invoke(null, new HassiumObject[] { obj });
        }
        public virtual HassiumObject Mod(HassiumObject obj)
        {
            return ((HassiumFunction)Attributes[MOD_FUNCTION]).Invoke(null, new HassiumObject[] { obj });
        }
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
        public virtual HassiumObject Invoke(VirtualMachine vm, HassiumObject[] args)
        {
            if (Attributes.ContainsKey(INVOKE_FUNCTION))
                return Attributes[INVOKE_FUNCTION].Invoke(vm, args);
            throw new Exception("Object does not support invoking!");
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
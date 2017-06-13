using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime
{
    public class HassiumObject : ICloneable
    {
        public static HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("object");

        public static string INVOKE = "__invoke__";
        public static string ADD = "__add__";
        public static string SUBTRACT = "__subtract__";
        public static string MULTIPLY = "__multiply__";
        public static string DIVIDE = "__divide__";
        public static string MODULUS = "__modulus__";
        public static string POWER = "__power__";
        public static string INTEGERDIVISION = "__intdivision__";
        public static string BITSHIFTLEFT = "__bitshiftleft__";
        public static string BITSHIFTRIGHT = "__bitshiftright__";
        public static string EQUALTO = "__equals__";
        public static string NOTEQUALTO = "__notequal__";
        public static string GREATERTHAN = "__greaterthan__";
        public static string GREATERTHANOREQUAL = "__greaterthanorequal__";
        public static string LESSERTHAN = "__lesserthan__";
        public static string LESSERTHANOREQUAL = "__lesserthanorequal__";
        public static string BITWISEAND = "__bitwiseand__";
        public static string BITWISEOR = "__bitwiseor__";
        public static string BITWISEXOR = "__bitwisexor__";
        public static string BITWISENOT = "__bitwisenot__";
        public static string LOGICALAND = "__logicaland__";
        public static string LOGICALOR = "__logicalor__";
        public static string LOGICALNOT = "__logicalnot__";
        public static string NEGATE = "__negate__";
        public static string INDEX = "__index__";
        public static string STOREINDEX = "__storeindex__";
        public static string ITER = "__iter__";
        public static string ITERABLEFULL = "__iterablefull__";
        public static string ITERABLENEXT = "__iterablenext__";
        public static string DISPOSE = "dispose";
        public static string TOBOOL = "toBool";
        public static string TOCHAR = "toChar";
        public static string TOINT = "toInt";
        public static string TOFLOAT = "toFloat";
        public static string TOLIST = "toList";
        public static string TOSTRING = "toString";
        public static string TOTUPLE = "toTuple";

        public HassiumClass Parent { get; set; }

        public List<HassiumTypeDefinition> Types = new List<HassiumTypeDefinition>()
        {
            TypeDefinition
        };
        public Dictionary<string, HassiumObject> Attributes = new Dictionary<string, HassiumObject>();


        public HassiumTypeDefinition Type()
        {
            return Types[Types.Count - 1];
        }

        public void AddAttribute(string name, HassiumObject value)
        {
            Attributes.Add(name, value);
        }
        public void AddAttribute(string name, HassiumFunctionDelegate func, params int[] paramLengths)
        {
            AddAttribute(name, new HassiumFunction(func, paramLengths));
        }
        public void AddAttribute(string name, HassiumFunctionDelegate func, int paramLength = -1)
        {
            AddAttribute(name, func, new int[] { paramLength });
        }
        public void AddType(HassiumTypeDefinition typeDefinition)
        {
            Types.Add(typeDefinition);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

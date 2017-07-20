using System;
using System.Collections.Generic;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumObject : ICloneable
    {
        public static HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("object");

        public static HassiumNull Null = new HassiumNull();
        public static HassiumTypeDefinition Number = new HassiumTypeDefinition("number");

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
        public static string XOR = "__xor__";

        public HassiumClass Parent { get; set; }

        public List<HassiumTypeDefinition> Types = new List<HassiumTypeDefinition>();

        public Dictionary<string, HassiumObject> Attributes = new Dictionary<string, HassiumObject>();

        public HassiumTypeDefinition Type()
        {
            return Types[Types.Count - 1] as HassiumTypeDefinition;
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

        public virtual HassiumObject Add(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(ADD))
                return Attributes[ADD].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, ADD));
            return Null;
        }
        public virtual HassiumObject Subtract(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(SUBTRACT))
                return Attributes[SUBTRACT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, SUBTRACT));
            return Null;
        }
        public virtual HassiumObject Multiply(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(MULTIPLY))
                return Attributes[MULTIPLY].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, MULTIPLY));
            return Null;
        }
        public virtual HassiumObject Divide(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(DIVIDE))
                return Attributes[DIVIDE].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, DIVIDE));
            return Null;
        }
        public virtual HassiumObject Modulus(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(MODULUS))
                return Attributes[MODULUS].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, MODULUS));
            return Null;
        }
        public virtual HassiumObject Power(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(POWER))
                return Attributes[POWER].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, POWER));
            return Null;
        }
        public virtual HassiumObject IntegerDivision(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(INTEGERDIVISION))
                return Attributes[INTEGERDIVISION].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, INTEGERDIVISION));
            return Null;
        }
        public virtual HassiumObject BitshiftLeft(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(BITSHIFTLEFT))
                return Attributes[BITSHIFTLEFT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, BITSHIFTLEFT));
            return Null;
        }
        public virtual HassiumObject BitshiftRight(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(BITSHIFTRIGHT))
                return Attributes[BITSHIFTRIGHT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, BITSHIFTRIGHT));
            return Null;
        }
        public virtual HassiumBool EqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(EQUALTO))
                return Attributes[EQUALTO].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, EQUALTO));
            return new HassiumBool(false);
        }
        public virtual HassiumBool NotEqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(NOTEQUALTO))
                return Attributes[NOTEQUALTO].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, NOTEQUALTO));
            return new HassiumBool(false);
        }
        public virtual HassiumObject GreaterThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(GREATERTHAN))
                return Attributes[GREATERTHAN].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, GREATERTHAN));
            return Null;
        }
        public virtual HassiumObject GreaterThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(GREATERTHANOREQUAL))
                return Attributes[GREATERTHANOREQUAL].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, GREATERTHANOREQUAL));
            return Null;
        }
        public virtual HassiumObject LesserThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(LESSERTHAN))
                return Attributes[LESSERTHAN].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, LESSERTHAN));
            return Null;
        }
        public virtual HassiumObject LesserThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(LESSERTHANOREQUAL))
                return Attributes[LESSERTHANOREQUAL].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, LESSERTHANOREQUAL));
            return Null;
        }
        public virtual HassiumObject BitwiseAnd(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(BITWISEAND))
                return Attributes[BITWISEAND].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, BITWISEAND));
            return Null;
        }
        public virtual HassiumObject BitwiseOr(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(BITWISEOR))
                return Attributes[BITWISEOR].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, BITWISEOR));
            return Null;
        }
        public virtual HassiumObject BitwiseNot(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(BITWISENOT))
                return Attributes[BITWISENOT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, BITWISENOT));
            return Null;
        }
        public virtual HassiumObject LogicalAnd(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(LOGICALAND))
                return Attributes[LOGICALAND].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, LOGICALAND));
            return Null;
        }
        public virtual HassiumObject LogicalOr(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(LOGICALOR))
                return Attributes[LOGICALOR].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, LOGICALOR));
            return Null;
        }
        public virtual HassiumObject LogicalNot(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(LOGICALNOT))
                return Attributes[LOGICALNOT].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, LOGICALNOT));
            return Null;
        }
        public virtual HassiumObject Negate(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(NEGATE))
                return Attributes[NEGATE].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, NEGATE));
            return Null;
        }
        public virtual HassiumObject Index(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(INDEX))
                return Attributes[INDEX].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, INDEX));
            return Null;
        }
        public virtual HassiumObject StoreIndex(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(STOREINDEX))
                return Attributes[STOREINDEX].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, STOREINDEX));
            return Null;
        }
        public virtual HassiumObject Iter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(ITER))
                return Attributes[ITER].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, ITER));
            return Null;
        }
        public virtual HassiumObject IterableFull(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(ITERABLEFULL))
                return Attributes[ITERABLEFULL].Invoke(vm, location, args).ToBool(vm, location);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, ITERABLEFULL));
            return Null;
        }
        public virtual HassiumObject IterableNext(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(ITERABLENEXT))
                return Attributes[ITERABLENEXT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, ITERABLENEXT));
            return Null;
        }
        public virtual HassiumObject Dispose(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(DISPOSE))
                return Attributes[DISPOSE].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, DISPOSE));
            return Null;
        }
        public virtual HassiumBool ToBool(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOBOOL))
                return (HassiumBool)Attributes[TOBOOL].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOBOOL));
            return new HassiumBool(false);
        }
        public virtual HassiumChar ToChar(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOCHAR))
                return (HassiumChar)Attributes[TOCHAR].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOCHAR));
            return new HassiumChar('\0');
        }
        public virtual HassiumInt ToInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOINT))
                return (HassiumInt)Attributes[TOINT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOINT));
            return new HassiumInt(-1);
        }
        public virtual HassiumFloat ToFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOFLOAT))
                return (HassiumFloat)Attributes[TOFLOAT].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOFLOAT));
            return new HassiumFloat(-1);
        }
        public virtual HassiumList ToList(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOLIST))
                return (HassiumList)Attributes[TOLIST].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOLIST));
            return new HassiumList(new HassiumObject[0]);
        }
        public virtual HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOSTRING))
                return Attributes[TOSTRING].Invoke(vm, location, args).ToString(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOSTRING));
            return new HassiumString("");
        }
        public virtual HassiumTuple ToTuple(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(TOTUPLE))
                return (HassiumTuple)Attributes[TOTUPLE].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, TOTUPLE));
            return new HassiumTuple(new HassiumObject[0]);
        }
        
        public virtual HassiumObject Invoke(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(INVOKE))
                return Attributes[INVOKE].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, INVOKE));
            return Null;
        }
        
        public virtual HassiumObject Xor(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey(XOR))
                return Attributes[XOR].Invoke(vm, location, args);
            vm.RaiseException(new HassiumAttributeNotFoundException(this, XOR));
            return Null;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

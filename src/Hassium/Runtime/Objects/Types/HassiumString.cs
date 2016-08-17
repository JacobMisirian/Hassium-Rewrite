using System;
using System.Text;

namespace Hassium.Runtime.Objects.Types
{
    public class HassiumString: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("string");

        public static HassiumString Cast(HassiumObject obj)
        {
            if (obj is HassiumString)
                return (HassiumString)obj;
            throw new InternalException("Could not convert {0} to {1}", obj.Type(), TypeDefinition);
        }

        public string String { get; private set; }

        public HassiumString(string val)
        {
            AddType(TypeDefinition);
            String = val;
        }

        public override HassiumObject Add(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumString(String + args[0].ToString(vm, args).String);
        }
        public override HassiumObject EqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(String == args[0].ToString(vm, args).String);
        }
        public override HassiumObject Multiply(VirtualMachine vm, params HassiumObject[] args)
        {
            StringBuilder sb = new StringBuilder();
            int times = (int)args[0].ToInt(vm, args).Int;
            for (int i = 0; i < times; i++)
                sb.Append(String);
            return new HassiumString(sb.ToString());
        }
        public override HassiumObject NotEqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            return EqualTo(vm, args).LogicalNot(vm, args);
        }
        public override HassiumBool ToBool(VirtualMachine vm, params HassiumObject[] args)
        {
            switch (String.ToLower().Trim())
            {
                case "true":
                    return new HassiumBool(true);
                case "false":
                    return new HassiumBool(false);
                default:
                    throw new InternalException(InternalException.CONVERSION_ERROR, Type(), HassiumBool.TypeDefinition);
            }
        }
        public override HassiumObject Index(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumChar(String[(int)args[0].ToInt(vm).Int]);
        }
        public override HassiumChar ToChar(VirtualMachine vm, params HassiumObject[] args)
        {
            if (String.Trim().Length != 1)
                throw new InternalException(InternalException.CONVERSION_ERROR, Type(), HassiumChar.TypeDefinition);
            return new HassiumChar(String[0]);
        }
        public override HassiumFloat ToFloat(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Convert.ToDouble(String));
        }
        public override HassiumInt ToInt(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Convert.ToInt64(String));
        }
        public override HassiumList ToList(VirtualMachine vm, params HassiumObject[] args)
        {
            HassiumChar[] chars = new HassiumChar[String.Length];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = new HassiumChar(String[i]);
            return new HassiumList(chars);
        }
        public override HassiumString ToString(VirtualMachine vm, params HassiumObject[] args)
        {
            return this;
        }
    }
}


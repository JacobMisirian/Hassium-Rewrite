using Hassium.Compiler;

using System;

namespace Hassium.Runtime.Types
{
    public class HassiumString : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("string");

        public string String { get; private set; }

        public HassiumString(string val)
        {
            AddType(TypeDefinition);
            String = val;

            AddAttribute(EQUALTO, EqualTo, 1);
            AddAttribute("format", format, -1);
            AddAttribute(INDEX, Index, 1);
            AddAttribute(ITER, Iter, 0);
            AddAttribute("length", new HassiumProperty(get_length));
            AddAttribute(TOSTRING, ToString,   0);
        }

        public override HassiumBool EqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(String == args[0].ToString(vm, location).String);
        }

        public HassiumString format(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string[] strargs = new string[args.Length];
            for (int i = 0; i < strargs.Length; i++)
                strargs[i] = args[i].ToString(vm, location).String;
            return new HassiumString(string.Format(String, strargs));
        }

        public override HassiumObject Index(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumChar(String[(int)args[0].ToInt(vm, location).Int]);
        }

        public override HassiumObject Iter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);
            foreach (var c in String)
                list.add(vm, location, new HassiumChar(c));
            return list;
        }

        public HassiumInt get_length(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(String == null ? -1 : String.Length);
        }

        public override HassiumInt ToInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            try
            {
                return new HassiumInt(Convert.ToInt64(String));
            }
            catch
            {
                vm.RaiseException(HassiumConversionFailedException._new(vm, location, this, HassiumInt.TypeDefinition));
                return new HassiumInt(-1);
            }
        }

        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }
    }
}

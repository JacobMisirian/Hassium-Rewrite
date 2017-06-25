﻿using Hassium.Compiler;

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

            AddAttribute(TOSTRING, ToString,   0);
        }

        public override HassiumObject Iter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);
            foreach (var c in String)
                list.add(vm, location, new HassiumChar(c));
            return list;
        }

        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Hassium.Runtime.Objects.Types
{
    public class HassiumList: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("list");

        public List<HassiumObject> List { get; private set; }

        public HassiumList(HassiumObject[] elements)
        {
            AddType(TypeDefinition);
            List = new List<HassiumObject>(elements);
        }

        public override HassiumObject Index(VirtualMachine vm, params HassiumObject[] args)
        {
            return List[(int)args[0].ToInt(vm).Int];
        }
        public override HassiumList ToList(VirtualMachine vm, params HassiumObject[] args)
        {
            return this;
        }
    }
}


using System;

using Hassium.Runtime.Objects.Types;

namespace Hassium.Runtime.Objects
{
    public class HassiumTypeDefinition: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("TypeDefinition");

        public string TypeName { get; private set; }

        public HassiumTypeDefinition(string type)
        {
            TypeName = type;
            AddType(TypeDefinition);
        }

        public override Hassium.Runtime.Objects.Types.HassiumString ToString(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumString(TypeName);
        }
    }
}


using System;

namespace Hassium.Runtime.Objects
{
    public class HassiumTypeDefinition: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("TypeDefinition");

        public string TypeName { get; private set; }

        public HassiumTypeDefinition(string type)
        {
            TypeName = type;
        }

        public override string ToString()
        {
            return TypeName;
        }
    }
}


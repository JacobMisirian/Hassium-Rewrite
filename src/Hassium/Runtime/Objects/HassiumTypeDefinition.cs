using System;

namespace Hassium.Runtime.Objects
{
    public class HassiumTypeDefinition: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("TypeDefinition");

        public static HassiumTypeDefinition Cast(HassiumObject obj)
        {
            if (obj is HassiumTypeDefinition)
                return (HassiumTypeDefinition)obj;
            throw new InternalException("Could not convert {0} to {1}", obj.Type(), TypeDefinition);
        }

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


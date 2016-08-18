using System;

namespace Hassium.Runtime.Objects.Types
{
    public class TypesModule : InternalModule
    {
        public TypesModule() : base("types")
        {
            AddAttribute("bool",            HassiumBool.TypeDefinition);
            AddAttribute("char",            HassiumChar.TypeDefinition);
            AddAttribute("Event",           new HassiumEvent());
            AddAttribute("float",           HassiumFloat.TypeDefinition);
            AddAttribute("func",            HassiumFunction.TypeDefinition);
            AddAttribute("int",             HassiumInt.TypeDefinition);
            AddAttribute("list",            HassiumList.TypeDefinition);
            AddAttribute("object",          HassiumObject.TypeDefinition);
            AddAttribute("property",        HassiumProperty.TypeDefinition);
            AddAttribute("null",            HassiumObject.Null);
            AddAttribute("string",          HassiumString.TypeDefinition);
            AddAttribute("TypeDefinition",  HassiumTypeDefinition.TypeDefinition);
        }
    }
}


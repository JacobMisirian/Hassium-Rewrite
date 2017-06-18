using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime.Types
{
    public class HassiumTypesModule : InternalModule
    {
        public HassiumTypesModule() : base("types")
        {
            AddAttribute("bool", HassiumBool.TypeDefinition);
            AddAttribute("char", HassiumChar.TypeDefinition);
            AddAttribute("dictionary", HassiumDictionary.TypeDefinition);
            AddAttribute("float", HassiumFloat.TypeDefinition);
            AddAttribute("func", HassiumFunction.TypeDefinition);
            AddAttribute("int", HassiumInt.TypeDefinition);
            AddAttribute("list", HassiumList.TypeDefinition);
            AddAttribute("null", HassiumObject.Null);
            AddAttribute("number", HassiumObject.Number);
            AddAttribute("object", HassiumObject.TypeDefinition);
            AddAttribute("string", HassiumString.TypeDefinition);
            AddAttribute("tuple", HassiumTuple.TypeDefinition);
            AddAttribute("TypeDefinition", HassiumTypeDefinition.TypeDefinition);
        }
    }
}

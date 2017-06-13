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
            AddAttribute("func", HassiumFunction.TypeDefinition);
            AddAttribute("null", HassiumObject.Null);
            AddAttribute("object", HassiumObject.TypeDefinition);
            AddAttribute("TypeDefinition", HassiumTypeDefinition.TypeDefinition);
        }
    }
}

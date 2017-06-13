using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        }
    }
}

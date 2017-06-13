using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime.Types
{
    public class HassiumChar : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("char");

        public char Char { get; private set; }

        public HassiumChar(char val)
        {
            AddType(TypeDefinition);
            Char = val;
        }
    }
}

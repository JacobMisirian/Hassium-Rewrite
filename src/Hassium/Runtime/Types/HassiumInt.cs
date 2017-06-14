using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime.Types
{
    public class HassiumInt : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("int");

        public long Int { get; private set; }

        public HassiumInt(long val)
        {
            AddType(TypeDefinition);
            Int = val;
        }
    }
}

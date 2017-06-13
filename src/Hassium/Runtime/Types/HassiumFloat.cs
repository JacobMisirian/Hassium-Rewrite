using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime.Types
{
    public class HassiumFloat : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("float");

        public double Float { get; private set; }

        public HassiumFloat(double val)
        {
            AddType(TypeDefinition);
            Float = val;
        }
    }
}

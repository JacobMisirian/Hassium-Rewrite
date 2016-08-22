using System;
using System.Collections.Generic;

using Hassium.Runtime.Objects.IO;
using Hassium.Runtime.Objects.Math;
using Hassium.Runtime.Objects.Text;
using Hassium.Runtime.Objects.Types;

namespace Hassium.Runtime.Objects
{
    public class InternalModule: HassiumObject
    {
        public string Name { get; private set; }
        public InternalModule(string name)
        {
            Name = name;
        }

        public static Dictionary<string, InternalModule> InternalModules = new Dictionary<string, InternalModule>()
        {
            { "IO",     new HassiumIOModule()       },
            { "Math",   new HassiumMathModule()     },
            { "Text",   new HassiumTextModule()     },
            { "Types",  new HassiumTypesModule()    }
        };
    }
}


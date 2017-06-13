using System.Collections.Generic;

using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class InternalModule : HassiumObject
    {
        public string Name { get; private set; }
        public InternalModule(string name)
        {
            Name = name;
        }

        public static Dictionary<string, InternalModule> InternalModules = new Dictionary<string, InternalModule>()
        {
             { "Types",      new HassiumTypesModule()        }
        };
    }
}

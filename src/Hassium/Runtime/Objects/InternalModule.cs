﻿using System;
using System.Collections.Generic;

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

        public static List<InternalModule> InternalModules = new List<InternalModule>()
        {
            new TypesModule()
        };
    }
}

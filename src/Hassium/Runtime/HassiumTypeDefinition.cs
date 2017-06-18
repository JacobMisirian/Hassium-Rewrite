﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime
{
    public class HassiumTypeDefinition : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("TypeDefinition");

        public string TypeName { get; private set; }

        public HassiumTypeDefinition(string type)
        {
            TypeName = type;
            AddType(this);
        }

        public override string ToString()
        {
            return TypeName;
        }
    }
}

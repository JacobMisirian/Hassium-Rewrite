using System;

namespace Hassium.Runtime.Objects
{
    public class HassiumClass: HassiumObject
    {
        public string Name { get; set; }
        public new HassiumTypeDefinition TypeDefinition { get; set; }

        public HassiumClass()
        {
        }
    }
}


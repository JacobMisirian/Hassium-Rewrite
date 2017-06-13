using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime
{
    public class HassiumClass : HassiumObject
    {
        public string Name { get; set; }

        public new void AddAttribute(string name, HassiumObject obj)
        {
            obj.Parent = this;
            Attributes.Add(name, obj);
        }
    }
}

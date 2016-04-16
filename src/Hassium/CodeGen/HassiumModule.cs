using System;
using System.Collections.Generic;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.CodeGen
{
    public class HassiumModule: HassiumObject
    {
        public string Name { get; private set; }
        public List<string> ConstantPool { get; private set; }

        public HassiumModule(string name)
        {
            Name = name;
            ConstantPool = new List<string>();
        }
    }
}


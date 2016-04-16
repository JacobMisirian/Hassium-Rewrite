using System;
using System.Collections.Generic;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public abstract class HassiumObject
    {
        public Dictionary<string, HassiumObject> Attributes = new Dictionary<string, HassiumObject>();
        public object Value { get; private set; }
    }
}
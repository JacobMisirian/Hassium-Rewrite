using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;
using Hassium.Runtime.Exceptions;

namespace Hassium.Runtime.Types
{
    public class HassiumDictionary : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("dictionary");

        public Dictionary<HassiumObject, HassiumObject> Dictionary { get; private set; }

        public HassiumDictionary(Dictionary<HassiumObject, HassiumObject> initial)
        {
            Dictionary = new Dictionary<HassiumObject, HassiumObject>();
            AddType(TypeDefinition);
            foreach (var pair in initial)
                Dictionary.Add(pair.Key, pair.Value);
        }

        public override HassiumObject Iter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);
            foreach (var pair in Dictionary)
                list.add(vm, location, new HassiumTuple(pair.Key, pair.Value));
            return list;
        }
    }
}

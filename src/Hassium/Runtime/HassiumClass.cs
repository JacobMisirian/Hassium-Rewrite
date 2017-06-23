using System.Collections.Generic;
using Hassium.Compiler;

using Hassium.Runtime.Exceptions;

namespace Hassium.Runtime
{
    public class HassiumClass : HassiumObject
    {
        public string Name { get; set; }
        public new HassiumTypeDefinition TypeDefinition { get; private set; }
        public List<HassiumMethod> Inherits { get; private set; }

        public HassiumClass(string name)
        {
            Name = name;
            TypeDefinition = new HassiumTypeDefinition(name);
            Inherits = new List<HassiumMethod>();
        }

        public new void AddAttribute(string name, HassiumObject obj)
        {
            obj.Parent = this;
            Attributes.Add(name, obj);
        }

        public override HassiumObject Invoke(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Attributes.ContainsKey("new"))
                return Attributes["new"].Invoke(vm, location, args);
            else if (Attributes.ContainsKey(INVOKE))
                return Attributes[INVOKE].Invoke(vm, location, args);
            throw new InternalException(vm, location, InternalException.OPERATOR_ERROR, "()", TypeDefinition);
        }
    }
}

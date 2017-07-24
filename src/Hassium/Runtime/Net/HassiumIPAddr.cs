﻿using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.Net;

namespace Hassium.Runtime.Net
{
    public class HassiumIPAddr : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("IPAddr");
        
        public HassiumString Address { get; private set; }
        public HassiumInt Port { get; private set; }

        public HassiumIPAddr()
        {
            AddType(TypeDefinition);

            AddAttribute(INVOKE, _new, 1, 2);
        }

        public static HassiumIPAddr _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumIPAddr addr = new HassiumIPAddr();

            addr.Address = args[0].ToString(vm, location);
            addr.Port = args.Length == 2 ? args[1].ToInt(vm, location) : new HassiumInt(-1);
            addr.AddAttribute("address", new HassiumProperty(addr.get_address));
            addr.AddAttribute("port", new HassiumProperty(addr.get_port));
            addr.AddAttribute(TOSTRING, addr.toString, 0);
            
            return addr;
        }

        public HassiumString get_address(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Address;
        }

        public HassiumInt get_port(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Port;
        }

        public HassiumString toString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Port.Int == -1)
                return new HassiumString(Address.String);
            else
                return new HassiumString(string.Format("{0}:{1}", Address.String, Port.Int));
        }
    }
}

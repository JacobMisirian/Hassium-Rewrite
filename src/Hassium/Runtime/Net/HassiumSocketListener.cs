using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.Net;
using System.Net.Sockets;

namespace Hassium.Runtime.Net
{
    public class HassiumSocketListener : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("SocketListener");

        public TcpListener TcpListener { get; set; }

        public HassiumSocketListener()
        {
            AddType(TypeDefinition);

        }

        [FunctionAttribute("func new (portOrIPAddr : object) : SocketListener", "func new (ip : string, port) : SocketListener")]
        public static HassiumSocketListener _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumSocketListener listener = new HassiumSocketListener();

            switch (args.Length)
            {
                case 1:
                    if (args[0] is HassiumIPAddr)
                    {
                        var ip = args[0] as HassiumIPAddr;
                        listener.TcpListener = new TcpListener(IPAddress.Parse(ip.Address.String), (int)args[1].ToInt(vm, location).Int);
                    }
                    else
                        listener.TcpListener = new TcpListener(IPAddress.Any, (int)args[0].ToInt(vm, location).Int);
                    break;
                case 2:
                    listener.TcpListener = new TcpListener(IPAddress.Parse(args[0].ToString(vm, location).String), (int)args[1].ToInt(vm, location).Int);
                    break;
            }

            return listener;
        }
    }
}

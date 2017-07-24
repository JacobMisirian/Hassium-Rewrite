using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.IO;
using System.Net.Sockets;

namespace Hassium.Runtime.Net
{
    public class HassiumSocket : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("Socket");

        public TcpClient Client { get; set; }
        public BinaryReader Reader { get; set; }
        public BinaryWriter Writer { get; set; }

        public HassiumSocket()
        {
            AddType(TypeDefinition);

            AddAttribute(INVOKE, _new, 0, 1, 2);
        }

        public HassiumObject _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumSocket socket = new HassiumSocket();

            switch (args.Length)
            {
                case 0:
                    socket.Client = new TcpClient();
                    break;
                case 1:
                    if (args[0] is HassiumIPAddr)
                    {
                        var ipAddr = args[0] as HassiumIPAddr;
                        socket.Client = new TcpClient(ipAddr.Address.String, (int)ipAddr.Port.Int);
                    }
                    else
                        return _new(vm, location, InternalModule.InternalModules["Net"].Attributes["IPAddr"].Attributes[INVOKE].Invoke(vm, location, args[0].ToString(vm, location)));
                    break;
                case 2:
                    socket.Client = new TcpClient(args[0].ToString(vm, location).String, (int)args[1].ToInt(vm, location).Int);
                    break;
            }
            socket.Reader = new BinaryReader(socket.Client.GetStream());
            socket.Writer = new BinaryWriter(socket.Client.GetStream());
            socket.AddAttribute("close", close, 0);
            socket.AddAttribute("connect", socket.connect, 1, 2);
            socket.AddAttribute("isConnected", new HassiumProperty(socket.get_isConnected));

            return socket;
        }

        public HassiumNull close(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            Client.Close();
            return Null;
        }

        public HassiumNull connect(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    if (args[0] is HassiumIPAddr)
                    {
                        var ipAddr = args[0] as HassiumIPAddr;
                        Client.Connect(ipAddr.Address.String, (int)ipAddr.Port.Int);
                        return Null;
                    }
                    else
                        return connect(vm, location, InternalModule.InternalModules["Net"].Attributes["IPAddr"].Attributes[INVOKE].Invoke(vm, location, args[0].ToString(vm, location)));
                case 2:
                    Client.Connect(args[0].ToString(vm, location).String, (int)args[1].ToInt(vm, location).Int);
                    return Null;
            }
            return Null;
        }

        public HassiumBool get_isConnected(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Client.Connected);
        }
    }
}

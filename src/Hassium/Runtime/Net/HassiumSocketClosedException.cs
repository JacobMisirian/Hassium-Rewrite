using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.Net;

namespace Hassium.Runtime.Net
{
    public class HassiumSocketClosedException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("ConnectionClosedException");

        public HassiumSocket Socket { get; set; }

        public HassiumSocketClosedException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 1);
        }

        public static HassiumSocketClosedException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumSocketClosedException exception = new HassiumSocketClosedException();

            exception.Socket = args[0] as HassiumSocket;
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute("socket", new HassiumProperty(exception.get_socket));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Socket Closed: The connection to '{0}' has been terminated", (Socket.Client.Client.RemoteEndPoint as IPEndPoint).Address));
        }

        public HassiumSocket get_socket(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Socket;
        }
    }
}

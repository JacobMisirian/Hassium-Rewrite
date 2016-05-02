using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Hassium.Runtime.StandardLibrary.IO;
using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.Net
{
    public class HassiumNetConnection: HassiumObject
    {
        public TcpClient TcpClient { get; set; }

        public HassiumNetConnection()
        {
            Attributes.Add(HassiumObject.INVOKE_FUNCTION, new HassiumFunction(_new, 2));
        }

        private HassiumNetConnection _new(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumNetConnection hassiumNetConnection = new HassiumNetConnection();

            hassiumNetConnection.TcpClient = new TcpClient(HassiumString.Create(args[0]).Value, (int)HassiumInt.Create(args[1]).Value);
            hassiumNetConnection.Attributes.Add("close", new HassiumFunction(close, 0));
            hassiumNetConnection.Attributes.Add("getStream", new HassiumFunction(getStream, 0));

            return hassiumNetConnection;
        }

        public HassiumNull close(VirtualMachine vm, HassiumObject[] args)
        {
            TcpClient.Close();
            return HassiumObject.Null;
        }
        public HassiumStream getStream(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumStream(TcpClient.GetStream());
        }
    }
}
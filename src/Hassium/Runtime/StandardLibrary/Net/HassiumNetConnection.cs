using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.Net
{
    public class HassiumNetConnection: HassiumObject
    {
        public TcpClient TcpClient { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }

        public HassiumNetConnection()
        {
            Attributes.Add(HassiumObject.INVOKE_FUNCTION, new HassiumFunction(_new, 2));
        }

        private HassiumNetConnection _new(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumNetConnection hassiumNetConnection = new HassiumNetConnection();

            hassiumNetConnection.TcpClient = new TcpClient(HassiumString.Create(args[0]).Value, (int)HassiumInt.Create(args[1]).Value);
            hassiumNetConnection.StreamReader = new StreamReader(hassiumNetConnection.TcpClient.GetStream());
            hassiumNetConnection.StreamWriter = new StreamWriter(hassiumNetConnection.TcpClient.GetStream());

            hassiumNetConnection.Attributes.Add("read", new HassiumFunction(hassiumNetConnection.read, 0));
            hassiumNetConnection.Attributes.Add("readLine", new HassiumFunction(hassiumNetConnection.readLine, 0));
            hassiumNetConnection.Attributes.Add("write", new HassiumFunction(hassiumNetConnection.write, 1));
            hassiumNetConnection.Attributes.Add("writeLine", new HassiumFunction(hassiumNetConnection.writeLine, 1));

            return hassiumNetConnection;
        }

        public HassiumInt read(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumInt(StreamReader.Read());
        }
        public HassiumString readLine(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(StreamReader.ReadLine());
        }
        public HassiumNull write(VirtualMachine vm, HassiumObject[] args)
        {
            if (args[0] is HassiumBool)
                StreamWriter.Write(HassiumBool.Create(args[0]).Value);
            else if (args[0] is HassiumChar)
                StreamWriter.Write(HassiumChar.Create(args[0]).Value);
            else if (args[0] is HassiumDouble)
                StreamWriter.Write(HassiumDouble.Create(args[0]).Value);
            else if (args[0] is HassiumInt)
                StreamWriter.Write(HassiumInt.Create(args[0]).Value);
            else if (args[0] is HassiumList)
                foreach (HassiumObject obj in HassiumList.Create(args[0]).Value)
                    write(vm, new HassiumObject[] { obj });
            else if (args[0] is HassiumString)
                StreamWriter.Write(HassiumString.Create(args[0]).Value);
            else
                throw new InternalException("Unknown type " + args[0].GetType().Name);

            return HassiumObject.Null;
        }
        public HassiumNull writeLine(VirtualMachine vm, HassiumObject[] args)
        {
            write(vm, args);
            write(vm, new HassiumObject[] { new HassiumString("\r\n") });

            return HassiumObject.Null;
        }
    }
}
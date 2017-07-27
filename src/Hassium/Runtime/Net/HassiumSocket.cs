using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Hassium.Runtime.Net
{
    public class HassiumSocket : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("Socket");

        public TcpClient Client { get; set; }
        public BinaryReader Reader { get; set; }
        public BinaryWriter Writer { get; set; }

        public bool AutoFlush { get; set; }
        public bool Closed { get; set; }

        public HassiumSocket()
        {
            AddType(TypeDefinition);

            AddAttribute(INVOKE, _new, 0, 1, 2);
        }

        [FunctionAttribute("func new () : Socket, func new (ip : IPAddr) : Socket, func new (ipStr : string) : Socket, func new (ip : string, port : int) : Socket")]
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
                        return _new(vm, location, HassiumIPAddr._new(vm, location, args[0].ToString(vm, location)));
                    break;
                case 2:
                    socket.Client = new TcpClient(args[0].ToString(vm, location).String, (int)args[1].ToInt(vm, location).Int);
                    break;
            }
            socket.Reader = new BinaryReader(socket.Client.GetStream());
            socket.Writer = new BinaryWriter(socket.Client.GetStream());
            socket.AutoFlush = true;
            socket.Closed = false;
            socket.AddAttribute("autoFlush", new HassiumProperty(socket.get_autoFlush, socket.set_autoFlush));
            socket.AddAttribute("close", socket.close, 0);
            socket.AddAttribute("connect", socket.connect, 1, 2);
            socket.AddAttribute("connectedFrom", new HassiumProperty(socket.get_connectedFrom));
            socket.AddAttribute("connectedTo", new HassiumProperty(socket.get_connectedTo));
            socket.AddAttribute("flush", socket.flush, 0);
            socket.AddAttribute("isConnected", new HassiumProperty(socket.get_isConnected));
            socket.AddAttribute("readInt", socket.readInt, 0);
            socket.AddAttribute("readLine", socket.readLine, 0);
            socket.AddAttribute("readList", socket.readList, 1);
            socket.AddAttribute("readLong", socket.readLong, 0);
            socket.AddAttribute("readShort", socket.readShort, 0);
            socket.AddAttribute("readString", socket.readString, 0);
            socket.AddAttribute("writeByte", socket.writeByte, 1);
            socket.AddAttribute("writeFloat", socket.writeFloat, 1);
            socket.AddAttribute("writeInt", socket.writeInt, 1);
            socket.AddAttribute("writeLine", socket.writeLine, 1);
            socket.AddAttribute("writeList", socket.writeList, 1);
            socket.AddAttribute("writeLong", socket.writeLong, 1);
            socket.AddAttribute("writeShort", socket.writeShort, 1);
            socket.AddAttribute("writeString", socket.writeString, 1);

            return socket;
        }

        [FunctionAttribute("autoFluah { get; }")]
        public HassiumBool get_autoFlush(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(AutoFlush);
        }

        [FunctionAttribute("autoFluah { set; }")]
        public HassiumNull set_autoFlush(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            AutoFlush = args[0].ToBool(vm, location).Bool;

            return Null;
        }

        [FunctionAttribute("func close () : null")]
        public HassiumNull close(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            Client.Close();
            return Null;
        }

        [FunctionAttribute("func connect (ip : IPAddr) : null, func connect (ipStr : string) : null, func connect (ip : string, port : int) : null")]
        public HassiumNull connect(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    if (args[0] is HassiumIPAddr)
                    {
                        var ipAddr = args[0] as HassiumIPAddr;
                        Client = new TcpClient(ipAddr.Address.String, (int)ipAddr.Port.Int);
                        return Null;
                    }
                    else
                        return connect(vm, location, HassiumIPAddr._new(vm, location, args[0].ToString(vm, location)));
                case 2:
                    Client = new TcpClient(args[0].ToString(vm, location).String, (int)args[1].ToInt(vm, location).Int);
                    return Null;
            }
            return Null;
        }

        [FunctionAttribute("connectedFrom { get; }")]
        public HassiumObject get_connectedFrom(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }
            string[] parts = (Client.Client.LocalEndPoint as IPEndPoint).ToString().Split(':');
            return HassiumIPAddr._new(vm, location, new HassiumString(parts[0]), new HassiumString(parts[1]));
        }

        [FunctionAttribute("connectedTo { get; }")]
        public HassiumObject get_connectedTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }
            string[] parts = (Client.Client.RemoteEndPoint as IPEndPoint).ToString().Split(':');
            return HassiumIPAddr._new(vm, location, new HassiumString(parts[0]), new HassiumString(parts[1]));
        }

        [FunctionAttribute("func flish () : null")]
        public HassiumNull flush(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Flush();
            return Null;
        }

        [FunctionAttribute("isConnected { get; }")]
        public HassiumBool get_isConnected(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Client.Connected);
        }

        [FunctionAttribute("func readByte () : char")]
        public HassiumObject readByte(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {

            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumChar((char)Reader.ReadBytes(1)[0]);
        }

        [FunctionAttribute("func readBytes (count : int) : list")]
        public HassiumObject readList(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            HassiumList list = new HassiumList(new HassiumObject[0]);
            int count = (int)args[0].ToInt(vm, location).Int;
            for (int i = 0; i < count; i++)
                list.add(vm, location, new HassiumChar((char)Reader.ReadBytes(1)[0]));

            return list;
        }

        [FunctionAttribute("func readFloat () : float")]
        public HassiumObject readFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {

            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumFloat(Reader.ReadDouble());
        }

        [FunctionAttribute("func readInt () : int")]
        public HassiumObject readInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {

            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumInt(Reader.ReadInt32());
        }

        [FunctionAttribute("func readLine () : string")]
        public HassiumObject readLine(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {

            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumString(new StreamReader(Reader.BaseStream).ReadLine());
        }

        [FunctionAttribute("func readLong () : int")]
        public HassiumObject readLong(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumInt(Reader.ReadInt64());
        }

        [FunctionAttribute("func readShort () : int")]
        public HassiumObject readShort(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {

            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumInt(Reader.ReadInt16());
        }

        [FunctionAttribute("func readString () : string")]
        public HassiumObject readString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            return new HassiumString(Reader.ReadString());
        }

        [FunctionAttribute("func writeByte (b : char) : null")]
        public HassiumNull writeByte(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Write((byte)args[0].ToChar(vm, location).Char);
            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        [FunctionAttribute("func writeFloat (f : float) : null")]
        public HassiumNull writeFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Write(args[0].ToFloat(vm, location).Float);
            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        [FunctionAttribute("func writeInt (i : int) : null")]
        public HassiumNull writeInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Write((int)args[0].ToInt(vm, location).Int);
            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        [FunctionAttribute("func writeLine (str : string) : null")]
        public HassiumNull writeLine(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            string str = args[0].ToString(vm, location).String;

            new StreamWriter(Writer.BaseStream).WriteLine(str);

            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        [FunctionAttribute("func writeList (l : list) : null")]
        public HassiumNull writeList(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            foreach (var i in args[0].ToList(vm, location).Values)
                writeHassiumObject(Writer, i, vm, location);

            return Null;
        }

        [FunctionAttribute("func writeLong (l : int) : null")]
        public HassiumNull writeLong(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Write(args[0].ToInt(vm, location).Int);
            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        [FunctionAttribute("func writeShort (s : int) : null")]
        public HassiumNull writeShort(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Write((short)args[0].ToInt(vm, location).Int);
            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        [FunctionAttribute("func writeString (str : string) : null")]
        public HassiumNull writeString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Closed)
            {
                vm.RaiseException(HassiumSocketClosedException._new(vm, location, this));
                return Null;
            }

            Writer.Write(args[0].ToString(vm, location).String);
            if (AutoFlush)
                Writer.Flush();
            return Null;
        }

        private void writeHassiumObject(BinaryWriter writer, HassiumObject obj, VirtualMachine vm, SourceLocation location)
        {
            var type = obj.Type();

            if (type == HassiumBool.TypeDefinition)
                writer.Write(obj.ToBool(vm, location).Bool);
            else if (type == HassiumChar.TypeDefinition)
                writer.Write((byte)obj.ToChar(vm, location).Char);
            else if (type == HassiumFloat.TypeDefinition)
                writer.Write(obj.ToFloat(vm, location).Float);
            else if (type == HassiumInt.TypeDefinition)
                writer.Write(obj.ToInt(vm, location).Int);
            else if (type == HassiumList.TypeDefinition)
                foreach (var item in obj.ToList(vm, location).Values)
                    writeHassiumObject(writer, item, vm, location);
            else if (type == HassiumString.TypeDefinition)
                writer.Write(obj.ToString(vm, location).String);
            else if (type == HassiumTuple.TypeDefinition)
                foreach (var item in obj.ToTuple(vm, location).Values)
                    writeHassiumObject(writer, item, vm, location);
        }
    }
}

using System.IO;
using System.Text;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumFile : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("File");

        public HassiumString AbsolutePath { get; private set; }
        public HassiumString RelativePath { get; private set; }

        public FileInfo FileInfo { get; private set; }

        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }

        private bool closed = false;
        private bool autoFlush = true;

        public HassiumFile(string path)
        {
            AddType(TypeDefinition);

            AbsolutePath = new HassiumString(Path.GetFullPath(path));
            RelativePath = new HassiumString(path);

            FileInfo = new FileInfo(path);
            var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Reader = new BinaryReader(fs);
            Writer = new BinaryWriter(fs);

            AddAttribute("absolutePath", new HassiumProperty(get_absolutePath));
            AddAttribute("autoFlush", new HassiumProperty(get_autoFlush, set_autoFlush));
            AddAttribute("close", close, 0);
            AddAttribute("copyTo", copyTo, 1);
            AddAttribute("exists", new HassiumProperty(get_exists));
            AddAttribute("extension", new HassiumProperty(get_extension, set_extension));
            AddAttribute("flush", flush, 0);
            AddAttribute("isClosed", new HassiumProperty(get_isClosed));
            AddAttribute("length", new HassiumProperty(get_length));
            AddAttribute("moveTo", moveTo, 1);
            AddAttribute("name", new HassiumProperty(get_name, set_name));
            AddAttribute("position", new HassiumProperty(get_position, set_position));
            AddAttribute("readAllByteS", readAllBytes, 0);
            AddAttribute("readAllLines", readAllLines, 0);
            AddAttribute("readAllText", readAllText, 0);
            AddAttribute("readInt", readInt, 0);
            AddAttribute("readLine", readLine, 0);
            AddAttribute("readList", readList, 1);
            AddAttribute("readLong", readLong, 0);
            AddAttribute("readShort", readShort, 0);
            AddAttribute("readString", readString, 0);
            AddAttribute("relativePath", new HassiumProperty(get_relativePath));
            AddAttribute("size", new HassiumProperty(get_size));
            AddAttribute("writeAllBytes", writeAllBytes, 1);
            AddAttribute("writeAllLines", writeAllLines, -1);
            AddAttribute("writeAllText", writeAllText, 1);
            AddAttribute("writeByte", writeByte, 1);
            AddAttribute("writeFloat", writeFloat, 1);
            AddAttribute("writeInt", writeInt, 1);
            AddAttribute("writeLine", writeLine, 1);
            AddAttribute("writeList", writeList, 1);
            AddAttribute("writeLong", writeLong, 1);
            AddAttribute("writeShort", writeShort, 1);
            AddAttribute("writeString", writeString, 1);
        }

        public HassiumString get_absolutePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return AbsolutePath;
        }

        public HassiumBool get_autoFlush(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(autoFlush);
        }

        public HassiumNull set_autoFlush(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            autoFlush = args[0].ToBool(vm, location).Bool;

            return Null;
        }

        public HassiumNull close(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            try
            {
                Reader.Close();
                Writer.Close();
                return Null;
            }
            finally
            {
                closed = true;
            }
        }

        public HassiumNull copyTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            File.Copy(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumNull delete(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            File.Delete(AbsolutePath.String);

            return Null;
        }

        public HassiumBool get_exists(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(File.Exists(AbsolutePath.String));
        }

        public HassiumString get_extension(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(FileInfo.Extension);
        }
        public HassiumNull set_extension(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            moveTo(vm, location, new HassiumString(Path.ChangeExtension(AbsolutePath.String, args[0].ToString(vm, location).String)));

            return Null;
        }

        public HassiumNull flush(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Flush();
            return Null;
        }

        public HassiumBool get_isClosed(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(closed);
        }

        public HassiumInt get_length(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Reader.BaseStream.Length);
        }

        public HassiumNull moveTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            File.Move(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumString get_name(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetFileName(AbsolutePath.String));
        }
        public HassiumNull set_name(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            File.Move(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumInt get_position(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Reader.BaseStream.Position);
        }
        public HassiumNull set_position(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            Reader.BaseStream.Position = args[0].ToInt(vm, location).Int;
            return Null;
        }

        public HassiumObject readAllBytes(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            HassiumList list = new HassiumList(new HassiumObject[0]);

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
                list.add(vm, location, new HassiumChar((char)Reader.ReadBytes(1)[0]));

            return list;
        }

        public HassiumObject readAllLines(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            HassiumList list = new HassiumList(new HassiumObject[0]);

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
                list.add(vm, location, readLine(vm, location));

            return list;
        }

        public HassiumObject readAllText(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            StringBuilder sb = new StringBuilder();

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
                sb.AppendLine(readLine(vm, location).ToString(vm, location).String);

            return new HassiumString(sb.ToString());
        }

        public HassiumObject readByte(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumChar((char)Reader.ReadBytes(1)[0]);
        }

        public HassiumObject readFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumFloat(Reader.ReadDouble());
        }

        public HassiumObject readInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumInt(Reader.ReadInt32());
        }

        public HassiumObject readLine(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumString(new StreamReader(Reader.BaseStream).ReadLine());
        }

        public HassiumObject readList(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            HassiumList list = new HassiumList(new HassiumObject[0]);
            int count = (int)args[0].ToInt(vm, location).Int;
            for (int i = 0; i < count; i++)
                list.add(vm, location, new HassiumChar((char)Reader.ReadBytes(1)[0]));

            return list;
        }

        public HassiumObject readLong(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumInt(Reader.ReadInt64());
        }

        public HassiumObject readShort(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumInt(Reader.ReadInt16());
        }

        public HassiumObject readString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!File.Exists(AbsolutePath.String))
            {
                vm.RaiseException(HassiumFileNotFoundException._new(vm, location, AbsolutePath));
                return Null;
            }

            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            return new HassiumString(Reader.ReadString());
        }

        public HassiumString get_relativePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return RelativePath;
        }

        public HassiumInt get_size(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(FileInfo.Length);
        }

        public HassiumNull writeAllBytes(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            for (int i = 0; i < args.Length; i++)
                writeHassiumObject(Writer, args[i], vm, location);

            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeAllLines(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var type = args[i].Type();

                if (type == HassiumList.TypeDefinition)
                    foreach (var item in args[i].ToList(vm, location).Values)
                        writeLine(vm, location, item.ToString(vm, location));
                else if (type == HassiumString.TypeDefinition)
                    writeLine(vm, location, args[i].ToString(vm, location));
            }

            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeAllText(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            foreach (var c in args[0].ToString(vm, location).String)
                Writer.Write(c);
            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeByte(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Write((byte)args[0].ToChar(vm, location).Char);
            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Write(args[0].ToFloat(vm, location).Float);
            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Write((int)args[0].ToInt(vm, location).Int);
            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeLine(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            string str = args[0].ToString(vm, location).String;

            new StreamWriter(Writer.BaseStream).WriteLine(str);

            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeList(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            foreach (var i in args[0].ToList(vm, location).Values)
                writeHassiumObject(Writer, i, vm, location);

            return Null;
        }

        public HassiumNull writeLong(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Write(args[0].ToInt(vm, location).Int);
            if (autoFlush)
                Writer.Flush();
            return Null;
        }

        public HassiumNull writeShort(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Write((short)args[0].ToInt(vm, location).Int);
            if (autoFlush)
                Writer.Flush();
            return Null;
        }
        
        public HassiumNull writeString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (closed)
            {
                vm.RaiseException(HassiumFileClosedException._new(vm, location, this, get_absolutePath(vm, location)));
                return Null;
            }

            Writer.Write(args[0].ToString(vm, location).String);
            if (autoFlush)
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

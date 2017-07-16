using System;
using System.IO;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumFS : HassiumObject
    {
        public HassiumFS()
        {
            AddAttribute("oepn", open, 1);
            AddAttribute("readBytes", readBytes, 1);
            AddAttribute("readLines", readLines, 1);
            AddAttribute("readString", readString, 1);
        }

        public HassiumFile open(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string path = args[0].ToString(vm, location).String;
            if (!File.Exists(path))
                File.Create(path);
            return new HassiumFile(path);
        }

        public HassiumList readBytes(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);

            var stream = new FileStream(args[0].ToString(vm, location).String, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var reader = new BinaryReader(stream);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
                list.add(vm, location, new HassiumChar((char)reader.ReadBytes(1)[0]));

            return list;
        }

        public HassiumList readLines(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);

            var stream = new FileStream(args[0].ToString(vm, location).String, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var reader = new StreamReader(stream);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
                list.add(vm, location, new HassiumString(reader.ReadLine()));

            return list;
        }

        public HassiumString readString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(File.ReadAllText(args[0].ToString(vm, location).String));
        }
    }
}

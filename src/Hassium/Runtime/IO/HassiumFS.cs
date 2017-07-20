﻿using System.IO;
using System.Text;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumFS : HassiumObject
    {
        public HassiumFS()
        {
            AddAttribute("copy", copy, 2);
            AddAttribute("move", move, 2);
            AddAttribute("open", open, 1);
            AddAttribute("readBytes", readBytes, 1);
            AddAttribute("readLines", readLines, 1);
            AddAttribute("readString", readString, 1);
            AddAttribute("writeBytes", writeBytes, -1);
            AddAttribute("writeLines", writeLines, -1);
            AddAttribute("writeString", writeString, -1);
        }

        public HassiumNull close(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Null;
        }

        public HassiumNull copy(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string source = args[0].ToString(vm, location).String;
            HassiumFileNotFoundException.VerifyPath(vm, source);
            File.Copy(source, args[1].ToString(vm, location).String);

            return Null;
        }

        public HassiumNull move(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string source = args[0].ToString(vm, location).String;
            HassiumFileNotFoundException.VerifyPath(vm, source);
            File.Move(source, args[1].ToString(vm, location).String);

            return Null;
        }

        public HassiumFile open(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string path = args[0].ToString(vm, location).String;
            return new HassiumFile(path);
        }

        public HassiumList readBytes(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);

            var stream = new FileStream(args[0].ToString(vm, location).String, FileMode.Open, FileAccess.Read, FileShare.Read);
            var reader = new BinaryReader(stream);

            try
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                    list.add(vm, location, new HassiumChar((char)reader.ReadBytes(1)[0]));

                return list;
            }
            finally
            {
                reader.Close();
            }
        }

        public HassiumList readLines(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);

            var stream = new FileStream(args[0].ToString(vm, location).String, FileMode.Open, FileAccess.Read, FileShare.Read);
            var reader = new StreamReader(stream);

            try
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                    list.add(vm, location, new HassiumString(reader.ReadLine()));

                return list;
            }
            finally
            {
                reader.Close();
            }
        }

        public HassiumString readString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder sb = new StringBuilder();

            var stream = new FileStream(args[0].ToString(vm, location).String, FileMode.Open, FileAccess.Read, FileShare.Read);
            var reader = new StreamReader(stream);

            try
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                    sb.AppendLine(reader.ReadLine());

                return new HassiumString(sb.ToString());
            }
            finally
            {
                reader.Close();
            }
        }

        public HassiumNull writeBytes(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string path = args[0].ToString(vm, location).String;

            var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            var writer = new BinaryWriter(stream);

            try
            {
                for (int i = 1; i < args.Length; i++)
                    writeHassiumObject(writer, args[i], vm, location);

                return Null;
            }
            finally
            {
                writer.Close();
            }
        }

        public HassiumNull writeLines(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string path = args[0].ToString(vm, location).String;

            var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            var writer = new StreamWriter(stream);

            try
            {
                for (int i = 1; i < args.Length; i++)
                {
                    var type = args[i].Type();

                    if (type == HassiumList.TypeDefinition)
                        foreach (var item in args[i].ToList(vm, location).Values)
                            writer.WriteLine(item.ToString(vm, location).String);
                    else if (type == HassiumString.TypeDefinition)
                        writer.WriteLine(args[i].ToString(vm, location).String);
                    writer.Flush();
                }

                return Null;
            }
            finally
            {
                writer.Close();
            }
        }

        public HassiumNull writeString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string path = args[0].ToString(vm, location).String;

            File.WriteAllText(path, args[1].ToString(vm, location).String);

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
            writer.Flush();
        }
    }
}

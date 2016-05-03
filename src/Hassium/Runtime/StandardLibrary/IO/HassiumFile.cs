using System;
using System.Collections.Generic;
using System.IO;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.IO
{
    public class HassiumFile: HassiumObject
    {
        public HassiumFile()
        {
            Attributes.Add("currentDirectory",  new HassiumProperty(get_CurrentDirectory, set_CurrentDirectory));
            Attributes.Add("getDirectories",    new HassiumFunction(getDirectories, 1));
            Attributes.Add("getFiles",          new HassiumFunction(getFiles, 1));
            Attributes.Add("readBytes",         new HassiumFunction(readBytes, 1));
            Attributes.Add("readLines",         new HassiumFunction(readLines, 1));
            Attributes.Add("readText",          new HassiumFunction(readText, 1));
            Attributes.Add("writeBytes",        new HassiumFunction(writeBytes, 2));
            Attributes.Add("writeLines",        new HassiumFunction(writeLines, 2));
            Attributes.Add("writeText",         new HassiumFunction(writeText, 2));
        }

        private HassiumString get_CurrentDirectory(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(Directory.GetCurrentDirectory());
        }
        private HassiumNull set_CurrentDirectory(VirtualMachine vm, HassiumObject[] args)
        {
            Directory.SetCurrentDirectory(HassiumString.Create(args[0]).Value);

            return HassiumObject.Null;
        }
        private HassiumList getDirectories(VirtualMachine vm, HassiumObject[] args)
        {
            string[] dirs = Directory.GetDirectories(HassiumString.Create(args[0]).Value);
            HassiumString[] elements = new HassiumString[dirs.Length];
            for (int i = 0; i < elements.Length; i++)
                elements[i] = new HassiumString(dirs[i]);

            return new HassiumList(elements);
        }
        private HassiumList getFiles(VirtualMachine vm, HassiumObject[] args)
        {
            string[] files = Directory.GetFiles(HassiumString.Create(args[0]).Value);
            HassiumString[] elements = new HassiumString[files.Length];
            for (int i = 0; i < elements.Length; i++)
                elements[i] = new HassiumString(files[i]);

            return new HassiumList(elements);
        }
        private HassiumList readBytes(VirtualMachine vm, HassiumObject[] args)
        {
            BinaryReader reader = new BinaryReader(new StreamReader(HassiumString.Create(args[0]).Value).BaseStream);
            List<HassiumChar> bytes = new List<HassiumChar>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
                bytes.Add(new HassiumChar((char)reader.ReadByte()));

            return new HassiumList(bytes.ToArray());
        }
        private HassiumList readLines(VirtualMachine vm, HassiumObject[] args)
        {
            StreamReader reader = new StreamReader(HassiumString.Create(args[0]).Value);
            List<HassiumString> strings = new List<HassiumString>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
                strings.Add(new HassiumString(reader.ReadLine()));

            return new HassiumList(strings.ToArray());
        }
        private HassiumString readText(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(File.ReadAllText(HassiumString.Create(args[0]).Value));
        }
        private HassiumNull writeBytes(VirtualMachine vm, HassiumObject[] args)
        {
            BinaryWriter writer = new BinaryWriter(new StreamWriter(HassiumString.Create(args[0]).Value).BaseStream);
            HassiumList chars = HassiumList.Create(args[1]);
            foreach (HassiumObject obj in chars.Value)
                writer.Write((byte)HassiumChar.Create(obj).Value);
            writer.Flush();
            writer.Close();

            return HassiumObject.Null;
        }
        private HassiumNull writeLines(VirtualMachine vm, HassiumObject[] args)
        {
            StreamWriter writer = new StreamWriter(HassiumString.Create(args[0]).Value);
            HassiumList strings = HassiumList.Create(args[1]);
            foreach (HassiumObject obj in strings.Value)
                writer.WriteLine(HassiumString.Create(obj).Value);
            writer.Flush();
            writer.Close();

            return HassiumObject.Null;
        }
        private HassiumNull writeText(VirtualMachine vm, HassiumObject[] args)
        {
            File.WriteAllText(HassiumString.Create(args[0]).Value, HassiumString.Create(args[1]).Value);

            return HassiumObject.Null;
        }
    }
}


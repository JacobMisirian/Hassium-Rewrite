using System;
using System.IO;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.IO
{
    public class HassiumFileReader: HassiumObject
    {
        public new BinaryReader Value { get; set; }

        public HassiumFileReader()
        {
            Attributes.Add(HassiumObject.INVOKE_FUNCTION, new HassiumFunction(_new, 1));
        }

        private HassiumFileReader _new(HassiumObject[] args)
        {
            HassiumFileReader hassiumFileReader = new HassiumFileReader();

            hassiumFileReader.Value = new BinaryReader(new StreamReader(HassiumString.Create(args[0]).Value).BaseStream);
            hassiumFileReader.Attributes.Add("endOfFile", new HassiumProperty(get_EndOfFile));
            hassiumFileReader.Attributes.Add("length", new HassiumProperty(get_Length));
            hassiumFileReader.Attributes.Add("position", new HassiumProperty(get_Position));
            hassiumFileReader.Attributes.Add("readBool", new HassiumFunction(readBool, 0));
            hassiumFileReader.Attributes.Add("readByte", new HassiumFunction(readByte, 0));
            hassiumFileReader.Attributes.Add("readChar", new HassiumFunction(readChar, 0));
            hassiumFileReader.Attributes.Add("readDouble", new HassiumFunction(readDouble, 0));
            hassiumFileReader.Attributes.Add("readString", new HassiumFunction(readString, 0));

            return hassiumFileReader;
        }
        private HassiumBool get_EndOfFile(HassiumObject[] args)
        {
            return new HassiumBool(Value.BaseStream.Position < Value.BaseStream.Length);
        }
        private HassiumDouble get_Length(HassiumObject[] args)
        {
            return new HassiumDouble(Value.BaseStream.Length);
        }
        private HassiumDouble get_Position(HassiumObject[] args)
        {
            return new HassiumDouble(Value.BaseStream.Position);
        }
        private HassiumBool readBool(HassiumObject[] args)
        {
            return new HassiumBool(Value.ReadBoolean());
        }
        private HassiumChar readByte(HassiumObject[] args)
        {
            return new HassiumChar(Convert.ToChar(Value.ReadByte()));
        }
        private HassiumChar readChar(HassiumObject[] args)
        {
            return new HassiumChar(Value.ReadChar());
        }
        private HassiumDouble readDouble(HassiumObject[] args)
        {
            return new HassiumDouble(Value.ReadDouble());
        }
        private HassiumString readString(HassiumObject[] args)
        {
            return new HassiumString(Value.ReadString());
        }
    }
}


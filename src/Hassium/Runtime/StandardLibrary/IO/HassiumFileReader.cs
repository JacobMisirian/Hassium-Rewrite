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
            hassiumFileReader.Attributes.Add("endOfFile", new HassiumProperty(hassiumFileReader.get_EndOfFile));
            hassiumFileReader.Attributes.Add("length", new HassiumProperty(hassiumFileReader.get_Length));
            hassiumFileReader.Attributes.Add("position", new HassiumProperty(hassiumFileReader.get_Position));
            hassiumFileReader.Attributes.Add("readBool", new HassiumFunction(hassiumFileReader.readBool, 0));
            hassiumFileReader.Attributes.Add("readByte", new HassiumFunction(hassiumFileReader.readByte, 0));
            hassiumFileReader.Attributes.Add("readChar", new HassiumFunction(hassiumFileReader.readChar, 0));
            hassiumFileReader.Attributes.Add("readDouble", new HassiumFunction(hassiumFileReader.readDouble, 0));
            hassiumFileReader.Attributes.Add("readString", new HassiumFunction(hassiumFileReader.readString, 0));

            return hassiumFileReader;
        }
        public HassiumBool get_EndOfFile(HassiumObject[] args)
        {
            return new HassiumBool(Value.BaseStream.Position < Value.BaseStream.Length);
        }
        public HassiumDouble get_Length(HassiumObject[] args)
        {
            return new HassiumDouble(Value.BaseStream.Length);
        }
        public HassiumDouble get_Position(HassiumObject[] args)
        {
            return new HassiumDouble(Value.BaseStream.Position);
        }
        public HassiumBool readBool(HassiumObject[] args)
        {
            return new HassiumBool(Value.ReadBoolean());
        }
        public HassiumChar readByte(HassiumObject[] args)
        {
            return new HassiumChar(Convert.ToChar(Value.ReadByte()));
        }
        public HassiumChar readChar(HassiumObject[] args)
        {
            return new HassiumChar(Value.ReadChar());
        }
        public HassiumDouble readDouble(HassiumObject[] args)
        {
            return new HassiumDouble(Value.ReadDouble());
        }
        public HassiumString readString(HassiumObject[] args)
        {
            return new HassiumString(Value.ReadString());
        }
    }
}


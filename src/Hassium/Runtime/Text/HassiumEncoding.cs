using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.Text;

namespace Hassium.Runtime.Text
{
    public class HassiumEncoding : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("Encoding");

        public Encoding Encoding { get; private set; }

        public HassiumEncoding()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 1);
        }

        [FunctionAttribute("func new (scheme : string) : Encoding")]
        public static HassiumEncoding _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumEncoding encoding = new HassiumEncoding();

            switch (args[0].ToString(vm, location).String)
            {
                case "UNICODE":
                    encoding.Encoding = Encoding.Unicode;
                    break;
                case "UTF7":
                    encoding.Encoding = Encoding.UTF7;
                    break;
                case "UTF8":
                    encoding.Encoding = Encoding.UTF8;
                    break;
                case "UTF32":
                    encoding.Encoding = Encoding.UTF32;
                    break;
                default:
                    encoding.Encoding = Encoding.ASCII;
                    break;
            }

            return encoding;
        }

        [FunctionAttribute("bodyName { get; }")]
        public HassiumString get_bodyName(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Encoding.BodyName);
        }

        [FunctionAttribute("encodingName { get; }")]
        public HassiumString get_encodingName(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Encoding.EncodingName);
        }

        [FunctionAttribute("func getBytes (str : string) : list")]
        public HassiumList getBytes(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            byte[] bytes = Encoding.GetBytes(args[0].ToString(vm, location).String);
            HassiumList list = new HassiumList(new HassiumObject[0]);

            foreach (byte b in bytes)
                list.add(vm, location, new HassiumChar((char)b));

            return list;
        }

        [FunctionAttribute("func getString (bytes : list) : string")]
        public HassiumString getString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var list = args[0].ToList(vm, location).Values;
            byte[] bytes = new byte[list.Count];

            for (int i = 0; i < list.Count; i++)
                bytes[i] = (byte)list[i].ToChar(vm, location).Char;

            return new HassiumString(Encoding.GetString(bytes));
        }

        [FunctionAttribute("headerName { get; }")]
        public HassiumString get_headerName(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Encoding.HeaderName);
        }
    }
}

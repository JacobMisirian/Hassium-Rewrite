using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumArgumentLengthException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("ArgumentLengthException");

        public HassiumInt ExpectedLength { get; private set; }
        public HassiumObject Function { get; private set; }
        public HassiumInt GivenLength { get; private set; }

        public HassiumArgumentLengthException(HassiumObject function, int expectedLength, int givenLength)
        {
            Function = function;
            ExpectedLength = new HassiumInt(expectedLength);
            GivenLength = new HassiumInt(givenLength);
            AddType(TypeDefinition);

            AddAttribute("expectedLength", new HassiumProperty(get_expectedLength));
            AddAttribute("function", new HassiumProperty(get_function));
            AddAttribute("givenLength", new HassiumProperty(get_givenLength));
            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute(TOSTRING, Attributes["message"]);
        }

        public HassiumInt get_expectedLength(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return ExpectedLength;
        }

        public HassiumObject get_function(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Function;
        }

        public HassiumInt get_givenLength(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return GivenLength;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Argument Length Error: Expected {0} arguments, {1} given", ExpectedLength.Int, GivenLength.Int));
        }
    }
}

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

        public HassiumArgumentLengthException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 3);
        }

        public static HassiumArgumentLengthException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumArgumentLengthException exception = new HassiumArgumentLengthException();

            exception.ExpectedLength = args[1].ToInt(vm, location);
            exception.Function = args[0];
            exception.GivenLength = args[2].ToInt(vm, location);
            exception.AddAttribute("expectedLength", new HassiumProperty(exception.get_expectedLength));
            exception.AddAttribute("function", new HassiumProperty(exception.get_function));
            exception.AddAttribute("givenLength", new HassiumProperty(exception.get_givenLength));
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
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
            return new HassiumString(string.Format("Argument Length Error: Expected '{0}' arguments, '{1}' given", ExpectedLength.Int, GivenLength.Int));
        }
    }
}

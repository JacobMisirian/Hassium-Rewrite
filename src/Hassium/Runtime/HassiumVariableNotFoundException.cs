using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumVariableNotFoundException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("VariableNotFoundException");

        public HassiumVariableNotFoundException()
        {
            AddType(TypeDefinition);

        }

        public static HassiumVariableNotFoundException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumVariableNotFoundException exception = new HassiumVariableNotFoundException();

            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Variable Not Found: variable was not found inside the stack frmae"));
        }
    }
}

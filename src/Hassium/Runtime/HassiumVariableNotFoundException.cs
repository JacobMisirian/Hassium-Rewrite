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

            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute(TOSTRING, Attributes["message"]);
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Variable Not Found: variable was not found inside the stack frmae"));
        }
    }
}

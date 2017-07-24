using Hassium.Compiler;
using Hassium.Runtime;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumDirectoryNotFoundException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("DirectoryNotFoundException");

        public HassiumString Path { get; set; }

        public HassiumDirectoryNotFoundException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 1);
        }

        public static HassiumDirectoryNotFoundException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumDirectoryNotFoundException exception = new HassiumDirectoryNotFoundException();

            exception.Path = args[0].ToString(vm, location);
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute("path", new HassiumProperty(exception.get_path));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Directory Not Found: '{0}' does not exist!", Path.String));
        }

        public HassiumString get_path(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Path;
        }
    }
}

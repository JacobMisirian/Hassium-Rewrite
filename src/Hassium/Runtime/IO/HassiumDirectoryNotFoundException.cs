using Hassium.Compiler;
using Hassium.Runtime;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumDirectoryNotFoundException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("DirectoryNotFoundException");

        public HassiumString Path { get; private set; }

        public HassiumDirectoryNotFoundException(HassiumString path)
        {
            Path = path;
            AddType(TypeDefinition);

            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute("path", new HassiumProperty(get_path));
            AddAttribute(TOSTRING, Attributes["message"]);
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Directory Not Found: {0} does not exist!", Path.String));
        }

        public HassiumString get_path(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Path;
        }
    }
}

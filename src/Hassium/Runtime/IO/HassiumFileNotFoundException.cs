using System.IO;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumFileNotFoundException : HassiumObject
    {
        public static void VerifyPath(VirtualMachine vm, HassiumString path)
        {
            VerifyPath(vm, path.String);
        }
        public static void VerifyPath(VirtualMachine vm, string path)
        {
            if (!File.Exists(path))
                vm.RaiseException(new HassiumFileNotFoundException(new HassiumString(path)));
        }

        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("FileNotFoundException");
        
        public HassiumString Path { get; private set; }

        public HassiumFileNotFoundException(HassiumString path)
        {
            Path = path;
            AddType(TypeDefinition);

            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute("path", new HassiumProperty(get_path));
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("File not found: {0} does not exist!", Path.String));
        }

        public HassiumObject get_path(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Path;
        }
    }
}

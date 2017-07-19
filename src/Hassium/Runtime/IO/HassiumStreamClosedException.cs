using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumStreamClosedException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("StreamClosedException");

        public HassiumFile File { get; private set; }
        public HassiumString FilePath { get; private set; }

        public HassiumStreamClosedException(HassiumFile file, HassiumString filePath)
        {
            File = file;
            FilePath = filePath;
            AddType(TypeDefinition);

            AddAttribute("file", new HassiumProperty(get_file));
            AddAttribute("filePath", new HassiumProperty(get_filePath));
        }

        public HassiumFile get_file(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return File;
        }

        public HassiumString get_filePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return FilePath;
        }
    }
}

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumFileClosedException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("StreamClosedException");

        public HassiumFile File { get; set; }
        public HassiumString FilePath { get; set; }

        public HassiumFileClosedException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 2);
        }

        public static HassiumFileClosedException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumFileClosedException exception = new HassiumFileClosedException();

            exception.File = args[0] as HassiumFile;
            exception.FilePath = args[1].ToString(vm, location);
            exception.AddAttribute("file", new HassiumProperty(exception.get_file));
            exception.AddAttribute("filePath", new HassiumProperty(exception.get_filePath));
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumFile get_file(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return File;
        }

        public HassiumString get_filePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return FilePath;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("File Closed: Filepath '{0}' has been closed", FilePath.String));
        }
    }
}

using Hassium.Compiler;
using Hassium.Runtime.Types;

using System;

namespace Hassium.Runtime.IO
{
    public class HassiumFileNotFoundException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("FileNotFoundException");
        
        public HassiumString Path { get; set; }

        public HassiumFileNotFoundException()
        {
            AddType(TypeDefinition);
            
        }

        public static HassiumFileNotFoundException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumFileNotFoundException exception = new HassiumFileNotFoundException();

            exception.Path = args[0].ToString(vm, location);
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute("path", new HassiumProperty(exception.get_path));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("File not found: '{0}' does not exist!", Path.String));
        }

        public HassiumObject get_path(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Path;
        }
    }
}

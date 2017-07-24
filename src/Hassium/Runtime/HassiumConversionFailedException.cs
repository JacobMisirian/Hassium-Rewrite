using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumConversionFailedException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("ConversionFailedException");

        public HassiumTypeDefinition DesiredType { get; set; }
        public HassiumObject Object { get; set; }

        public HassiumConversionFailedException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 2);
        }

        public static HassiumConversionFailedException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumConversionFailedException exception = new HassiumConversionFailedException();

            exception.Object = args[0];
            exception.DesiredType = args[1] as HassiumTypeDefinition;
            exception.AddAttribute("desiredType", new HassiumProperty(exception.get_desiredType));
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute("object", new HassiumProperty(exception.get_object));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumTypeDefinition get_desiredType(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return DesiredType;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Conversion Failed: Could not convert object of type '{0}' to type '{1}'", Object.Type().ToString(vm, location).String, DesiredType.ToString(vm, location).String));
        }

        public HassiumObject get_object(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Object;
        }
    }
}

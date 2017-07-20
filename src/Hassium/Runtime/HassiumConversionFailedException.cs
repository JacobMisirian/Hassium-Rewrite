using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumConversionFailedException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("ConversionFailedException");

        public HassiumTypeDefinition DesiredType { get; private set; }
        public HassiumObject Object { get; private set; }

        public HassiumConversionFailedException(HassiumObject obj, HassiumTypeDefinition desiredType)
        {
            Object = obj;
            DesiredType = desiredType;
            AddType(TypeDefinition);

            AddAttribute("desiredType", new HassiumProperty(get_desiredType));
            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute("object", new HassiumProperty(get_object));
            AddAttribute(TOSTRING, Attributes["message"]);
        }

        public HassiumTypeDefinition get_desiredType(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return DesiredType;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Conversion Failed: Could not convert object of type {0} to type {1}", Object.Type().ToString(vm, location).String, DesiredType.ToString(vm, location).String));
        }

        public HassiumObject get_object(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Object;
        }
    }
}

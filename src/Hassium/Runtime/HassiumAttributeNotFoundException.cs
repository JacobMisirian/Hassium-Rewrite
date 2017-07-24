using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumAttributeNotFoundException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("AttributeNotFoundException");

        public HassiumObject Object { get; private set; }
        public HassiumString Attribute { get; private set; }

        public HassiumAttributeNotFoundException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 2);
        }

        public static HassiumAttributeNotFoundException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumAttributeNotFoundException exception = new HassiumAttributeNotFoundException();

            exception.Object = args[0];
            exception.Attribute = args[1].ToString(vm, location);
            exception.AddAttribute("attribute", new HassiumProperty(exception.get_attribute));
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute("object", new HassiumProperty(exception.get_object));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumObject get_attribute(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Attribute;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Attribute Not Found: Could not find attribute '{0}' in object of type '{1}'", Attribute.String, Object.Type().ToString(vm, location).String));
        }

        public HassiumObject get_object(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Object;
        }
    }
}

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumAttributeNotFoundException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("AttributeNotFoundException");

        public HassiumObject Object { get; private set; }
        public HassiumString Attribute { get; private set; }

        public string DummyAttribute { get; private set; }

        public HassiumAttributeNotFoundException(HassiumObject obj, string attribute)
        {
            Object = obj;
            Attribute = new HassiumString(attribute);
            DummyAttribute = attribute;
            AddType(TypeDefinition);

            AddAttribute("attribute", new HassiumProperty(get_attribute));
            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute("object", new HassiumProperty(get_object));
            AddAttribute(TOSTRING, Attributes["message"]);
        }

        public HassiumObject get_attribute(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Attribute;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Attribute Not Found: Could not find attribute {0} in object of type {1}", Attribute.String, Object.Type().ToString(vm, location).String));
        }

        public HassiumObject get_object(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Object;
        }
    }
}

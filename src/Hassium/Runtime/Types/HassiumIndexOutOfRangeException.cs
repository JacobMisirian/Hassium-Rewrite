using Hassium.Compiler;

namespace Hassium.Runtime.Types
{
    public class HassiumIndexOutOfRangeException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("IndexOutOfRangeException");

        public HassiumObject Object { get; private set; }
        public HassiumInt RequestedIndex { get; private set; }

        public HassiumIndexOutOfRangeException(HassiumObject obj, HassiumInt requestedIndex)
        {
            Object = obj;
            RequestedIndex = requestedIndex;
            AddType(TypeDefinition);

            AddAttribute("index", new HassiumProperty(get_index));
            AddAttribute("message", new HassiumProperty(get_message));
            AddAttribute("object", new HassiumProperty(get_object));
            AddAttribute(TOSTRING, Attributes["message"]);
        }

        public HassiumInt get_index(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return RequestedIndex;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Out of range: Index {0} is less than 0 or greater than the size of the collection of type {1}", RequestedIndex.Int, Object.Type()));
        }

        public HassiumObject get_object(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Object;
        }
    }
}

using Hassium.Compiler;

namespace Hassium.Runtime.Types
{
    public class HassiumIndexOutOfRangeException : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("IndexOutOfRangeException");

        public HassiumObject Object { get;  set; }
        public HassiumInt RequestedIndex { get; set; }

        public HassiumIndexOutOfRangeException()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 2);
        }

        public static HassiumIndexOutOfRangeException _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumIndexOutOfRangeException exception = new HassiumIndexOutOfRangeException();

            exception.Object = args[0];
            exception.RequestedIndex = args[1].ToInt(vm, location);
            exception.AddAttribute("index", new HassiumProperty(exception.get_index));
            exception.AddAttribute("message", new HassiumProperty(exception.get_message));
            exception.AddAttribute("object", new HassiumProperty(exception.get_object));
            exception.AddAttribute(TOSTRING, exception.Attributes["message"]);

            return exception;
        }

        public HassiumInt get_index(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return RequestedIndex;
        }

        public HassiumString get_message(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(string.Format("Out of range: Index '{0}' is less than 0 or greater than the size of the collection of type '{1}'", RequestedIndex.Int, Object.Type()));
        }

        public HassiumObject get_object(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Object;
        }
    }
}

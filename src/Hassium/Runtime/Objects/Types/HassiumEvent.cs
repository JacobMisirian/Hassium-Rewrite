using System;

namespace Hassium.Runtime.Objects.Types
{
    public class HassiumEvent: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("event");

        public HassiumList Handlers { get; set; }

        public HassiumEvent()
        {
            AddAttribute(HassiumObject.INVOKE, new_, 0, 1);
        }

        public HassiumEvent new_(VirtualMachine vm, params HassiumObject[] args)
        {
            var hassiumEvent = new HassiumEvent();

            hassiumEvent.AddType(TypeDefinition);
            hassiumEvent.Handlers = args.Length == 0 ? new HassiumList(new HassiumObject[0]) : args[0] as HassiumList;
            hassiumEvent.AddAttribute("add",    hassiumEvent.add);
            hassiumEvent.AddAttribute("fire",   hassiumEvent.fire);
            hassiumEvent.AddAttribute("remove", hassiumEvent.remove);

            return hassiumEvent;
        }
        public HassiumObject add(VirtualMachine vm, params HassiumObject[] args)
        {
            foreach (var obj in args)
                Handlers.add(vm, obj);
            return args[0];
        }
        public HassiumList fire(VirtualMachine vm, params HassiumObject[] args)
        {
            HassiumList result = new HassiumList(new HassiumObject[0]);
            foreach (var obj in Handlers.List)
                result.add(vm, obj.Invoke(vm, args));
            return result;
        }
        public HassiumNull remove(VirtualMachine vm, params HassiumObject[] args)
        {
            foreach (var obj in args)
                Handlers.remove(vm, obj);
            return HassiumObject.Null;
        }
    }
}


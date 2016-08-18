using System;
using System.Collections.Generic;

using Hassium.Runtime.Objects;
using Hassium.Runtime.Objects.Types;

namespace Hassium.Runtime
{
    public class GlobalFunctions
    {
        public static Dictionary<string, HassiumObject> Functions = new Dictionary<string, HassiumObject>()
        {
            { "format", new HassiumFunction(format, -1) },
            { "print", new HassiumFunction(print, -1) },
            { "println", new HassiumFunction(println, -1) },
            { "type", new HassiumFunction(type, 1) }
        };

        public static HassiumString format(VirtualMachine vm, params HassiumObject[] args)
        {
            string[] elements = new string[args.Length - 1];
            for (int i = 1; i < elements.Length; i++)
                elements[i - 1] = args[i].ToString(vm).String;
            return new HassiumString(string.Format(args[0].ToString(vm).String, elements));
        }
        public static HassiumObject print(VirtualMachine vm, params HassiumObject[] args)
        {
            foreach (var arg in args)
                Console.Write(arg.ToString(vm).String);
            return HassiumObject.Null;
        }
        public static HassiumObject println(VirtualMachine vm, params HassiumObject[] args)
        {
            foreach (var arg in args)
                Console.WriteLine(arg.ToString(vm).String);
            return HassiumObject.Null;
        }
        public static HassiumTypeDefinition type(VirtualMachine vm, params HassiumObject[] args)
        {
            return args[0].Type();
        }
    }
}


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
            { "print", new HassiumFunction(print, -1) },
            { "println", new HassiumFunction(println, -1) }
        };

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
    }
}


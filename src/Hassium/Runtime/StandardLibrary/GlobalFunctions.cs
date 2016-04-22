using System;
using System.Collections.Generic;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary
{
    public static class GlobalFunctions
    {
        public static Dictionary<string, HassiumFunction> FunctionList = new Dictionary<string, HassiumFunction>()
        {
            { "input", new HassiumFunction(input, 0) },
            { "inputChar", new HassiumFunction(inputChar, 0) },
            { "print", new HassiumFunction(print, -1) },
            { "println", new HassiumFunction(println, -1) },
            { "type", new HassiumFunction(type, 1) },
            { "types", new HassiumFunction(types, -1) }
        };
        private static HassiumString input(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(Console.ReadLine());
        }
        private static HassiumChar inputChar(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumChar(Convert.ToChar(Console.Read()));
        }
        private static HassiumObject print(VirtualMachine vm, HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Console.Write(obj.ToString());
            return HassiumObject.Null;
        }
        private static HassiumObject println(VirtualMachine vm, HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Console.WriteLine(obj.ToString());
            return HassiumObject.Null;
        }
        private static HassiumObject type(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(args[0].Types[args[0].Types.Count - 1]);
        }
        private static HassiumObject types(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumObject[] elements = new HassiumObject[args[0].Types.Count];
            for (int i = 0; i < elements.Length; i++)
                elements[i] = new HassiumString(args[0].Types[i]);
            return new HassiumList(elements);
        }
    }
}
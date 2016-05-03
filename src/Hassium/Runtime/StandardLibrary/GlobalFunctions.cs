using System;
using System.Collections.Generic;
using System.Linq;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary
{
    public static class GlobalFunctions
    {
        public static Dictionary<string, HassiumFunction> FunctionList = new Dictionary<string, HassiumFunction>()
        {
            { "format",     new HassiumFunction(format, -1) },
            { "input",      new HassiumFunction(input, 0) },
            { "inputChar",  new HassiumFunction(inputChar, 0) },
            { "print",      new HassiumFunction(print, -1) },
            { "println",    new HassiumFunction(println, -1) },
            { "range",      new HassiumFunction(range, 2) },
            { "type",       new HassiumFunction(type, 1) },
            { "types",      new HassiumFunction(types, -1) }
        };
        private static HassiumString format(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumObject[] objs = new HassiumObject[args.Length - 1];
            for (int i = 1; i < args.Length; i++)
                objs[i - 1] = args[i];
            return new HassiumString(string.Format(HassiumString.Create(args[0]).ToString(), objs));
        }
        private static HassiumString input(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(Console.ReadLine());
        }
        private static HassiumChar inputChar(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumChar(Convert.ToChar(Console.Read()));
        }
        private static HassiumList map(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumList list = args[0] as HassiumList;
            HassiumList result = new HassiumList(new HassiumObject[0]);
            list.EnumerableReset(vm);
            while (!HassiumBool.Create(list.EnumerableFull(vm)).Value)
                result.Value.Add(args[1].Invoke(vm, new HassiumObject[] { list.EnumerableNext(vm) }));
            list.EnumerableReset(vm);

            return result;
        }
        private static HassiumObject print(VirtualMachine vm, HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Console.Write(obj.ToString(vm));
            return HassiumObject.Null;
        }
        private static HassiumObject println(VirtualMachine vm, HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Console.WriteLine(obj.ToString(vm));
            return HassiumObject.Null;
        }
        private static HassiumObject range(VirtualMachine vm, HassiumObject[] args)
        {
            int max = (int)HassiumInt.Create(args[1]).Value;
            HassiumList list = new HassiumList(new HassiumObject[0]);
            for (int i = (int)HassiumInt.Create(args[0]).Value; i < max; i++)
                list.Value.Add(new HassiumInt(i));

            return list;
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
using System;
using System.Collections.Generic;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class GlobalFunctions
    {
        public static Dictionary<string, HassiumObject> Functions = new Dictionary<string, HassiumObject>()
        {
            { "format",        new HassiumFunction(format,         -1) },
            { "input",         new HassiumFunction(input,           0) },
            { "print",         new HassiumFunction(print,          -1) },
            { "println",       new HassiumFunction(println,        -1) },
            { "sleep",         new HassiumFunction(sleep,           1) },
            { "type",          new HassiumFunction(type,            1) },
            { "types",         new HassiumFunction(types,           1) }
        };

        [FunctionAttribute("func format (fmt : string, params obj) : string")]
        public static HassiumString format(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (args.Length <= 0)
                return new HassiumString(string.Empty);
            if (args.Length == 1)
                return args[0].ToString(vm, location);

            string[] fargs = new string[args.Length];
            for (int i = 1; i < args.Length; i++)
                fargs[i] = args[i].ToString(vm, location).String;
            return new HassiumString(string.Format(args[0].ToString(vm, location).String, fargs));
        }

        [FunctionAttribute("func input () : string")]
        public static HassiumString input(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Console.ReadLine());
        }

        [FunctionAttribute("func print (params obj) : null")]
        public static HassiumNull print(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            foreach (var arg in args)
                Console.Write(arg.ToString(vm, location).String);
            return HassiumObject.Null;
        }

        [FunctionAttribute("func println (params obj) : null")]
        public static HassiumNull println(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            foreach (var arg in args)
                Console.WriteLine(arg.ToString(vm, location).String);
            return HassiumObject.Null;
        }

        [FunctionAttribute("func sleep (milliseconds : int) : null")]
        public static HassiumNull sleep(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            System.Threading.Thread.Sleep((int)args[0].ToInt(vm, location).Int);
            return HassiumObject.Null;
        }

        [FunctionAttribute("func type (obj : object) : typedef")]
        public static HassiumTypeDefinition type(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return args[0].Type();
        }

        [FunctionAttribute("func types (obj : object) : list")]
        public static HassiumList types(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumList(args[0].Types);
        }
    }
}

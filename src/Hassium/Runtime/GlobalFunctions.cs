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
            { "println",       new HassiumFunction(println,        -1) },
            { "type",          new HassiumFunction(type,            1) },
            { "types",         new HassiumFunction(types,           1) }
        };

        public static HassiumNull println(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            foreach (var arg in args)
                Console.WriteLine(arg.ToString(vm, location).String);
            return HassiumObject.Null;
        }

        public static HassiumTypeDefinition type(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return args[0].Type();
        }

        public static HassiumList types(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumList(args[0].Types);
        }
    }
}

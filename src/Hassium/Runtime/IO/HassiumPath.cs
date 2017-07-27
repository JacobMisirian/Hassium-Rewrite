﻿using Hassium.Compiler;
using Hassium.Runtime;
using Hassium.Runtime.Types;

using System.IO;

namespace Hassium.Runtime.IO
{
    public class HassiumPath : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("Path");

        public HassiumPath()
        {
            AddType(TypeDefinition);

            AddAttribute("combinePath", combinePath, -1);
            AddAttribute("parseDirectoryName", parseDirectoryName, 1);
            AddAttribute("parseExtension", parseExtension, 1);
            AddAttribute("parseFileName", parseFileName, 1);
            AddAttribute("parseFileNameWithoutExtension", parseFileNameWithoutExtension, 1);
            AddAttribute("parseRoot", parseRoot, 1);
        }

        [FunctionAttribute("func combinePath (params paths) : string")]
        public HassiumString combinePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            string[] paths = new string[args.Length];
            for (int i = 0; i < paths.Length; i++)
                paths[i] = args[i].ToString(vm, location).String;
            return new HassiumString(Path.Combine(paths));
        }

        [FunctionAttribute("func parseDirectoryName (path : string) : string")]
        public HassiumString parseDirectoryName(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetDirectoryName(args[0].ToString(vm, location).String));
        }

        [FunctionAttribute("func parseExtension (path : string) : string")]
        public HassiumString parseExtension(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetExtension(args[0].ToString(vm, location).String));
        }

        [FunctionAttribute("func parseFileName (path : string) : string")]
        public HassiumString parseFileName(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetFileName(args[0].ToString(vm, location).String));
        }

        [FunctionAttribute("func parseFileNameWithoutExtension (path : string) : string")]
        public HassiumString parseFileNameWithoutExtension(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetFileNameWithoutExtension(args[0].ToString(vm, location).String));
        }

        [FunctionAttribute("func parseRoot (path : string) : string")]
        public HassiumString parseRoot(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetPathRoot(args[0].ToString(vm, location).String));
        }
    }
}
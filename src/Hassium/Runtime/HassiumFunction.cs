﻿using Hassium.Compiler;
using Hassium.Runtime.Exceptions;

namespace Hassium.Runtime
{
    public delegate HassiumObject HassiumFunctionDelegate(VirtualMachine vm, SourceLocation location, params HassiumObject[] args);
    public class HassiumFunction : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = HassiumMethod.TypeDefinition;
        public HassiumFunctionDelegate Target { get; private set; }
        public int[] ParameterLengths { get; private set; }

        public HassiumFunction(HassiumFunctionDelegate target, int paramLength)
        {
            AddType(TypeDefinition);
            Target = target;
            ParameterLengths = new int[] { paramLength };
        }
        public HassiumFunction(HassiumFunctionDelegate target, int[] paramLengths)
        {
            AddType(TypeDefinition);
            Target = target;
            ParameterLengths = paramLengths;
        }

        public override HassiumObject Invoke(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (ParameterLengths[0] != -1)
            {
                foreach (int len in ParameterLengths)
                    if (len == args.Length)
                        return Target(vm, location, args);
                throw new InternalException(vm, location, "Expected argument length of {0}, got {1} in {2}!", ParameterLengths[0], args.Length, Target.Method.Name);
            }
            return Target(vm, location, args);
        }
    }
}

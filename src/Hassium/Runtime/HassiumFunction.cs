using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime
{
    public delegate HassiumObject HassiumFunctionDelegate(VirtualMachine vm, params HassiumObject[] args);
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

    }
}

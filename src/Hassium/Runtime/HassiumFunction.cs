using Hassium.Compiler;
using Hassium.Runtime.Types;

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
            var v = Target.Method.GetCustomAttributes(typeof(FunctionAttribute), false);
            if (v.Length > 0)
                vm.PushCallStack(string.Format("{0}\t{1}", (v[0] as FunctionAttribute).SourceRepresentation, location));
            if (ParameterLengths[0] != -1)
            {
                foreach (int len in ParameterLengths)
                    if (len == args.Length)
                        return Target(vm, location, args);
                vm.RaiseException(HassiumArgumentLengthException._new(vm, location, this, new HassiumInt(ParameterLengths[0]), new HassiumInt(args.Length)));
                return Null;
            }
            return Target(vm, location, args);
        }
    }
}

using System;

using Hassium.Runtime;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public delegate HassiumObject HassiumFunctionDelegate(params HassiumObject[] args);
    public class HassiumFunction: HassiumObject
    {
        private HassiumFunctionDelegate target;
        public int ParamLength { get; private set; }

        public HassiumFunction(HassiumFunctionDelegate target, int paramLength)
        {
            this.target = target;
            ParamLength = paramLength;
        }

        public override HassiumObject Invoke(VirtualMachine vm, HassiumObject[] args)
        {
            if (args.Length != ParamLength && ParamLength != -1)
                throw new Exception(string.Format("Expected argument length of {0} got {1}!", ParamLength, args.Length));
            return target(args);
        }
    }
}


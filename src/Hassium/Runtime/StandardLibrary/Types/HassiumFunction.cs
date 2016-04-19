using System;

using Hassium.Runtime;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public delegate HassiumObject HassiumFunctionDelegate(params HassiumObject[] args);
    public class HassiumFunction: HassiumObject
    {
        private HassiumFunctionDelegate target;
        public int[] ParamLengths { get; private set; }

        public HassiumFunction(HassiumFunctionDelegate target, int paramLength)
        {
            this.target = target;
            ParamLengths = new int[] { paramLength };
            Types.Add(this.GetType().Name);
        }
        public HassiumFunction(HassiumFunctionDelegate target, int[] paramLengths)
        {
            this.target = target;
            ParamLengths = paramLengths;
            Types.Add(this.GetType().Name);
        }

        public override HassiumObject Invoke(VirtualMachine vm, HassiumObject[] args)
        {
            if (ParamLengths[0] != -1)
            {
                foreach (int i in ParamLengths)
                    if (i == args.Length)
                        return target(args);
                throw new Exception(string.Format("Expected argument length of {0}, got {1}", ParamLengths[0], args.Length));
            }
            else
                return target(args);
        }
    }
}


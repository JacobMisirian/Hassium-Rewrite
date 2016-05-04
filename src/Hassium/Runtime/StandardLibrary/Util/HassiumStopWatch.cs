using System;
using System.Diagnostics;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.Util
{
    public class HassiumStopWatch: HassiumObject
    {
        public new Stopwatch Value { get; private set; }
        public HassiumStopWatch()
        {

        }

        private HassiumStopWatch _new(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumStopWatch hassiumStopWatch = new HassiumStopWatch();

            hassiumStopWatch.Value = new Stopwatch();

            return hassiumStopWatch;
        }

        public HassiumInt get_Milliseconds(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumInt(Value.ElapsedMilliseconds);
        }
        public HassiumInt get_Minutes(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumInt(Value.Elapsed.Minutes);
        }
        public HassiumInt get_Seconds(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumInt(Value.Elapsed.Seconds);
        }
        public HassiumInt get_Ticks(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumInt(Value.ElapsedTicks);
        }
    }
}
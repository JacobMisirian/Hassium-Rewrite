using System;

namespace Hassium.Runtime.Objects.Util
{
    public class HassiumUtilModule: InternalModule
    {
        public HassiumUtilModule() : base("Util")
        {
            AddAttribute("Process",         new HassiumProcess());
            AddAttribute("ProcessContext",  new HassiumProcessContext());
            AddAttribute("StopWatch",       new HassiumStopWatch());
        }
    }
}


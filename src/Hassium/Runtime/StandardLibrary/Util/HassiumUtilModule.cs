using System;

namespace Hassium.Runtime.StandardLibrary.Util
{
    public class HassiumUtilModule : InternalModule
    {
        public HassiumUtilModule() : base ("Util")
        {
            Attributes.Add("StopWatch", new HassiumStopWatch());
        }
    }
}


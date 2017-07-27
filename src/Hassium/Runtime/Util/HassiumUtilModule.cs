namespace Hassium.Runtime.Util
{
    public class HassiumUtilModule : InternalModule
    {
        public HassiumUtilModule() : base("Util")
        {
            AddAttribute("StopWatch", new HassiumStopWatch());
        }
    }
}

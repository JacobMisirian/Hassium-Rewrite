namespace Hassium.Runtime.Util
{
    public class HassiumUtilModule : InternalModule
    {
        public HassiumUtilModule() : base("Util")
        {
            AddAttribute("DateTime", new HassiumDateTime());
            AddAttribute("OS", new HassiumOS());
            AddAttribute("Process", HassiumProcess.TypeDefinition);
            AddAttribute("StopWatch", new HassiumStopWatch());
        }
    }
}

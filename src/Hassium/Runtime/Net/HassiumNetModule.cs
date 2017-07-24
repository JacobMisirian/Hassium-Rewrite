namespace Hassium.Runtime.Net
{
    public class HassiumNetModule : InternalModule
    {
        public HassiumNetModule() : base("Net")
        {
            AddAttribute("IPAddr", new HassiumIPAddr());
            AddAttribute("Socket", new HassiumSocket());
            AddAttribute("SocketClosedException", new HassiumSocketClosedException());
        }
    }
}

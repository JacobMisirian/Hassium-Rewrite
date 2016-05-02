using System;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.Net
{
    public class HassiumNetModule: InternalModule
    {
        public HassiumNetModule() : base("Net")
        {
            Attributes.Add("ConnectionListener", new HassiumConnectionListener());
            Attributes.Add("Dns", new HassiumDns());
            Attributes.Add("NetConnection", new HassiumNetConnection());
        }
    }
}


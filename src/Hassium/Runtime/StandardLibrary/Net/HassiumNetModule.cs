using System;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.Net
{
    public class HassiumNetModule: InternalModule
    {
        public HassiumNetModule() : base("Net")
        {
            Attributes.Add("NetConnection", new HassiumNetConnection());
            Attributes.Add("Dns", new HassiumDns());
        }
    }
}


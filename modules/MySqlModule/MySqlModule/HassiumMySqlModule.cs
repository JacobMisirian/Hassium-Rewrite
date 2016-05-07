using System;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary;

namespace MySqlModule
{
    public class HassiumMySqlModule : InternalModule
    {
        public HassiumMySqlModule() : base("MySql")
        {
            Attributes.Add("MySql", new HassiumMySql());
        }
    }
}

using System;

using Hassium.Runtime.StandardLibrary;
using Hassium.Runtime.StandardLibrary.Types;

namespace TestModule
{
    public class HassiumTestModule : InternalModule
    {
        public HassiumTestModule() : base("TestModule")
        {
            Attributes.Add("TestClass", new HassiumTestClass());
        }
    }
}

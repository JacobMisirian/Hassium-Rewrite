using System;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary.Types;

namespace TestModule
{
    public class HassiumTestClass: HassiumObject
    {
        public HassiumTestClass()
        {
            Attributes.Add("testFunction", new HassiumFunction(testFunction, 3));
        }

        private HassiumInt testFunction(VirtualMachine vm, HassiumObject[] args)
        {
            Console.WriteLine(args[0].ToString(vm));
            return new HassiumInt(HassiumInt.Create(args[1]).Value + HassiumInt.Create(args[2]).Value);
        }
    }
}


using System;
using System.Collections.Generic;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary
{
    public static class GlobalFunctions
    {
        public static Dictionary<string, HassiumFunction> FunctionList = new Dictionary<string, HassiumFunction>()
        {
            { "print", new HassiumFunction(print, -1) },
            { "println", new HassiumFunction(println, -1) }
        };
        private static HassiumObject print(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Console.Write(obj.ToString());
            return null;
        }
        private static HassiumObject println(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Console.WriteLine(obj.ToString());
            return null;
        }
    }
}
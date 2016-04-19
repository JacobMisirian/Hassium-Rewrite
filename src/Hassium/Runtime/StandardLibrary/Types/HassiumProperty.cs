using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumProperty: HassiumObject
    {
        public HassiumFunctionDelegate GetValue;
        public HassiumFunctionDelegate SetValue;

        public HassiumProperty(HassiumFunctionDelegate getValue, HassiumFunctionDelegate setValue = null)
        {
            GetValue = getValue;
            SetValue = setValue;
        }

        public HassiumObject Invoke(HassiumObject[] args)
        {
            return GetValue.Invoke(args);
        }
        public HassiumObject Set(HassiumObject[] args)
        {
            return SetValue.Invoke(args);
        }
    }
}


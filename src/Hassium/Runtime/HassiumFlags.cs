using System;

namespace Hassium
{
    public class HassiumFlags
    {
        public bool Equal { get; private set; }
        public bool Greater { get; private set; }
        public void ProcessFlags(double value)
        {
            Equal = false;
            Greater = false;

            if (value > 0)
                Greater = true;
            if (value == 0)
                Equal = true;
        }
    }
}


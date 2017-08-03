using Hassium.Compiler;
using Hassium.Runtime.Types;

using System;
using System.Diagnostics;

namespace Hassium.Runtime.Util
{
    public class HassiumProcess : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("Process");

        public Process Process { get; set; }
        public ProcessStartInfo StartInfo { get; set; }

        public HassiumProcess(Process process)
        {
            AddType(TypeDefinition);
            Process = process;
            StartInfo = process.StartInfo;

        }
        public HassiumProcess(Process process, ProcessStartInfo startInfo)
        {
            AddType(TypeDefinition);
            Process = process;
            StartInfo = startInfo;
            
        }


    }
}

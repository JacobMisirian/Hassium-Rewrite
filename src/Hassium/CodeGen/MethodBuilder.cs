using System;
using System.Collections.Generic;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.CodeGen
{
    public class MethodBuilder: HassiumObject
    {
        public bool IsConstructor { get { return Name == "new"; } }
        public string Name { get; set; }
        public Dictionary<string, int> Parameters = new Dictionary<string, int>();
        public List<Instruction> Instructions = new List<Instruction>();
        public Dictionary<double, int> Labels = new Dictionary<double, int>();

        public override HassiumObject Invoke(VirtualMachine vm, HassiumObject[] args)
        {
            vm.StackFrame.EnterFrame();
            foreach (int param in Parameters.Values)
                vm.StackFrame.Add(param, args[param]);
            vm.ExecuteMethod(this);
            vm.StackFrame.PopFrame();
            return null;
        }

        public void Emit(InstructionType instructionType, double value = 0)
        {
            Instructions.Add(new Instruction(instructionType, value));
        }
    }
}
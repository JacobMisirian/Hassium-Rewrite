using System;
using System.Collections.Generic;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.CodeGen
{
    public class MethodBuilder : HassiumFunction
    {
        public string Name { get; set; }
        public List<Instruction> Instructions = new List<Instruction>();

        public MethodBuilder(HassiumFunctionDelegate target, int paramLength) : base (target, paramLength)
        {
        }

        public override HassiumObject Invoke(VirtualMachine vm, HassiumObject[] args)
        {
            vm.ExecuteMethod(this);
            return null;
        }

        public void Emit(InstructionType instructionType, double value = 0)
        {
            Instructions.Add(new Instruction(instructionType, value));
        }
    }
}
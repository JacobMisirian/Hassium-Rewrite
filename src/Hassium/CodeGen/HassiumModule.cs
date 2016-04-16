using System;
using System.Collections.Generic;

namespace Hassium.CodeGen
{
    public class HassiumModule
    {
        public string Name { get; private set; }
        public List<Instruction> Instructions { get; private set; }
        public List<string> ConstantPool { get; private set; }

        public HassiumModule(string name)
        {
            Name = name;
            Instructions = new List<Instruction>();
            ConstantPool = new List<string>();
        }

        public void Emit(InstructionType instructionType, double value = 0)
        {
            Instructions.Add(new Instruction(instructionType, value));
        }
    }
}


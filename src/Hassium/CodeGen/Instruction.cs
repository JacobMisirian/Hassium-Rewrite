using System;

namespace Hassium.CodeGen
{
    public class Instruction
    {
        public InstructionType InstructionType { get; private set; }
        public double Argument { get; private set; }

        public Instruction(InstructionType instructionType, double argument)
        {
            InstructionType = instructionType;
            Argument = argument;
        }
    }

    public enum InstructionType
    {
        Add,
        Sub,
        Mul,
        Div,
        Mod,
        Compare,
        Load_Attribute,
        Load_Global,
        Load_Local,
        Store_Local,
        Store_Global,
        Push,
        Push_String,
        Pop,
        Call,
        Label,
        Jump,
        JumpIfEqual,
        JumpIfNotEqual,
        JumpIfGreater,
        JumpIfLesser,
        JumpIfGreaterOrEqual,
        JumpIfLesserOrEqual,
        Push_Frame,
        Pop_Frame
    }
}


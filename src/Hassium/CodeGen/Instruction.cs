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
        Binary_Operation,
        Greater_Than,
        Lesser_Than,
        Greater_Than_Or_Equal,
        Lesser_Than_Or_Equal,
        Equal,
        Not_Equal,
        Load_Attribute,
        Load_Global,
        Load_Local,
        Store_Local,
        Store_Global,
        Store_Attribute,
        Push,
        Push_String,
        Push_Char,
        Push_Bool,
        Create_List,
        Push_List,
        Load_List_Element,
        Store_List_Element,
        Pop,
        Self_Reference,
        Construct,
        Call,
        Label,
        Jump,
        Jump_If_True,
        Jump_If_False,
        Push_Frame,
        Pop_Frame,
        Return
    }
}


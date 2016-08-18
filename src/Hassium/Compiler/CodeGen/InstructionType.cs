using System;

namespace Hassium.Compiler.CodeGen
{
    public enum InstructionType
    {
        Call,
        BinaryOperation,
        UnaryOperation,
        PushConstant,
        PushObject,
        Raise,
        Return,
        LoadLocal,
        StoreLocal,
        LoadAttribute,
        StoreAttribute,
        LoadGlobal,
        LoadGlobalVariable,
        StoreGlobalVariable,
        BuildList,
        LoadListElement,
        StoreListElement,
        Push,
        Pop,
        Duplicate,
        Jump,
        JumpIfTrue,
        JumpIfFalse,
        Label,
        Iter,
        IterableFull,
        IterableNext
    }
}


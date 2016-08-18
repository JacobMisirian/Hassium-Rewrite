using System;

namespace Hassium.Compiler.CodeGen
{
    public enum InstructionType
    {
        BinaryOperation,
        BuildList,
        Call,
        Duplicate,
        Iter,
        IterableFull,
        IterableNext,
        Jump,
        JumpIfFalse,
        JumpIfTrue,
        Label,
        LoadAttribute,
        LoadGlobal,
        LoadGlobalVariable,
        LoadListElement,
        LoadLocal,
        Pop,
        Push,
        PushConstant,
        PushObject,
        Raise,
        Return,
        SelfReference,
        StoreAttribute,
        StoreGlobalVariable,
        StoreListElement,
        StoreLocal,
        UnaryOperation
    }
}


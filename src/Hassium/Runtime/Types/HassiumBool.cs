﻿using Hassium.Compiler;

namespace Hassium.Runtime.Types
{
    public class HassiumBool : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("bool");

        public bool Bool { get; private set; }

        public HassiumBool(bool val)
        {
            AddType(TypeDefinition);
            Bool = val;

            AddAttribute(EQUALTO, EqualTo, 1);
            AddAttribute(LOGICALAND, LogicalAnd, 1);
            AddAttribute(LOGICALNOT, LogicalNot, 0);
            AddAttribute(LOGICALOR, LogicalOr, 1);
            AddAttribute(NOTEQUALTO, NotEqualTo, 1);
            AddAttribute(TOBOOL, ToBool, 0);
            AddAttribute(TOINT, ToInt, 0);
            AddAttribute(TOSTRING, ToString, 0);
        }

        public override HassiumBool EqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Bool == args[0].ToBool(vm, location).Bool);
        }

        public override HassiumObject LogicalAnd(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Bool && args[0].ToBool(vm, location).Bool);
        }

        public override HassiumObject LogicalNot(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(!Bool);
        }

        public override HassiumObject LogicalOr(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Bool || args[0].ToBool(vm, location).Bool);
        }

        public override HassiumBool NotEqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Bool != args[0].ToBool(vm, location).Bool);
        }

        public override HassiumBool ToBool(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }

        public override HassiumInt ToInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Bool ? 1 : 0);
        }

        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Bool.ToString().ToLower());
        }
    }
}

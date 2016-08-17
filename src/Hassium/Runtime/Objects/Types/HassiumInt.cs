﻿using System;

namespace Hassium.Runtime.Objects.Types
{
    public class HassiumInt: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("int");

        public long Int { get; private set; }

        public HassiumInt(long val)
        {
            Int = val;
            AddType(TypeDefinition);
        }

        public override HassiumObject Add(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Int + args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject BitshiftLeft(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt((int)Int << (int)args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject BitshiftRight(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt((int)Int >> (int)args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject BitwiseAnd(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Int & args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject BitwiseNot(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(~Int);
        }
        public override HassiumObject BitwiseOr(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Int | args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject Divide(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Int / args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject EqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Int == args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject GreaterThan(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Int > args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject GreaterThanOrEqual(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Int >= args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject IntegerDivision(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Int / args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject LesserThan(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Int < args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject LesserThanOrEqual(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Int <= args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject Modulus(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Int % args[0].ToInt(vm, args).Int);
        }
        public override HassiumObject Multiply(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Int * args[0].ToFloat(vm, args).Float);
        }
        public override HassiumObject Negate(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(-Int);
        }
        public override HassiumObject NotEqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            return EqualTo(vm, args).LogicalNot(vm, args);
        }
        public override HassiumObject Power(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Math.Pow((double)Int, args[0].ToFloat(vm, args).Float));
        }
        public override HassiumObject Subtract(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Int - args[0].ToInt(vm, args).Int);
        }
        public override HassiumBool ToBool(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Int == 1 ? true : false);
        }
        public override HassiumChar ToChar(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumChar((char)Int);
        }
        public override HassiumFloat ToFloat(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Convert.ToDouble(Int));
        }
        public override HassiumInt ToInt(VirtualMachine vm, params HassiumObject[] args)
        {
            return this;
        }
        public override HassiumString ToString(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumString(Int.ToString());
        }
    }
}

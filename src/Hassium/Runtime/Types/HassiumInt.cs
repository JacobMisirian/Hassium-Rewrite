using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;
using Hassium.Runtime.Exceptions;

namespace Hassium.Runtime.Types
{
    public class HassiumInt : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("int");

        public long Int { get; private set; }

        public HassiumInt(long val)
        {
            AddType(Number);
            AddType(TypeDefinition);
            Int = val;

            AddAttribute("getBit", getBit, 1);
            AddAttribute("setBit", setBit, 2);
            AddAttribute(TOFLOAT, ToFloat);
            AddAttribute(TOINT, ToInt);
            AddAttribute(TOSTRING, ToString);
        }

        public override HassiumObject Add(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var intArg = args[0] as HassiumInt;
            if (intArg != null)
                return new HassiumInt(Int + args[0].ToInt(vm, location).Int);
            var floatArg = args[0] as HassiumFloat;
            if (floatArg != null)
                return new HassiumFloat(Int + args[0].ToFloat(vm, location).Float);
            throw new InternalException(vm, location, InternalException.CONVERSION_ERROR, args[0].Type(), Number);
        }

        public override HassiumObject BitshiftLeft(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt((int)Int << (int)args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject BitshiftRight(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt((int)Int >> (int)args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject Divide(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var intArg = args[0] as HassiumInt;
            if (intArg != null)
                return new HassiumInt(Int / args[0].ToInt(vm, location).Int);
            var floatArg = args[0] as HassiumFloat;
            if (floatArg != null)
                return new HassiumFloat(Int / args[0].ToFloat(vm, location).Float);
            throw new InternalException(vm, location, InternalException.CONVERSION_ERROR, args[0].Type(), Number);
        }

        public override HassiumBool EqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int == args[0].ToInt(vm, location).Int);
        }

        public HassiumBool getBit(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((Int & (1 << (int)args[0].ToInt(vm, location).Int - 1)) != 0);
        }

        public override HassiumBool GreaterThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int > args[0].ToInt(vm, location).Int);
        }

        public override HassiumBool GreaterThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int >= args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject IntegerDivision(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Int / args[0].ToInt(vm, location).Int);
        }

        public override HassiumBool LesserThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int < args[0].ToInt(vm, location).Int);
        }

        public override HassiumBool LesserThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int <= args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject Modulus(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Int % args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject Multiply(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var intArg = args[0] as HassiumInt;
            if (intArg != null)
                return new HassiumInt(Int * args[0].ToInt(vm, location).Int);
            var floatArg = args[0] as HassiumFloat;
            if (floatArg != null)
                return new HassiumFloat(Int * args[0].ToFloat(vm, location).Float);
            throw new InternalException(vm, location, InternalException.CONVERSION_ERROR, args[0].Type(), Number);
        }

        public override HassiumObject Negate(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(-Int);
        }

        public override HassiumBool NotEqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int != args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject Power(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt((long)Math.Pow((double)Int, (double)args[0].ToInt(vm, location).Int));
        }

        public HassiumInt setBit(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            int index = (int)args[0].ToInt(vm, location).Int;
            bool val = args[1].ToBool(vm, location).Bool;
            if (val)
                return new HassiumInt((int)Int | 1 << index);
            else
                return new HassiumInt(Int & ~(1 << index));
        }

        public override HassiumObject Subtract(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var intArg = args[0] as HassiumInt;
            if (intArg != null)
                return new HassiumInt(Int - args[0].ToInt(vm, location).Int);
            var floatArg = args[0] as HassiumFloat;
            if (floatArg != null)
                return new HassiumFloat(Int - args[0].ToFloat(vm, location).Float);
            throw new InternalException(vm, location, InternalException.CONVERSION_ERROR, args[0].Type(), Number);
        }

        public override HassiumBool ToBool(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Int == 1);
        }

        public override HassiumFloat ToFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat((double)Int);
        }

        public override HassiumInt ToInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }

        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Int.ToString());
        }

        public override HassiumObject Xor(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Int ^ args[0].ToInt(vm, location).Int);
        }
    }
}

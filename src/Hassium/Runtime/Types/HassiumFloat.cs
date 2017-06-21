using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;
using Hassium.Runtime.Exceptions;

namespace Hassium.Runtime.Types
{
    public class HassiumFloat : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("float");

        public double Float { get; private set; }

        public HassiumFloat(double val)
        {
            AddType(Number);
            AddType(TypeDefinition);
            Float = val;

            AddAttribute(TOFLOAT, ToFloat, 0);
            AddAttribute(TOINT, ToInt, 0);
            AddAttribute(TOSTRING, ToString, 0);
        }

        public override HassiumObject Add(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat(Float + args[0].ToFloat(vm, location).Float);
        }

        public override HassiumObject Divide(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat(Float / args[0].ToFloat(vm, location).Float);
        }

        public override HassiumBool EqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Float == args[0].ToFloat(vm, location).Float);
        }

        public override HassiumBool GreaterThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Float > args[0].ToFloat(vm, location).Float);
        }

        public override HassiumBool GreaterThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Float >= args[0].ToFloat(vm, location).Float);
        }

        public override HassiumObject IntegerDivision(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt((long)Float / args[0].ToInt(vm, location).Int);
        }

        public override HassiumBool LesserThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Float < args[0].ToFloat(vm, location).Float);
        }

        public override HassiumBool LesserThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Float <= args[0].ToFloat(vm, location).Float);
        }

        public override HassiumObject Multiply(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat(Float * args[0].ToFloat(vm, location).Float);
        }

        public override HassiumObject Negate(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat(-Float);
        }

        public override HassiumBool NotEqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Float != args[0].ToFloat(vm, location).Float);
        }

        public override HassiumObject Power(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat(Math.Pow(Float, args[0].ToFloat(vm, location).Float));
        }

        public override HassiumObject Subtract(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat(Float - args[0].ToFloat(vm, location).Float);
        }

        public override HassiumFloat ToFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }

        public override HassiumInt ToInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt((long)Float);
        }

        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Float.ToString());
        }
    }
}

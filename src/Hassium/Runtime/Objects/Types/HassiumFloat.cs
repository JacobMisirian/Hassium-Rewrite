using System;

namespace Hassium.Runtime.Objects.Types
{
    public class HassiumFloat: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("float");

        public double Float { get; private set; }

        public HassiumFloat(double val)
        {
            Float = val;
            AddType(TypeDefinition);
        }

        public override HassiumObject Add(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Float + args[0].ToFloat(vm).Float);
        }
        public override HassiumObject Divide(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Float / args[0].ToFloat(vm).Float);
        }
        public override HassiumObject EqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Float == args[0].ToFloat(vm).Float);
        }
        public override HassiumObject GreaterThan(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Float > args[0].ToFloat(vm).Float);
        }
        public override HassiumObject GreaterThanOrEqual(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Float >= args[0].ToFloat(vm).Float);
        }
        public override HassiumObject IntegerDivision(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt((long)(Float / args[0].ToFloat(vm).Float));
        }
        public override HassiumObject LesserThan(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Float < args[0].ToFloat(vm).Float);
        }
        public override HassiumObject LesserThanOrEqual(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(Float <= args[0].ToFloat(vm).Float);
        }
        public override HassiumObject Modulus(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt((long)(Float % args[0].ToFloat(vm).Float));
        }
        public override HassiumObject Multiply(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Float * args[0].ToFloat(vm).Float);
        }
        public override HassiumObject Negate(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(-Float);
        }
        public override HassiumObject NotEqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            return EqualTo(vm, args).LogicalNot(vm, args);
        }
        public override HassiumObject Power(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Math.Pow(Float, args[0].ToFloat(vm).Float));
        }
        public override HassiumObject Subtract(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumFloat(Float - args[0].ToFloat(vm).Float);
        }
        public override HassiumChar ToChar(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumChar((char)Float);
        }
        public override HassiumFloat ToFloat(VirtualMachine vm, params HassiumObject[] args)
        {
            return this;
        }
        public override HassiumInt ToInt(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumInt(Convert.ToInt64(Float));
        }
        public override HassiumString ToString(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumString(Float.ToString());
        }
    }
}


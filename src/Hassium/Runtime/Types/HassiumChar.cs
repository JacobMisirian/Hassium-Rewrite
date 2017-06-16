using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hassium.Compiler;

namespace Hassium.Runtime.Types
{
    public class HassiumChar : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("char");

        public char Char { get; private set; }

        public HassiumChar(char val)
        {
            AddType(TypeDefinition);
            Char = val;

            AddAttribute("getBit", getBit, 1);
            AddAttribute("isControl", isControl, 0);
            AddAttribute("isDigit", isDigit, 0);
            AddAttribute("isLetter", isLetter, 0);
            AddAttribute("isLetterOrDigit", isLetterOrDigit, 0);
            AddAttribute("isLower", isLower, 0);
            AddAttribute("isSymbol", isSymbol, 0);
            AddAttribute("isUpper", isUpper, 0);
            AddAttribute("isWhiteSpace", isWhiteSpace, 0);
            AddAttribute("setBit", setBit, 2);
            AddAttribute(TOCHAR, ToChar, 0);
            AddAttribute(TOFLOAT, ToFloat, 0);
            AddAttribute(TOINT, ToInt, 0);
            AddAttribute("toLower", toLower, 0);
            AddAttribute(TOSTRING, ToString, 0);
            AddAttribute("toUpper", toUpper, 0);
        }

        public override HassiumObject Add(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumChar((char)(Char + (char)args[0].ToInt(vm, location).Int));
        }

        public override HassiumObject BitshiftLeft(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char << (int)args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject BitshiftRight(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char >> (int)args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject BitwiseAnd(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char & args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject BitwiseNot(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(~Char);
        }

        public override HassiumObject BitwiseOr(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char | args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject BitwiseXor(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumChar((char)((byte)Char ^ (byte)args[0].ToChar(vm, location).Char));
        }

        public override HassiumObject Divide(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char / args[0].ToInt(vm, location).Int);
        }

        public override HassiumBool EqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Char == args[0].ToChar(vm, location).Char);
        }

        public HassiumBool getBit(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(((byte)Char & (1 << (int)args[0].ToInt(vm, location).Int - 1)) != 0);
        }

        public override HassiumObject GreaterThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((int)Char > args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject GreaterThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((int)Char >= args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject IntegerDivision(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char / args[0].ToInt(vm, location).Int);
        }

        public HassiumBool isControl(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(char.IsControl(Char));
        }

        public HassiumBool isDigit(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(char.IsDigit(Char));
        }

        public HassiumBool isLetter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(char.IsLetter(Char));
        }

        public HassiumBool isLetterOrDigit(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(char.IsLetterOrDigit(Char));
        }

        public HassiumBool isLower(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((int)Char >= 97 && (int)Char <= 122);
        }

        public HassiumBool isSymbol(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(char.IsSymbol(Char));
        }

        public HassiumBool isUpper(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((int)Char >= 65 && (int)Char <= 90);
        }

        public HassiumBool isWhiteSpace(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(char.IsWhiteSpace(Char));
        }

        public override HassiumObject LesserThan(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((int)Char < args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject LesserThanOrEqual(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool((int)Char <= args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject Modulus(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char % args[0].ToInt(vm, location).Int);
        }

        public override HassiumObject Multiply(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(Char * args[0].ToInt(vm, location).Int);
        }

        public override HassiumBool NotEqualTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(Char != args[0].ToChar(vm, location).Char);
        }

        public override HassiumObject Power(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return base.Power(vm, location, args);
        }

        public HassiumChar setBit(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            int index = (int)args[0].ToInt(vm, location).Int;
            bool val = args[1].ToBool(vm, location).Bool;
            if (val)
                return new HassiumChar((char)(Char | 1 << index));
            else
                return new HassiumChar((char)(Char & ~(1 << index)));
        }
        
        public override HassiumObject Subtract(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumChar((char)(Char - (char)args[0].ToInt(vm, location).Int));
        }

        public override HassiumChar ToChar(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }

        public override HassiumFloat ToFloat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumFloat((double)Char);
        }

        public override HassiumInt ToInt(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt((long)Char);
        }
        
        public HassiumChar toLower(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumChar(Char.ToLower(Char));
        }

        public HassiumChar toUpper(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumChar(Char.ToUpper(Char));
        }


        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Char.ToString());
        }
    }
}

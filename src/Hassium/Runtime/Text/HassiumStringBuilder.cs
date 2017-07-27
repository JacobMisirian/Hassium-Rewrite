using Hassium.Compiler;
using Hassium.Runtime.Types;

using System.Text;

namespace Hassium.Runtime.Text
{
    public class HassiumStringBuilder : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("StringBuilder");

        public StringBuilder StringBuilder { get; private set; }

        public HassiumStringBuilder()
        {
            AddType(TypeDefinition);
            AddAttribute(INVOKE, _new, 0, 1);
        }

        public static HassiumStringBuilder _new(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumStringBuilder sb = new HassiumStringBuilder();

            sb.StringBuilder = args.Length == 0 ? new StringBuilder() : new StringBuilder(args[0].ToString(vm, location).String);
            sb.AddAttribute("append", sb.append, 1);
            sb.AddAttribute("appendFormat", sb.appendFormat, -1);
            sb.AddAttribute("appendLine", sb.appendLine, 1);
            sb.AddAttribute("clear", sb.clear, 0);
            sb.AddAttribute("insert", sb.insert, 2);
            sb.AddAttribute("length", new HassiumProperty(sb.get_length));
            sb.AddAttribute("replace", sb.replace, 2);
            sb.AddAttribute(TOSTRING, sb.ToString, 0);

            return sb;
        }

        public HassiumStringBuilder append(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder.Append(args[0].ToString(vm, location).String);

            return this;
        }

        public HassiumStringBuilder appendFormat(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder.Append(GlobalFunctions.format(vm, location, args).ToString(vm, location).String);

            return this;
        }

        public HassiumStringBuilder appendLine(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder.AppendLine(args[0].ToString(vm, location).String);

            return this;
        }

        public HassiumStringBuilder clear(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder.Clear();

            return this;
        }

        public HassiumStringBuilder insert(VirtualMachine vm, SourceLocation location, params HassiumObject[] argS)
        {
            StringBuilder.Insert((int)argS[0].ToInt(vm, location).Int, argS[1].ToString(vm, location).String);

            return this;
        }

        public HassiumInt get_length(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(StringBuilder.Length);
        }

        public HassiumStringBuilder replace(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder.Replace(args[0].ToString(vm, location).String, args[1].ToString(vm, location).String);

            return this;
        }

        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(StringBuilder.ToString());
        }
    }
}

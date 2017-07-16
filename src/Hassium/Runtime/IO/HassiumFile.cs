using System.IO;

using Hassium.Compiler;
using Hassium.Runtime.Types;

namespace Hassium.Runtime.IO
{
    public class HassiumFile : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("File");

        public HassiumString AbsolutePath { get; private set; }
        public HassiumString RelativePath { get; private set; }

        public FileInfo FileInfo { get; private set; }

        public HassiumFile(string path)
        {
            AddType(TypeDefinition);

            AbsolutePath = new HassiumString(Path.GetFullPath(path));
            RelativePath = new HassiumString(path);

            FileInfo = new FileInfo(path);

            AddAttribute("absolutePath", new HassiumProperty(get_absolutePath));
            AddAttribute("copyTo", copyTo, 1);
            AddAttribute("exists", new HassiumProperty(get_exists));
            AddAttribute("extension", new HassiumProperty(get_extension, set_extension));
            AddAttribute("moveTo", moveTo, 1);
            AddAttribute("name", new HassiumProperty(get_name, set_name));
            AddAttribute("relativePath", new HassiumProperty(get_relativePath));
            AddAttribute("size", new HassiumProperty(get_size));
        }

        public HassiumString get_absolutePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return AbsolutePath;
        }

        public HassiumNull copyTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!get_exists(vm, location).Bool)
                vm.RaiseException(new HassiumFileNotFoundException(AbsolutePath));
            File.Copy(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumBool get_exists(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(File.Exists(AbsolutePath.String));
        }

        public HassiumString get_extension(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(FileInfo.Extension);
        }
        public HassiumNull set_extension(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            Path.ChangeExtension(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumNull moveTo(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!get_exists(vm, location).Bool)
                vm.RaiseException(new HassiumFileNotFoundException(AbsolutePath));

            File.Move(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumString get_name(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumString(Path.GetFileName(AbsolutePath.String));
        }
        public HassiumNull set_name(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (!get_exists(vm, location).Bool)
                vm.RaiseException(new HassiumFileNotFoundException(AbsolutePath));

            File.Move(AbsolutePath.String, args[0].ToString(vm, location).String);

            return Null;
        }

        public HassiumString get_relativePath(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return RelativePath;
        }

        public HassiumInt get_size(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumInt(FileInfo.Length);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;

namespace Hassium.Runtime.Types
{
    public class HassiumList : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("list");

        public List<HassiumObject> Values { get; private set; }

        public HassiumList(IEnumerable<HassiumObject> values)
        {
            AddType(TypeDefinition);
            Values = values.ToList();

            AddAttribute("add", add, -1);
            AddAttribute("contains", contains, 1);
            AddAttribute(ITER, Iter, 0);
            AddAttribute(ITERABLEFULL, IterableFull, 0);
            AddAttribute(ITERABLENEXT, IterableNext, 0);
        }

        public HassiumNull add(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            foreach (var arg in args)
                Values.Add(arg);

            return Null;
        }

        public HassiumBool contains(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            foreach (var value in Values)
                if (value == args[0] || args[0].EqualTo(vm, location, args[0]).Bool)
                    return new HassiumBool(true);
            return new HassiumBool(false);
        }

        public override HassiumObject Index(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var index = args[0].ToInt(vm, location);
            if (index.Int < 0 || index.Int >= Values.Count)
            {
                vm.RaiseException(new HassiumIndexOutOfRangeException(this, index));
                return Null;
            }
            return Values[(int)index.Int];
        }

        private int iterIndex = 0;
        public override HassiumObject Iter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return this;
        }

        public override HassiumBool IterableFull(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return new HassiumBool(iterIndex >= Values.Count);
        }

        public override HassiumObject IterableNext(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Values[iterIndex++];
        }

        public override HassiumObject StoreIndex(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            var index = args[0].ToInt(vm, location);
            if (index.Int < 0 || index.Int >= Values.Count)
            {
                vm.RaiseException(new HassiumIndexOutOfRangeException(this, index));
                return Null;
            }
            Values[(int)index.Int] = args[1];
            return args[1];
        }
    }
}

using System;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumTuple: HassiumObject
    {
        public new HassiumObject[] Value { get; private set; }
        public HassiumTuple(HassiumObject[] elements)
        {
            Value = elements;
            Attributes.Add(HassiumObject.INDEX_FUNCTION,    new HassiumFunction(__index__, 1));
            Attributes.Add(HassiumObject.ENUMERABLE_FULL,   new HassiumFunction(__enumerablefull__, 0));
            Attributes.Add(HassiumObject.ENUMERABLE_NEXT,   new HassiumFunction(__enumerablenext__, 0));
            Attributes.Add(HassiumObject.ENUMERABLE_RESET,  new HassiumFunction(__enumerablereset__, 0));
        }

        private HassiumObject __index__ (VirtualMachine vm, HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return Value[((HassiumDouble)obj).ValueInt];
            else if (obj is HassiumInt)
                return Value[(int)((HassiumInt)obj).Value];
            throw new InternalException("Cannot index list with " + obj.GetType().Name);
        }

        public int EnumerableIndex = 0;
        private HassiumObject __enumerablefull__ (VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumBool(EnumerableIndex >= Value.Length);
        }
        private HassiumObject __enumerablenext__ (VirtualMachine vm, HassiumObject[] args)
        {
            return Value[EnumerableIndex++];
        }
        private HassiumObject __enumerablereset__ (VirtualMachine vm, HassiumObject[] args)
        {
            EnumerableIndex = 0;
            return HassiumObject.Null;
        }
    }
}


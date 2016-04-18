using System;
using System.Collections.Generic;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumList: HassiumObject
    {
        public new List<HassiumObject> Value { get; private set; }
        public HassiumList(HassiumObject[] initial)
        {
            Value = new List<HassiumObject>();
            foreach (HassiumObject obj in initial)
                Value.Add(obj);
            Attributes.Add("add", new HassiumFunction(_add, -1));
            Attributes.Add("remove", new HassiumFunction(remove, -1));
            Attributes.Add("contains", new HassiumFunction(contains, -1));
            Attributes.Add(HassiumObject.INDEX_FUNCTION, new HassiumFunction(__index__, 1));
            Attributes.Add(HassiumObject.STORE_INDEX_FUNCTION, new HassiumFunction(__storeindex__, 2));
        }

        private HassiumObject _add(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Value.Add(obj);
            return HassiumObject.Null;
        }
        private HassiumObject remove(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Value.Remove(obj);
            return HassiumObject.Null;
        }
        private HassiumObject contains(HassiumObject[] args)
        {
            return new HassiumBool(Value.Contains(args[0]));
        }
        private HassiumObject __index__ (HassiumObject[] args)
        {
            HassiumObject obj = args[0];
            if (obj is HassiumDouble)
                return Value[((HassiumDouble)obj).ValueInt];
            throw new Exception("Cannot index list with " + obj);
        }
        private HassiumObject __storeindex__ (HassiumObject[] args)
        {
            HassiumObject index = args[0];
            if (index is HassiumDouble)
                Value[((HassiumDouble)index).ValueInt] = args[1];
            else
                throw new Exception("Cannot index with " + index);
            return args[1];
        }
    }
}
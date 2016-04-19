using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hassium.Runtime.StandardLibrary.Types
{
    public class HassiumList: HassiumObject
    {
        public new List<HassiumObject> Value { get; private set; }

        public static HassiumList Create(HassiumObject obj)
        {
            if (!(obj is HassiumList))
                throw new Exception(string.Format("Cannot convert from {0} to HassiumList!", obj.GetType()));
            return (HassiumList)obj;
        }

        public HassiumList(HassiumObject[] initial)
        {
            Value = new List<HassiumObject>();
            foreach (HassiumObject obj in initial)
                Value.Add(obj);
            Attributes.Add("add", new HassiumFunction(_add, -1));
            Attributes.Add("contains", new HassiumFunction(contains, -1));
            Attributes.Add("indexOf", new HassiumFunction(indexOf, 1));
            Attributes.Add("lastIndexOf", new HassiumFunction(lastIndexOf, 1));
            Attributes.Add("length", new HassiumProperty(length));
            Attributes.Add("remove", new HassiumFunction(remove, -1));
            Attributes.Add("reverse", new HassiumFunction(reverse, 0));
            Attributes.Add(HassiumObject.TOSTRING_FUNCTION, new HassiumFunction(__tostring__, 0));
            Attributes.Add(HassiumObject.INDEX_FUNCTION, new HassiumFunction(__index__, 1));
            Attributes.Add(HassiumObject.STORE_INDEX_FUNCTION, new HassiumFunction(__storeindex__, 2));
            Types.Add(GetType().Name);
        }

        private HassiumObject _add(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Value.Add(obj);
            return HassiumObject.Null;
        }
        private HassiumBool contains(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                if (!Value.Any(x => x.Equals(args[0]).Value))
                    return new HassiumBool(false);
            return new HassiumBool(true);
        }
        private HassiumDouble indexOf(HassiumObject[] args)
        {
            for (int i = 0; i < Value.Count; i++)
                if (Value[i].Equals(args[0]).Value)
                    return new HassiumDouble(i);
            return new HassiumDouble(-1);
        }
        private HassiumDouble lastIndexOf(HassiumObject[] args)
        {
            for (int i = Value.Count - 1; i >= 0; i--)
                if (Value[i].Equals(args[0]).Value)
                    return new HassiumDouble(i);
            return new HassiumDouble(-1);
        }
        private HassiumDouble length(HassiumObject[] args)
        {
            return new HassiumDouble(Value.Count);
        }
        private HassiumNull remove(HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Value.Remove(obj);
            return HassiumObject.Null;
        }
        private HassiumList reverse(HassiumObject[] args)
        {
            HassiumObject[] elements = new HassiumObject[Value.Count];
            for (int i = 0; i < elements.Length; i++)
                elements[i] = Value[Value.Count - (i + 1)];
            return new HassiumList(elements);
        }
        private HassiumString __tostring__ (HassiumObject[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (HassiumObject obj in Value)
                sb.Append(obj.ToString() + " ");
            return new HassiumString(sb.ToString());
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
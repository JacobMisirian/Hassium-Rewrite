﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hassium.Runtime.Objects.Types
{
    public class HassiumList: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("list");

        public List<HassiumObject> List { get; private set; }

        public HassiumList(HassiumObject[] elements)
        {
            AddType(TypeDefinition);
            List = new List<HassiumObject>(elements);

            AddAttribute("add",             add);
            AddAttribute("clear",           clear);
            AddAttribute("contains",        contains);
            AddAttribute("remove",          remove);
            AddAttribute(HassiumObject.TOLIST,      ToList,         0);
            AddAttribute(HassiumObject.TOSTRING,    ToString,    0, 1);
            AddAttribute(HassiumObject.TOTUPLE,     ToTuple,        0);
        }

        public HassiumObject add(VirtualMachine vm, params HassiumObject[] args)
        {
            foreach (var obj in args)
                List.Add(obj);
            return args[0];
        }
        public HassiumNull clear(VirtualMachine vm, params HassiumObject[] args)
        {
            List.Clear();
            return HassiumObject.Null;
        }
        public HassiumBool contains(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(List.Contains(args[0]));
        }
        public HassiumObject remove(VirtualMachine vm, params HassiumObject[] args)
        {
            foreach (var obj in args)
                List.Remove(args[0]);
            return args[0];
        }

        public override HassiumObject EqualTo(VirtualMachine vm, params HassiumObject[] args)
        {
            var list = args[0].ToList(vm);
            for (int i = 0; i < list.List.Count; i++)
                if (!list.List[i].EqualTo(vm, List[i]).ToBool(vm).Bool)
                    return new HassiumBool(false);
            return new HassiumBool(true);
        }
        public override HassiumObject Index(VirtualMachine vm, params HassiumObject[] args)
        {
            return List[(int)args[0].ToInt(vm).Int];
        }
        public override HassiumObject StoreIndex(VirtualMachine vm, params HassiumObject[] args)
        {
            return List[(int)args[0].ToInt(vm).Int] = args[1];
        }
        public override HassiumList ToList(VirtualMachine vm, params HassiumObject[] args)
        {
            return this;
        }
        public override HassiumString ToString(VirtualMachine vm, params HassiumObject[] args)
        {
            StringBuilder sb = new StringBuilder();
            if (args.Length == 0)
                foreach (var obj in List)
                    sb.Append(obj.ToString(vm).String);
            else
            {
                string seperator = args[0].ToString(vm).String;
                foreach (var obj in List)
                    sb.AppendFormat("{0}{1}", obj.ToString(vm).String, seperator);
            }
            return new HassiumString(sb.ToString());
        }
        public override HassiumTuple ToTuple(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumTuple(List.ToArray());
        }

        private int iterableIndex = 0;
        public override HassiumObject Iter(VirtualMachine vm, params HassiumObject[] args)
        {
            iterableIndex = 0;
            return this;
        }
        public override HassiumObject IterableFull(VirtualMachine vm, params HassiumObject[] args)
        {
            return new HassiumBool(iterableIndex >= List.Count);
        }
        public override HassiumObject IterableNext(VirtualMachine vm, params HassiumObject[] args)
        {
            return List[iterableIndex++];
        }
    }
}
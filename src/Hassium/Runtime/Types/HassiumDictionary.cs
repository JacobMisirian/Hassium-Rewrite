using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;

namespace Hassium.Runtime.Types
{
    public class HassiumDictionary : HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("dictionary");

        public Dictionary<HassiumObject, HassiumObject> Dictionary { get; private set; }

        public HassiumDictionary(Dictionary<HassiumObject, HassiumObject> initial)
        {
            Dictionary = new Dictionary<HassiumObject, HassiumObject>();
            AddType(TypeDefinition);
            foreach (var pair in initial)
                Dictionary.Add(pair.Key, pair.Value);

            AddAttribute("add", add, 2);
            AddAttribute("containsKey", containsKey, 1);
            AddAttribute("containsValue", containsValue, 1);
            AddAttribute(INDEX, Index, 1);
            AddAttribute(ITER, Iter, 0);
            AddAttribute("keyByValue", keyByValue, 1);
            AddAttribute(STOREINDEX, StoreIndex, 2);
            AddAttribute(TOSTRING, ToString, 0);
            AddAttribute("valueByKey", valueByKey, 1);
        }

        [FunctionAttribute("func add (key : object, val : object) : null")]
        public HassiumNull add(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            Dictionary.Add(args[0], args[1]);
            return Null;
        }

        [FunctionAttribute("func containsKey (key : object) : bool")]
        public HassiumBool containsKey(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Dictionary.ContainsKey(args[0]))
                return True;
            foreach (var key in Dictionary.Keys)
                if (key.EqualTo(vm, location, args[0]).Bool)
                    return True;
            return False;
        }

        [FunctionAttribute("func containsValue (val : object) : bool")]
        public HassiumBool containsValue(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Dictionary.ContainsValue(args[0]))
                return True;
            foreach (var val in Dictionary.Values)
                if (val.EqualTo(vm, location, args[0]).Bool)
                    return True;
            return False;
        }

        [FunctionAttribute("func __index__ (key : object) : object")]
        public override HassiumObject Index(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (Dictionary.ContainsKey(args[0]))
                return Dictionary[args[0]];
            foreach (var key in Dictionary.Keys)
                if (key.EqualTo(vm, location, args[0]).Bool)
                    return Dictionary[key];
            vm.RaiseException(HassiumAttributeNotFoundException._new(vm, location, this, args[0].ToString(vm, location)));
            return Null;
        }

        [FunctionAttribute("func __iter__ () : list")]
        public override HassiumObject Iter(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            HassiumList list = new HassiumList(new HassiumObject[0]);
            foreach (var pair in Dictionary)
                list.add(vm, location, new HassiumTuple(pair.Key, pair.Value));
            return list;
        }

        [FunctionAttribute("func keyByValue (val : object) : object")]
        public HassiumObject keyByValue(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            foreach (var pair in Dictionary)
                if (pair.Value.EqualTo(vm, location, args[0]).ToBool(vm, location).Bool)
                    return pair.Key;
            vm.RaiseException(HassiumKeyNotFoundException._new(vm, location, this, args[0]));
            return Null;
        }

        [FunctionAttribute("func __storeindex__ (key : object, val : object) : object")]
        public override HassiumObject StoreIndex(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            if (containsKey(vm, location, args[0]).Bool)
                Dictionary[args[0]] = args[1];
            else
                Dictionary.Add(args[0], args[1]);
            return args[1];
        }

        [FunctionAttribute("func toString () : string")]
        public override HassiumString ToString(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            foreach (var pair in Dictionary)
                sb.AppendFormat("{0} : {1}, ", pair.Key.ToString(vm, location).String, pair.Value.ToString(vm, location).String);
            if (Dictionary.Count > 0)
                sb.Append("\b\b ");
            sb.Append("}");

            return new HassiumString(sb.ToString());
        }

        [FunctionAttribute("func valueByKey (key : object) : object")]
        public HassiumObject valueByKey(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            return Index(vm, location, args[0]);
        }
    }
}

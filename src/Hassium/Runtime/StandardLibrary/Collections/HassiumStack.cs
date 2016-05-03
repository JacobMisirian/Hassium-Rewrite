using System;
using System.Collections.Generic;
using System.Text;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime.StandardLibrary.Collections
{
    public class HassiumStack: HassiumObject
    {
        public Stack<HassiumObject> Stack { get; set; }
        public HassiumStack()
        {
        }

        private HassiumStack _new(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumStack hassiumStack = new HassiumStack();

            hassiumStack.Stack = new Stack<HassiumObject>();

            return hassiumStack;
        }

        public HassiumObject peek(VirtualMachine vm, HassiumObject[] args)
        {
            return Stack.Peek();
        }
        public HassiumObject pop(VirtualMachine vm, HassiumObject[] args)
        {
            return Stack.Pop();
        }
        public HassiumNull push(VirtualMachine vm, HassiumObject[] args)
        {
            foreach (HassiumObject obj in args)
                Stack.Push(obj);
            return HassiumObject.Null;
        }
        public HassiumList toList(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumList(Stack.ToArray());
        }
        public HassiumString toString(VirtualMachine vm, HassiumObject[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (HassiumObject obj in Stack)
                sb.Append(obj.ToString(vm));

            return new HassiumString(sb.ToString());
        }
    }
}
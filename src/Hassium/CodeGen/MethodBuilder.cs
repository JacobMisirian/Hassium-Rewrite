using System;
using System.Collections.Generic;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary;
using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.CodeGen
{
    public class MethodBuilder: HassiumObject
    {
        public string Name { get; set; }
        public bool IsConstructor { get { return Name == "new"; } }
        public HassiumClass Parent { get; set; }
        public Dictionary<string, int> Parameters = new Dictionary<string, int>();
        public Dictionary<double, int> Labels = new Dictionary<double, int>();
        public List<Instruction> Instructions = new List<Instruction>();
        public Stack<double> BreakLabels = new Stack<double>();

        public override HassiumObject Invoke(VirtualMachine vm, HassiumObject[] args)
        {
            vm.StackFrame.EnterFrame();
            foreach (int param in Parameters.Values)
                vm.StackFrame.Add(param, args[param]);
            vm.ExecuteMethod(this);
            vm.StackFrame.PopFrame();
            if (IsConstructor)
            {
                HassiumClass ret = new HassiumClass();
                ret.Attributes = cloneDictionary(Parent.Attributes);
                foreach (HassiumObject obj in ret.Attributes.Values)
                    if (obj is MethodBuilder)
                        ((MethodBuilder)obj).Parent = ret;
                return ret;
            }
            return null;
        }

        private Dictionary<TKey, TValue> cloneDictionary<TKey, TValue>
            (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                                                                        original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue) entry.Value.Clone());
            }
            return ret;
        }


        public void Emit(InstructionType instructionType, double value = 0)
        {
            Instructions.Add(new Instruction(instructionType, value));
        }
    }
}
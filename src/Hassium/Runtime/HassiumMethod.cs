﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;
using Hassium.Compiler.Emit;
using Hassium.Compiler.Parser;
using Hassium.Runtime.Exceptions;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class HassiumMethod : HassiumObject
    {
        public Stack<int> BreakLabels { get; private set; }
        public Stack<int> ContinueLabels { get; private set; }
        public Dictionary<int, int> Labels { get; private set; }

        public string Name { get; set; }
        public bool IsConstructor { get { return Name == "new"; } }

        public Dictionary<FunctionParameter, int> Parameters { get; private set; }
        public List<HassiumInstruction> Instructions { get; private set; }
        public HassiumMethod ReturnType { get; private set; }

        public HassiumMethod()
        {
            BreakLabels = new Stack<int>();
            ContinueLabels = new Stack<int>();
            Instructions = new List<HassiumInstruction>();
            Labels = new Dictionary<int, int>();
            Parameters = new Dictionary<FunctionParameter, int>();

            AddAttribute(HassiumObject.INVOKE, Invoke);
        }
        public HassiumMethod(string name)
        {
            BreakLabels = new Stack<int>();
            ContinueLabels = new Stack<int>();
            Instructions = new List<HassiumInstruction>();
            Labels = new Dictionary<int, int>();
            Parameters = new Dictionary<FunctionParameter, int>();
            
            Name = name;

            AddAttribute(HassiumObject.INVOKE, Invoke);
        }

        public void Emit(SourceLocation location, InstructionType instructionType, int argument = 0)
        {
            Instructions.Add(new HassiumInstruction(location, instructionType, argument));
        }
        public void EmitLabel(SourceLocation location, int label)
        {
            Labels.Add(label, Instructions.Count - 1);
        }

        public override HassiumObject Invoke(VirtualMachine vm, SourceLocation location, params HassiumObject[] args)
        {
            try
            {
                if (Name != "lambda" && Name != "catch" && Name != "thread") vm.StackFrame.PushFrame();
                int i = 0;
                foreach (var param in Parameters)
                {
                    var arg = args[i++];
                    if (param.Key.FunctionParameterType == FunctionParameterType.Variadic)
                    {
                        if (arg is HassiumList || arg is HassiumTuple)
                            vm.StackFrame.Add(param.Value, arg);
                        else
                        {
                            HassiumList list = new HassiumList(new HassiumObject[0]);
                            for (; i < args.Length; i++)
                                list.add(vm, location, args[i]);
                            vm.StackFrame.Add(param.Value, list);
                        }
                        break;
                    }
                    if (param.Key.FunctionParameterType == FunctionParameterType.Enforced)
                        if (!arg.Types.Contains((HassiumTypeDefinition)vm.Globals[param.Key.Name]))
                            throw new InternalException(vm, location, InternalException.PARAMETER_ERROR, param.Key.Type, arg.Type());
                    vm.StackFrame.Add(param.Value, arg);
                }
                if (IsConstructor)
                {
                    HassiumClass ret = new HassiumClass();
                    ret.Attributes = CloneDictionary(Parent.Attributes);
                    foreach (var type in Parent.Types)
                        ret.AddType(type);
                    foreach (var attrib in ret.Attributes.Values)
                        attrib.Parent = ret;
                    vm.ExecuteMethod(ret.Attributes["new"] as HassiumMethod);
                    vm.StackFrame.PopFrame();
                    vm.CallStack.Pop();
                    return ret;
                }
                else
                {
                    var val = vm.ExecuteMethod(this);
                    if (Name == "catch")
                    {
                        vm.CallStack.Pop();
                        return val;
                    }
                    if (Name != "lambda") vm.StackFrame.PopFrame();
                    return val;
                }
            }
            catch (InternalException ex)
            {
                Console.WriteLine("At location {0}:", ex.VM.CurrentSourceLocation);
                Console.WriteLine("{0} at:", ex.Message);
                while (ex.VM.CallStack.Count > 0)
                    Console.WriteLine(ex.VM.CallStack.Pop());
                Environment.Exit(0);
                return null;
            }
        }

        public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>
 (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }
            return ret;
        }
    }
}
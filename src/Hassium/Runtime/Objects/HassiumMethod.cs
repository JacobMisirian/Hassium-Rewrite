﻿using System;
using System.Collections.Generic;

using Hassium.Compiler;
using Hassium.Compiler.CodeGen;
using Hassium.Compiler.Parser.Ast;

namespace Hassium.Runtime.Objects
{
    public class HassiumMethod: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("func");

        public string Name { get; set; }
        public HassiumClass Parent { get; set; }

        public List<Instruction> Instructions { get; private set; }
        public Dictionary<FuncParameter, int> Parameters { get; private set; }

        public Dictionary<int, int> Labels { get; private set; }
        public Stack<int> BreakLabels { get; private set; }
        public Stack<int> ContinueLabels { get; private set; }

        public string ReturnType { get; set; }

        public bool IsConstructor { get { return Name == "new"; } }

        public HassiumMethod()
        {
            AddType(TypeDefinition);

            Instructions = new List<Instruction>();
            Parameters = new Dictionary<FuncParameter, int>();

            Labels = new Dictionary<int, int>();
            BreakLabels = new Stack<int>();
            ContinueLabels = new Stack<int>();
        }

        public void Emit(SourceLocation location, InstructionType instructionType, int argument = 0)
        {
            Instructions.Add(new Instruction(location, instructionType, argument));
        }
        public void EmitLabel(SourceLocation locatiom, int label)
        {
           // Labels.Add(label, Instructions.Count);
            Instructions.Add(new Instruction(locatiom, InstructionType.Label, label));
        }

        public override HassiumObject Invoke(VirtualMachine vm, params HassiumObject[] args)
        {
            vm.StackFrame.PushFrame();

            int i = 0;
            foreach (var param in Parameters)
            {
                var arg = args[i++];
                if (param.Key.IsEnforced)
                if (!arg.Types.Contains((HassiumTypeDefinition)vm.Globals[param.Key.Type]))
                    throw new InternalException(InternalException.PARAMETER_ERROR, param.Key.Type, arg.Type());
                vm.StackFrame.Add(param.Value, arg);
            }
            var val = vm.ExecuteMethod(this);
            if (ReturnType != "")
            if (val.Type() != vm.Globals[ReturnType])
                throw new InternalException(InternalException.RETURN_ERROR, ReturnType, val.Type());
            vm.StackFrame.PopFrame();

            if (IsConstructor)
            {
                HassiumClass ret = new HassiumClass();
                ret.Attributes = CloneDictionary(Parent.Attributes);
                foreach (var type in Parent.Types)
                    ret.AddType(type);
                foreach (var attrib in ret.Attributes.Values)
                    if (attrib is HassiumMethod)
                        ((HassiumMethod)attrib).Parent = ret;
                return ret;
            }
            return val;
        }

        public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>
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
    }
}


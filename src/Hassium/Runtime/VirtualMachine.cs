using System;
using System.Collections.Generic;

using Hassium.CodeGen;
using Hassium.Runtime.StandardLibrary;
using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime
{
    public class VirtualMachine
    {
        private Dictionary<double, int> labels;
        private Dictionary<double, HassiumObject> globals;
        private Stack<HassiumObject> stack;
        private StackFrame stackFrame;
        private HassiumFlags flags;

        public void Execute(HassiumModule module)
        {
            labels = new Dictionary<double, int>();
            globals = new Dictionary<double, HassiumObject>();
            stack = new Stack<HassiumObject>();
            stackFrame = new StackFrame();
            flags = new HassiumFlags();

            gatherLabels(module.Instructions);
            gatherGlobals(module.ConstantPool);

            for (int position = 0; position < module.Instructions.Count; position++)
            {
                HassiumDouble left, right;
                double argument = module.Instructions[position].Argument;
                switch (module.Instructions[position].InstructionType)
                {
                    case InstructionType.Push_Frame:
                        stackFrame.EnterFrame();
                        break;
                    case InstructionType.Pop_Frame:
                        stackFrame.PopFrame();
                        break;
                    case InstructionType.Add:
                        HassiumObject rightObj = stack.Pop();
                        HassiumObject leftObj = stack.Pop();
                        if (rightObj is HassiumString || leftObj is HassiumString)
                            stack.Push((HassiumString)leftObj + (HassiumString)rightObj);
                        else
                            stack.Push((HassiumDouble)leftObj + (HassiumDouble)rightObj);
                        break;
                    case InstructionType.Sub:
                        right = stack.Pop() as HassiumDouble;
                        left = stack.Pop() as HassiumDouble;
                        stack.Push(left - right);
                        break;
                    case InstructionType.Mul:
                        right = stack.Pop() as HassiumDouble;
                        left = stack.Pop() as HassiumDouble;
                        stack.Push(left * right);
                        break;
                    case InstructionType.Div:
                        right = stack.Pop() as HassiumDouble;
                        left = stack.Pop() as HassiumDouble;
                        stack.Push(left / right);
                        break;
                    case InstructionType.Mod:
                        right = stack.Pop() as HassiumDouble;
                        left = stack.Pop() as HassiumDouble;
                        stack.Push(left % right);
                        break;
                    case InstructionType.Push:
                        stack.Push(new HassiumDouble(argument));
                        break;
                    case InstructionType.Push_String:
                        stack.Push(new HassiumString(module.ConstantPool[Convert.ToInt32(argument)]));
                        break;
                    case InstructionType.Store_Local:
                        HassiumObject value = stack.Pop();
                        if (stackFrame.Contains(argument))
                            stackFrame.Modify(argument, value);
                        else
                            stackFrame.Add(argument, value);
                        break;
                    case InstructionType.Load_Local:
                        stack.Push(stackFrame.GetVariable(argument));
                        break;
                    case InstructionType.Load_Global:
                        stack.Push(globals[argument]);
                        break;
                    case InstructionType.Call:
                        HassiumFunction target = stack.Pop() as HassiumFunction;
                        HassiumObject[] args = new HassiumObject[Convert.ToInt32(argument)];
                        for (int i = 0; i < args.Length; i++)
                            args[i] = stack.Pop();
                        stack.Push(target.Invoke(args));
                        break;
                    case InstructionType.Compare:
                        right = stack.Pop() as HassiumDouble;
                        left = stack.Pop() as HassiumDouble;
                        flags.ProcessFlags((left - right).Value);
                        break;
                    case InstructionType.Jump:
                        position = labels[argument];
                        break;
                    case InstructionType.JumpIfEqual:
                        if (flags.Equal)
                            position = labels[argument];
                        break;
                    case InstructionType.JumpIfNotEqual:
                        if (!flags.Equal)
                            position = labels[argument];
                        break;
                    case InstructionType.JumpIfGreater:
                        if (flags.Greater)
                            position = labels[argument];
                        break;
                    case InstructionType.JumpIfGreaterOrEqual:
                        if (flags.Greater || flags.Equal)
                            position = labels[argument];
                        break;
                    case InstructionType.JumpIfLesser:
                        if (!flags.Greater)
                            position = labels[argument];
                        break;
                    case InstructionType.JumpIfLesserOrEqual:
                        if (!flags.Greater || flags.Equal)
                            position = labels[argument];
                        break;
                }
            }
        }

        private void gatherLabels(List<Instruction> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                Console.WriteLine("{0}\t{1}", instructions[i].InstructionType, instructions[i].Argument);
                if (instructions[i].InstructionType == InstructionType.Label)
                    labels.Add(instructions[i].Argument, i);
            }
        }

        private void gatherGlobals(List<string> constantPool)
        {
            for (int i = 0; i < constantPool.Count; i++)
                if (GlobalFunctions.FunctionList.ContainsKey(constantPool[i]))
                    globals.Add(Convert.ToDouble(i), GlobalFunctions.FunctionList[constantPool[i]]);
        }
    }
}


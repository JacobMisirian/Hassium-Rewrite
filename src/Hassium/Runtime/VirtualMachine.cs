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
        private HassiumModule module;

        public void Execute(HassiumModule module)
        {
            labels = new Dictionary<double, int>();
            globals = new Dictionary<double, HassiumObject>();
            stack = new Stack<HassiumObject>();
            stackFrame = new StackFrame();
            this.module = module;
            gatherGlobals(module.ConstantPool);

            ExecuteMethod((MethodBuilder)module.Attributes["main"]);
        }

        public void ExecuteMethod(MethodBuilder method)
        {
            gatherLabels(method.Instructions);
            for (int position = 0; position < method.Instructions.Count; position++)
            {
                HassiumDouble left, right;
                double argument = method.Instructions[position].Argument;
                switch (method.Instructions[position].InstructionType)
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
                    case InstructionType.Equal:
                        rightObj = stack.Pop();
                        leftObj = stack.Pop();
                        stack.Push(leftObj.Equals(rightObj));
                        break;
                    case InstructionType.Not_Equal:
                        rightObj = stack.Pop();
                        leftObj = stack.Pop();
                        stack.Push(leftObj.NotEquals(rightObj));
                        break;
                    case InstructionType.Greater_Than:
                        rightObj = stack.Pop();
                        leftObj = stack.Pop();
                        stack.Push(leftObj.GreaterThan(rightObj));
                        break;
                    case InstructionType.Greater_Than_Or_Equal:
                        rightObj = stack.Pop();
                        leftObj = stack.Pop();
                        stack.Push(leftObj.GreaterThanOrEqual(rightObj));
                        break;
                    case InstructionType.Lesser_Than:
                        rightObj = stack.Pop();
                        leftObj = stack.Pop();
                        stack.Push(leftObj.LesserThan(rightObj));
                        break;
                    case InstructionType.Lesser_Than_Or_Equal:
                        rightObj = stack.Pop();
                        leftObj = stack.Pop();
                        stack.Push(leftObj.LesserThanOrEqual(rightObj));
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
                    case InstructionType.Load_Attribute:
                        string attribute = module.ConstantPool[Convert.ToInt32(argument)];
                        stack.Push(stack.Pop().Attributes[attribute]);
                        break;
                    case InstructionType.Call:
                        HassiumFunction target = stack.Pop() as HassiumFunction;
                        HassiumObject[] args = new HassiumObject[Convert.ToInt32(argument)];
                        if (!(target is MethodBuilder))
                        {
                            for (int i = 0; i < args.Length; i++)
                                args[i] = stack.Pop();
                            stack.Push(target.Invoke(null, args));
                        }
                        else
                            stack.Push(((MethodBuilder)target).Invoke(this, null));
                        break;
                    case InstructionType.Jump:
                        position = labels[argument];
                        break;
                    case InstructionType.Jump_If_True:
                        if (((HassiumBool)stack.Pop()).Value)
                            position = labels[argument];
                        break;
                    case InstructionType.Jump_If_False:
                        if (!((HassiumBool)stack.Pop()).Value)
                            position = labels[argument];
                        break;
                }
            }
        }

        private void gatherLabels(List<Instruction> instructions)
        {
           // Console.WriteLine();
            for (int i = 0; i < instructions.Count; i++)
            {
              //  Console.WriteLine("{0}\t{1}", instructions[i].InstructionType, instructions[i].Argument);
                if (instructions[i].InstructionType == InstructionType.Label)
                    labels.Add(instructions[i].Argument, i);
            }
        }

        private void gatherGlobals(List<string> constantPool)
        {
            for (int i = 0; i < constantPool.Count; i++)
                if (GlobalFunctions.FunctionList.ContainsKey(constantPool[i]))
                    globals.Add(Convert.ToDouble(i), GlobalFunctions.FunctionList[constantPool[i]]);
                else if (module.Attributes.ContainsKey(constantPool[i]))
                    globals.Add(i, module.Attributes[constantPool[i]]);
        }
    }
}
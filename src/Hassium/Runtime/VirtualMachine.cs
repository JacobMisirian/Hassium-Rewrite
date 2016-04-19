using System;
using System.Collections.Generic;

using Hassium.CodeGen;
using Hassium.Runtime.StandardLibrary;
using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime
{
    public class VirtualMachine
    {
        public StackFrame StackFrame { get { return stackFrame; } }
        public Stack<HassiumObject> Stack { get { return stack; } }
        private Dictionary<double, HassiumObject> globals;
        private Stack<HassiumObject> stack;
        private StackFrame stackFrame;
        private HassiumModule module;

        public void Execute(HassiumModule module)
        {
            globals = new Dictionary<double, HassiumObject>();
            stack = new Stack<HassiumObject>();
            stackFrame = new StackFrame();
            this.module = module;
            gatherGlobals(module.ConstantPool);

            ExecuteMethod((MethodBuilder)module.Attributes["main"]);
        }

        public HassiumObject ExecuteMethod(MethodBuilder method)
        {
            gatherLabels(method);
            for (int position = 0; position < method.Instructions.Count; position++)
            {
                HassiumObject left, right, value, list, index;
                double argument = method.Instructions[position].Argument;
                int argumentInt = Convert.ToInt32(argument);
                string attribute;
               Console.WriteLine("{0}\t{1}", method.Instructions[position].InstructionType, argument);
                switch (method.Instructions[position].InstructionType)
                {
                    case InstructionType.Push_Frame:
                        stackFrame.EnterFrame();
                        break;
                    case InstructionType.Pop_Frame:
                        stackFrame.PopFrame();
                        break;
                    case InstructionType.Binary_Operation:
                        right = stack.Pop();
                        left = stack.Pop();
                        executeBinaryOperation(left, right, argumentInt);
                        break;
                    case InstructionType.Push:
                        stack.Push(new HassiumDouble(argument));
                        break;
                    case InstructionType.Push_String:
                        stack.Push(new HassiumString(module.ConstantPool[Convert.ToInt32(argument)]));
                        break;
                    case InstructionType.Push_Char:
                        stack.Push(new HassiumChar((char)argumentInt));
                        break;
                    case InstructionType.Push_Bool:
                        stack.Push(new HassiumBool(argument == 1));
                        break;
                    case InstructionType.Store_Local:
                        value = stack.Pop();
                        if (stackFrame.Contains(argumentInt))
                            stackFrame.Modify(argumentInt, value);
                        else
                            stackFrame.Add(argumentInt, value);
                        break;
                    case InstructionType.Store_Attribute:
                        HassiumObject location = stack.Pop();
                        attribute = module.ConstantPool[argumentInt];
                        location.Attributes[attribute] = stack.Pop();
                        break;
                    case InstructionType.Load_Local:
                        stack.Push(stackFrame.GetVariable(argumentInt));
                        break;
                    case InstructionType.Load_Global:
                        stack.Push(globals[argument]);
                        break;
                    case InstructionType.Load_Attribute:
                        attribute = module.ConstantPool[argumentInt];
                        HassiumObject attrib = stack.Pop().Attributes[attribute];
                        if (attrib is HassiumProperty)
                            stack.Push(((HassiumProperty)attrib).GetValue(new HassiumObject[] { }));
                        else if (attrib is UserDefinedProperty)
                            stack.Push(ExecuteMethod(((UserDefinedProperty)attrib).GetMethod));
                        else
                            stack.Push(attrib);
                        break;
                    case InstructionType.Create_List:
                        HassiumObject[] elements = new HassiumObject[argumentInt];
                        for (int i = argumentInt - 1; i >= 0; i--)
                            elements[i] = stack.Pop();
                        stack.Push(new HassiumList(elements));
                        break;
                    case InstructionType.Load_List_Element:
                        index = stack.Pop();
                        list = stack.Pop();
                        stack.Push(list.Index(index));
                        break;
                    case InstructionType.Store_List_Element:
                        index = stack.Pop();
                        list = stack.Pop();
                        value = stack.Pop();
                        stack.Push(list.StoreIndex(index, value));
                        break;
                    case InstructionType.Self_Reference:
                        stack.Push(method.Parent);
                        break;
                    case InstructionType.Call:
                        HassiumObject target = stack.Pop();
                        HassiumObject[] args = new HassiumObject[argumentInt];
                        for (int i = 0; i < args.Length; i++)
                             args[i] = stack.Pop();
                        stack.Push(target.Invoke(this, args));
                        break;
                    case InstructionType.Jump:
                        position = method.Labels[argument];
                        break;
                    case InstructionType.Jump_If_True:
                        if (((HassiumBool)stack.Pop()).Value)
                            position = method.Labels[argument];
                        break;
                    case InstructionType.Jump_If_False:
                        if (!((HassiumBool)stack.Pop()).Value)
                            position = method.Labels[argument];
                        break;
                    case InstructionType.Return:
                        return stack.Pop();
                }
            }
            return HassiumObject.Null;
        }

        private void executeBinaryOperation(HassiumObject left, HassiumObject right, int argument)
        {
            switch (argument)
            {
                case 0:
                    stack.Push(left.Add(right));
                    break;
                case 1:
                    stack.Push(left.Sub(right));
                    break;
                case 2:
                    stack.Push(left.Mul(right));
                    break;
                case 3:
                    stack.Push(left.Div(right));
                    break;
                case 4:
                    stack.Push(left.Mod(right));
                    break;
                case 5:
                    stack.Push(left.XOR(right));
                    break;
                case 6:
                    stack.Push(left.OR(right));
                    break;
                case 7:
                    stack.Push(left.Xand(right));
                    break;
                case 8:
                    stack.Push(left.Equals(right));
                    break;
                case 9:
                    stack.Push(left.NotEquals(right));
                    break;
                case 10:
                    stack.Push(left.GreaterThan(right));
                    break;
                case 11:
                    stack.Push(left.GreaterThanOrEqual(right));
                    break;
                case 12:
                    stack.Push(left.LesserThan(right));
                    break;
                case 13:
                    stack.Push(left.LesserThanOrEqual(right));
                    break;
            }

        }

        private void gatherLabels(MethodBuilder method)
        {
           // Console.WriteLine();
            for (int i = 0; i < method.Instructions.Count; i++)
            {
                if (method.Instructions[i].InstructionType == InstructionType.Label)
                    method.Labels.Add(method.Instructions[i].Argument, i);
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
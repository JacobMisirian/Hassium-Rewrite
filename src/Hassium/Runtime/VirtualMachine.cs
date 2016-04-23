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
        public Stack<string> CallStack { get { return callStack; } }
        private StackFrame stackFrame;
        private Stack<HassiumObject> stack;
        private Stack<string> callStack = new Stack<string>();

        private Dictionary<double, HassiumObject> globals;
        private HassiumModule module;

        public void Execute(HassiumModule module)
        {
            globals = new Dictionary<double, HassiumObject>();
            stack = new Stack<HassiumObject>();
            stackFrame = new StackFrame();
            this.module = module;
            gatherGlobals(module.ConstantPool);

            callStack.Push("main");
            ExecuteMethod((MethodBuilder)module.Attributes["main"]);
            callStack.Pop();
        }

        public HassiumObject ExecuteMethod(MethodBuilder method)
        {
            gatherLabels(method);
            for (int position = 0; position < method.Instructions.Count; position++)
            {
                HassiumObject left, right, value, list, index, location;
                double argument = method.Instructions[position].Argument;
                int argumentInt = Convert.ToInt32(argument);
                SourceLocation sourceLocation = method.Instructions[position].SourceLocation;
                string attribute;
  //             Console.WriteLine("{0}\t{1}", method.Instructions[position].InstructionType, argument);
                try
                {
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
                            stack.Push(new HassiumString(module.ConstantPool[argumentInt]));
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
                            location = stack.Pop();
                            attribute = module.ConstantPool[argumentInt];
                            if (location is HassiumProperty)
                            {
                                HassiumProperty builtinProp = location as HassiumProperty;
                                builtinProp.Invoke(this, new HassiumObject[] { stack.Pop() });
                            }
                            else if (location is UserDefinedProperty)
                            {
                                UserDefinedProperty userProp = location as UserDefinedProperty;
                                userProp.SetMethod.Invoke(this, new HassiumObject[] { stack.Pop() });
                            }
                            else
                                try
                                {
                                    location.Attributes[attribute] = stack.Pop();
                                }
                                catch (KeyNotFoundException)
                                {
                                    throw new RuntimeException(location + " does not contain a definition for " + attribute, sourceLocation);
                                }
                            break;
                        case InstructionType.Load_Local:
                            stack.Push(stackFrame.GetVariable(argumentInt));
                            break;
                        case InstructionType.Load_Global:
                            try
                            {
                                stack.Push(globals[argument]);
                            }
                            catch (KeyNotFoundException)
                            {
                                throw new RuntimeException("Cannot find global identifier!", sourceLocation);
                            }
                            break;
                        case InstructionType.Load_Attribute:
                            attribute = module.ConstantPool[argumentInt];
                            location = stack.Pop();
                            HassiumObject attrib = null;
                            try
                            {
                                attrib = location.Attributes[attribute];
                            }
                            catch (KeyNotFoundException)
                            {
                                throw new RuntimeException(location + " does not contain a definition for " + attribute, sourceLocation);
                            }
                            if (attrib is HassiumProperty)
                                stack.Push(((HassiumProperty)attrib).GetValue(this, new HassiumObject[] { }));
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
                            stack.Push(list.Index(this, index));
                            break;
                        case InstructionType.Store_List_Element:
                            index = stack.Pop();
                            list = stack.Pop();
                            value = stack.Pop();
                            stack.Push(list.StoreIndex(this, index, value));
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
                catch (InternalException ex)
                {
                    throw new RuntimeException(ex.Message, sourceLocation);
                }
                catch (DivideByZeroException)
                {
                    throw new RuntimeException("Divide by zero!", sourceLocation);
                }
            }
            return HassiumObject.Null;
        }

        private void executeBinaryOperation(HassiumObject left, HassiumObject right, int argument)
        {
            switch (argument)
            {
                case 0:
                    stack.Push(left.Add(this, right));
                    break;
                case 1:
                    stack.Push(left.Sub(this, right));
                    break;
                case 2:
                    stack.Push(left.Mul(this, right));
                    break;
                case 3:
                    stack.Push(left.Div(this, right));
                    break;
                case 4:
                    stack.Push(left.Mod(this, right));
                    break;
                case 5:
                    stack.Push(left.XOR(this, right));
                    break;
                case 6:
                    stack.Push(left.OR(this, right));
                    break;
                case 7:
                    stack.Push(left.Xand(this, right));
                    break;
                case 8:
                    stack.Push(left.Equals(this, right));
                    break;
                case 9:
                    stack.Push(left.NotEquals(this, right));
                    break;
                case 10:
                    stack.Push(left.GreaterThan(this, right));
                    break;
                case 11:
                    stack.Push(left.GreaterThanOrEqual(this, right));
                    break;
                case 12:
                    stack.Push(left.LesserThan(this, right));
                    break;
                case 13:
                    stack.Push(left.LesserThanOrEqual(this, right));
                    break;
            }

        }

        private void gatherLabels(MethodBuilder method)
        {
            for (int i = 0; i < method.Instructions.Count; i++)
            {
                if (method.Instructions[i].InstructionType == InstructionType.Label)
                {
                    if (method.Labels.ContainsKey(method.Instructions[i].Argument))
                        method.Labels.Remove(method.Instructions[i].Argument);
                    method.Labels.Add(method.Instructions[i].Argument, i);
                }
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
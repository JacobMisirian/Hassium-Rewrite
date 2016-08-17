using System;
using System.Collections.Generic;

using Hassium.Compiler;
using Hassium.Compiler.CodeGen;
using Hassium.Compiler.Parser.Ast;
using Hassium.Runtime.Objects;
using Hassium.Runtime.Objects.Types;

namespace Hassium.Runtime
{
    public class VirtualMachine
    {
        public StackFrame StackFrame { get; private set; }
        public Stack<HassiumObject> Stack { get; private set; }
        public Dictionary<string, HassiumObject> Globals { get; private set; }
        public SourceLocation CurrentSourceLocation { get; private set; }
        public HassiumModule CurrentModule { get; private set; }
        public HassiumMethod CurrentMethod { get; private set; }

        public void Execute(HassiumModule module, List<string> args)
        {
            StackFrame = new StackFrame();
            Stack = new Stack<HassiumObject>();
            Globals = new Dictionary<string, HassiumObject>();
            CurrentModule = module;
            importGlobals();

            StackFrame.PushFrame();
            ExecuteMethod((HassiumMethod)module.Attributes["main"]);
            StackFrame.PopFrame();
        }

        public void ExecuteMethod(HassiumMethod method)
        {
            for (int pos = 0; pos < method.Instructions.Count; pos++)
            {
                HassiumObject left, right, val, list;
                HassiumObject[] elements;
                string attrib;

                int arg = method.Instructions[pos].Argument;
                CurrentSourceLocation = method.Instructions[pos].SourceLocation;

                try
                {
                    switch (method.Instructions[pos].InstructionType)
                    {
                        case InstructionType.BinaryOperation:
                            right = Stack.Pop();
                            left = Stack.Pop();
                            interpretBinaryOperation(left, right, arg);
                            break;
                        case InstructionType.BuildList:
                            elements = new HassiumObject[arg];
                            for (int i = elements.Length - 1; i >= 0; i--)
                                elements[i] = Stack.Pop();
                            Stack.Push(new HassiumList(elements));
                            break;
                        case InstructionType.Call:
                            val = Stack.Pop();
                            elements = new HassiumObject[arg];
                            for (int i = elements.Length - 1; i >= 0; i--)
                                elements[i] = Stack.Pop();
                            Stack.Push(val.Invoke(this, elements));
                            break;
                        case InstructionType.Duplicate:
                            Stack.Push(Stack.Peek());
                            break;
                        case InstructionType.Jump:
                            pos = arg;
                            break;
                        case InstructionType.JumpIfFalse:
                            if (!Stack.Pop().ToBool(this).Bool)
                                pos = arg;
                            break;
                        case InstructionType.JumpIfTrue:
                            if (Stack.Pop().ToBool(this).Bool)
                                pos = arg;
                            break;
                        case InstructionType.LoadAttribute:
                            Stack.Push(Stack.Pop().Attributes[CurrentModule.ConstantPool[arg]]);
                            break;
                        case InstructionType.LoadGlobal:
                            attrib = CurrentModule.ConstantPool[arg];
                            if (Globals.ContainsKey(attrib))
                                Stack.Push(Globals[attrib]);
                            else if (CurrentMethod.Parent.Attributes.ContainsKey(attrib))
                                Stack.Push(CurrentMethod.Parent.Attributes[attrib]);
                            break;
                        case InstructionType.LoadGlobalVariable:
                            Stack.Push(CurrentModule.Globals[arg]);
                            break;
                        case InstructionType.LoadListElement:
                            list = Stack.Pop();
                            Stack.Push(list.Index(this, Stack.Pop()));
                            break;
                        case InstructionType.LoadLocal:
                            Stack.Push(StackFrame.GetVariable(arg));
                            break;
                        case InstructionType.Pop:
                            Stack.Pop();
                            break;
                        case InstructionType.Push:
                            Stack.Push(new HassiumInt(arg));
                            break;
                        case InstructionType.PushConstant:
                            Stack.Push(new HassiumString(CurrentModule.ConstantPool[arg]));
                            break;
                        case InstructionType.PushObject:
                            Stack.Push(CurrentModule.ObjectPool[arg]);
                            break;
                        case InstructionType.StoreAttribute:
                            val = Stack.Pop();
                            attrib = CurrentModule.ConstantPool[arg];
                            val.Attributes[attrib] = Stack.Pop();
                            break;
                        case InstructionType.StoreGlobalVariable:
                            CurrentModule.Globals[arg] = Stack.Pop();
                            break;
                        case InstructionType.StoreLocal:
                            val = Stack.Pop();
                            if (StackFrame.Contains(arg))
                                StackFrame.Modify(arg, val);
                            else
                                StackFrame.Add(arg, val);
                            break;
                            
                    }
                }
                catch (InternalException)
                {
                    
                }
            }
        }

        private void interpretBinaryOperation(HassiumObject left, HassiumObject right, int op)
        {
            switch (op)
            {
                case (int)BinaryOperation.Addition:
                    Stack.Push(left.Add(this, right));
                    break;
                case (int)BinaryOperation.BitshiftLeft:
                    Stack.Push(left.BitshiftLeft(this, right));
                    break;
                case (int)BinaryOperation.BitshiftRight:
                    Stack.Push(left.BitshiftRight(this, right));
                    break;
                case (int)BinaryOperation.BitwiseAnd:
                    Stack.Push(left.BitwiseAnd(this, right));
                    break;
                case (int)BinaryOperation.BitwiseOr:
                    Stack.Push(left.BitwiseOr(this, right));
                    break;
                case (int)BinaryOperation.Division:
                    Stack.Push(left.Divide(this, right));
                    break;
                case (int)BinaryOperation.EqualTo:
                    Stack.Push(left.EqualTo(this, right));
                    break;
                case (int)BinaryOperation.GreaterThan:
                    Stack.Push(left.GreaterThan(this, right));
                    break;
                case (int)BinaryOperation.GreaterThanOrEqual:
                    Stack.Push(left.GreaterThanOrEqual(this, right));
                    break;
                case (int)BinaryOperation.IntegerDivision:
                    Stack.Push(left.IntegerDivision(this, right));
                    break;
                case (int)BinaryOperation.LesserThan:
                    Stack.Push(left.LesserThan(this, right));
                    break;
                case (int)BinaryOperation.LesserThanOrEqual:
                    Stack.Push(left.LesserThanOrEqual(this, right));
                    break;
                case (int)BinaryOperation.LogicalAnd:
                    Stack.Push(left.LogicalAnd(this, right));
                    break;
                case (int)BinaryOperation.LogicalOr:
                    Stack.Push(left.LogicalOr(this, right));
                    break;
                case (int)BinaryOperation.Modulus:
                    Stack.Push(left.Modulus(this, right));
                    break;
                case (int)BinaryOperation.Multiplication:
                    Stack.Push(left.Multiply(this, right));
                    break;
                case (int)BinaryOperation.NotEqualTo:
                    Stack.Push(left.NotEqualTo(this, right));
                    break;
                case (int)BinaryOperation.Power:
                    Stack.Push(left.Power(this, right));
                    break;
                case (int)BinaryOperation.Subraction:
                    Stack.Push(left.Subtract(this, right));
                    break;
            }
        }

        private void importGlobals()
        {
            foreach (string constant in CurrentModule.ConstantPool.Values)
            {
                if (GlobalFunctions.Functions.ContainsKey(constant))
                    Globals.Add(constant, GlobalFunctions.Functions[constant]);
                else if (CurrentModule.Attributes.ContainsKey(constant))
                    Globals.Add(constant, CurrentModule.Attributes[constant]);
            }
        }
    }
}


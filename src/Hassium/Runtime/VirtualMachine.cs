using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Hassium.Compiler;
using Hassium.Compiler.Parser.Ast;
using Hassium.Compiler.Emit;
using Hassium.Runtime.Exceptions;
using Hassium.Runtime.Types;

namespace Hassium.Runtime
{
    public class VirtualMachine : ICloneable
    {
        public Stack<string> CallStack { get; private set; }
        public HassiumMethod CurrentMethod { get; private set; }
        public HassiumModule CurrentModule { get; private set; }
        public SourceLocation CurrentSourceLocation { get; private set; }
        public Dictionary<HassiumMethod, int> ExceptionReturns { get; set; }
        public Dictionary<string, HassiumObject> Globals { get; private set; }
        public Stack<HassiumExceptionHandler> Handlers { get; set; }
        public Stack<HassiumObject> Stack { get; set; }
        public StackFrame StackFrame { get; set; }

        public StackFrame.Frame GlobalFrame { get; set;  }

        public void Execute(HassiumModule module, string[] args, StackFrame.Frame frame = null)
        {
            CallStack = new Stack<string>();
            CurrentModule = module;
            ExceptionReturns = new Dictionary<HassiumMethod, int>();
            Globals = new Dictionary<string, HassiumObject>();
            Handlers = new Stack<HassiumExceptionHandler>();
            Stack = new Stack<HassiumObject>();
            StackFrame = new StackFrame();
            importGlobals();

            var globalClass = module.Attributes["__global__"];
            (globalClass.Attributes["__init__"] as HassiumMethod).Invoke(this, new SourceLocation("", 0, 0));
            GlobalFrame = StackFrame.Frames.Peek();
            if (globalClass.Attributes.ContainsKey("main"))
                (globalClass.Attributes["main"] as HassiumMethod).Invoke(this, new SourceLocation("", 0, 0));

        }

        public HassiumObject ExecuteMethod(HassiumMethod method)
        {

            for (int pos = 0; pos < method.Instructions.Count; pos++)
            {
                if (ExceptionReturns.ContainsKey(method))
                {
                    pos = ExceptionReturns[method];
                    ExceptionReturns.Remove(method);
                }

                HassiumObject left, right, val, list;
                HassiumObject[] elements;
                string attrib;

                int arg = method.Instructions[pos].Arg;
                CurrentSourceLocation = method.Instructions[pos].SourceLocation;
                //Console.WriteLine(method.Instructions[pos].ToString() + "\t"  + method.Name);
                try
                {
                    switch (method.Instructions[pos].InstructionType)
                    {
                        case InstructionType.BinaryOperation:
                            right = Stack.Pop();
                            left = Stack.Pop();
                            interpretBinaryOperation(left, right, arg);
                            break;
                        case InstructionType.BuildClosure:
                            Stack.Push(new HassiumClosure(Stack.Pop() as HassiumMethod, StackFrame.Frames.Peek()));
                            break;
                        case InstructionType.BuildDictionary:
                            var initials = new Dictionary<HassiumObject, HassiumObject>();
                            for (int i = 0; i < arg; i++)
                                initials.Add(Stack.Pop(), Stack.Pop());
                            Stack.Push(new HassiumDictionary(initials));
                            break;
                        case InstructionType.BuildList:
                            elements = new HassiumObject[arg];
                            for (int i = elements.Length - 1; i >= 0; i--)
                                elements[i] = Stack.Pop();
                            Stack.Push(new HassiumList(elements));
                            break;
                        case InstructionType.BuildTuple:
                            HassiumObject[] tupleElements = new HassiumObject[arg];
                            for (int i = arg - 1; i >= 0; i--)
                                tupleElements[i] = Stack.Pop();
                            Stack.Push(new HassiumTuple(tupleElements));
                            break;
                        case InstructionType.Call:
                            val = Stack.Pop();
                            elements = new HassiumObject[arg];
                            for (int i = elements.Length - 1; i >= 0; i--)
                                elements[i] = Stack.Pop();
                            Stack.Push(val.Invoke(this, CurrentSourceLocation, elements));
                            break;
                        case InstructionType.Duplicate:
                            Stack.Push(Stack.Peek());
                            break;
                        case InstructionType.EnforcedAssignment:
                            var type = Globals[Stack.Pop().ToString(this, CurrentSourceLocation).String].Type();
                            val = Stack.Pop();
                            if (!val.Types.Contains(type))
                                throw new InternalException(this, CurrentSourceLocation, "Expected assignment type {0}, got {1}!", type, val.Type());
                            if (StackFrame.Contains(arg))
                                StackFrame.Modify(arg, val);
                            else
                                StackFrame.Add(arg, val);
                            Stack.Push(val);
                            break;
                        case InstructionType.Iter:
                            Stack.Push(Stack.Pop().Iter(this, CurrentSourceLocation));
                            break;
                        case InstructionType.IterableFull:
                            Stack.Push(Stack.Pop().IterableFull(this, CurrentSourceLocation));
                            break;
                        case InstructionType.IterableNext:
                            Stack.Push(Stack.Pop().IterableNext(this, CurrentSourceLocation));
                            break;
                        case InstructionType.Jump:
                            pos = method.Labels[arg];
                            break;
                        case InstructionType.JumpIfFalse:
                        if (!Stack.Pop().ToBool(this, CurrentSourceLocation).Bool)
                            pos = method.Labels[arg];
                            break;
                        case InstructionType.JumpIfTrue:
                            if (Stack.Pop().ToBool(this, CurrentSourceLocation).Bool)
                                pos = method.Labels[arg];
                            break;
                        case InstructionType.LoadAttribute:
                            val = Stack.Pop();
                            try
                            {
                                var attribute = val.Attributes[CurrentModule.ConstantPool[arg]];
                                Stack.Push(attribute);
                            }
                            catch (KeyNotFoundException)
                            {
                                throw new InternalException(this, CurrentSourceLocation, InternalException.ATTRIBUTE_NOT_FOUND, CurrentModule.ConstantPool[arg], val.Type());
                            }
                            break;
                        case InstructionType.LoadGlobal:
                            attrib = CurrentModule.ConstantPool[arg];
                            if (Globals.ContainsKey(attrib))
                                Stack.Push(Globals[attrib]);
                            else if (method.Parent != null)
                            {
                                if (method.Parent.Attributes.ContainsKey(attrib))
                                    Stack.Push(method.Parent.Attributes[attrib]);
                                else
                                    throw new InternalException(this, CurrentSourceLocation, InternalException.VARIABLE_ERROR, attrib);
                            }
                            else 
                                throw new InternalException(this, CurrentSourceLocation, InternalException.VARIABLE_ERROR, attrib);
                            break;
                        case InstructionType.LoadGlobalVariable:
                            try
                            {
                                Stack.Push(CurrentModule.Globals[arg]);
                            }
                            catch (KeyNotFoundException)
                            {
                                throw new InternalException(this, CurrentSourceLocation, InternalException.VARIABLE_ERROR, arg);
                            }
                            break;
                        case InstructionType.LoadIterableElement:
                            list = Stack.Pop();
                            Stack.Push(list.Index(this, CurrentSourceLocation, Stack.Pop()));
                            break;
                        case InstructionType.LoadLocal:
                            if (StackFrame.Contains(arg))
                                Stack.Push(StackFrame.GetVariable(CurrentSourceLocation, this, arg));
                            else
                                Stack.Push(GlobalFrame.GetVariable(arg));
                            break;
                        case InstructionType.Pop:
                            Stack.Pop();
                            break;
                        case InstructionType.PopHandler:
                            Handlers.Pop();
                            break;
                        case InstructionType.Push:
                            Stack.Push(new HassiumInt(arg));
                            break;
                        case InstructionType.PushConstant:
                            Stack.Push(new HassiumString(CurrentModule.ConstantPool[arg]));
                            break;
                        case InstructionType.PushHandler:
                            var handler = CurrentModule.ObjectPool[arg] as HassiumExceptionHandler;
                            handler.Frame = StackFrame.Frames.Peek();
                            Handlers.Push(handler);
                            break;
                        case InstructionType.PushObject:
                            Stack.Push(CurrentModule.ObjectPool[arg]);
                            break;
                        case InstructionType.Raise:
                            RaiseException(Stack.Pop());
                            break;
                        case InstructionType.Return:
                            return Stack.Pop();
                        case InstructionType.SelfReference:
                            Stack.Push(method.Parent);
                            break;
                        case InstructionType.SetInitialAttribute:
                            attrib = Stack.Pop().ToString(this, CurrentSourceLocation).String;
                            val = Stack.Pop();
                            var obj = Stack.Pop();
                            if (obj.Attributes.ContainsKey(attrib))
                                obj.Attributes.Remove(attrib);
                            obj.Attributes.Add(attrib, val);
                            Stack.Push(obj);
                            break;
                        case InstructionType.StoreAttribute:
                            val = Stack.Pop();
                            attrib = CurrentModule.ConstantPool[arg];
                            try
                            {
                                if (val.Attributes.ContainsKey(attrib))
                                    val.Attributes.Remove(attrib);
                                val.Attributes.Add(attrib, Stack.Pop());
                            }
                            catch (KeyNotFoundException)
                            {
                                throw new InternalException(this, CurrentSourceLocation, InternalException.VARIABLE_ERROR, val.Type());
                            }
                            break;
                        case InstructionType.StoreGlobalVariable:
                            CurrentModule.Globals[arg] = Stack.Pop();
                            break;
                        case InstructionType.StoreIterableElement:
                            Stack.Push(Stack.Pop().StoreIndex(this, CurrentSourceLocation, Stack.Pop(), Stack.Pop()));
                            break;
                        case InstructionType.StoreLocal:
                            val = Stack.Pop();
                            if (StackFrame.Contains(arg))
                                StackFrame.Modify(arg, val);
                            else
                                StackFrame.Add(arg, val);
                            break;
                        case InstructionType.Swap:
                            int index = (int)Stack.Pop().ToInt(this, CurrentSourceLocation).Int;
                            val = StackFrame.GetVariable(CurrentSourceLocation, this, index);
                            StackFrame.Modify(index, StackFrame.GetVariable(CurrentSourceLocation, this, arg));
                            StackFrame.Modify(arg, val);
                            Stack.Push(val);
                            break;
                        case InstructionType.UnaryOperation:
                            interpretUnaryOperation(Stack.Pop(), arg);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    RaiseException(new HassiumString(ex.ToString()));
                }
            }
            return HassiumObject.Null;
        }

        private void interpretBinaryOperation(HassiumObject left, HassiumObject right, int op)
        {
            switch (op)
            {
                case (int)BinaryOperation.Addition:
                    Stack.Push(left.Add(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.BitshiftLeft:
                    Stack.Push(left.BitshiftLeft(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.BitshiftRight:
                    Stack.Push(left.BitshiftRight(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.BitwiseAnd:
                    Stack.Push(left.BitwiseAnd(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.BitwiseOr:
                    Stack.Push(left.BitwiseOr(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.BitwiseXor:
                    Stack.Push(left.Xor(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.Division:
                    Stack.Push(left.Divide(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.EqualTo:
                    Stack.Push(left.EqualTo(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.GreaterThan:
                    Stack.Push(left.GreaterThan(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.GreaterThanOrEqual:
                    Stack.Push(left.GreaterThanOrEqual(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.IntegerDivision:
                    Stack.Push(left.IntegerDivision(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.Is:
                    Stack.Push(new HassiumBool(left.Types.Contains(right.Type())));
                    break;
                case (int)BinaryOperation.LesserThan:
                    Stack.Push(left.LesserThan(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.LesserThanOrEqual:
                    Stack.Push(left.LesserThanOrEqual(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.LogicalAnd:
                    Stack.Push(left.LogicalAnd(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.LogicalOr:
                    Stack.Push(left.LogicalOr(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.Modulus:
                    Stack.Push(left.Modulus(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.Multiplication:
                    Stack.Push(left.Multiply(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.NotEqualTo:
                    Stack.Push(left.NotEqualTo(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.NullCoalescing:
                    if (left == HassiumObject.Null || left == null)
                        Stack.Push(right);
                    else
                        Stack.Push(left);
                    break;
                case (int)BinaryOperation.Power:
                    Stack.Push(left.Power(this, CurrentSourceLocation, right));
                    break;
                case (int)BinaryOperation.Subtraction:
                    Stack.Push(left.Subtract(this, CurrentSourceLocation, right));
                    break;
            }
        }

        private void interpretUnaryOperation(HassiumObject target, int op)
        {
            switch (op)
            {
                case (int)UnaryOperation.BitwiseNot:
                    Stack.Push(target.BitwiseNot(this, CurrentSourceLocation));
                    break;
                case (int)UnaryOperation.LogicalNot:
                    Stack.Push(target.LogicalNot(this, CurrentSourceLocation));
                    break;
                case (int)UnaryOperation.Negate:
                    Stack.Push(target.Negate(this, CurrentSourceLocation));
                    break;
            }
        }

        public void RaiseException(HassiumObject message)
        {
            if (Handlers.Count == 0)
                throw new InternalException(this, CurrentSourceLocation, message.ToString(this, CurrentSourceLocation).String);
            var handler = Handlers.Peek();
            handler.Invoke(this, CurrentSourceLocation, message);
            ExceptionReturns.Add(handler.Caller, handler.Caller.Labels[handler.Label]);
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
            foreach (var pair in InternalModule.InternalModules["Types"].Attributes)
                Globals.Add(pair.Key, pair.Value);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

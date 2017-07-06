﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler.Parser;
using Hassium.Compiler.Parser.Ast;
using Hassium.Runtime;
using Hassium.Runtime.Types;

namespace Hassium.Compiler.Emit
{
    public class HassiumCompiler : IVisitor
    {
        private Stack<HassiumMethod> methodStack;
        private Stack<HassiumClass> classStack;

        private SymbolTable table;
        private HassiumModule module;

        public HassiumModule Compile(AstNode ast)
        {
            methodStack = new Stack<HassiumMethod>();
            classStack = new Stack<HassiumClass>();

            table = new SymbolTable();
            module = new HassiumModule();

            classStack.Push(new HassiumClass("__global__"));
            methodStack.Push(new HassiumMethod("__init__") { Parent = classStack.Peek() });

            ast.Visit(this);

            var globalClass = classStack.Pop();
            globalClass.AddAttribute("__init__", methodStack.Pop());
            module.AddAttribute("__global__", globalClass);
            return module;
        }

        public void Accept(ArgumentListNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(AttributeAccessNode node)
        {
            node.Left.Visit(this);
            emit(node.SourceLocation, InstructionType.LoadAttribute, handleConstant(node.Right));
        }
        public void Accept(BinaryOperationNode node)
        {
            switch (node.BinaryOperation)
            {
                case BinaryOperation.Assignment:
                    node.Right.Visit(this);
                    if (node.Left is IdentifierNode)
                    {
                        string identifier = ((IdentifierNode)node.Left).Identifier;
                        if (table.ContainsGlobalSymbol(identifier))
                        {
                            emit(node.SourceLocation, InstructionType.StoreGlobalVariable, table.GetGlobalSymbol(identifier));
                            emit(node.SourceLocation, InstructionType.LoadGlobalVariable, table.GetGlobalSymbol(identifier));
                        }
                        else
                        {
                            if (!table.ContainsSymbol(identifier))
                                table.AddSymbol(identifier);
                            emit(node.SourceLocation, InstructionType.StoreLocal, table.GetSymbol(identifier));
                            emit(node.SourceLocation, InstructionType.LoadLocal, table.GetSymbol(identifier));
                        }
                    }
                    else if (node.Left is AttributeAccessNode)
                    {
                        AttributeAccessNode accessor = node.Left as AttributeAccessNode;
                        accessor.Left.Visit(this);
                        if (!module.ConstantPool.ContainsValue(accessor.Right))
                            module.ConstantPool.Add(accessor.Right.GetHashCode(), accessor.Right);
                        emit(node.SourceLocation, InstructionType.StoreAttribute, accessor.Right.GetHashCode());
                        accessor.Left.Visit(this);
                    }
                    else if (node.Left is IterableAccessNode)
                    {
                        IterableAccessNode access = node.Left as IterableAccessNode;
                        access.Index.Visit(this);
                        access.Target.Visit(this);
                        emit(node.SourceLocation, InstructionType.StoreIterableElement);
                    }
                    break;
                case BinaryOperation.Swap:
                    emit(node.SourceLocation, InstructionType.Push, table.GetSymbol(((IdentifierNode)node.Left).Identifier));
                    emit(node.SourceLocation, InstructionType.Swap, table.GetSymbol(((IdentifierNode)node.Right).Identifier));
                    break;
                default:
                    node.VisitChildren(this);
                    emit(node.SourceLocation, InstructionType.BinaryOperation, (int)node.BinaryOperation);
                    break;
            }
        }
        public void Accept(BreakNode node)
        {
            emit(node.SourceLocation, InstructionType.Jump, methodStack.Peek().BreakLabels.Pop());
        }
        public void Accept(CharNode node)
        {
            emit(node.SourceLocation, InstructionType.PushObject, handleObject(new HassiumChar(node.Char)));
        }
        public void Accept(ClassDeclarationNode node)
        {
            var clazz = new HassiumClass(node.Name);
            clazz.Parent = classStack.Peek();

            handleConstant(node.Name);

            foreach (var inherit in node.Inherits)
            {
                methodStack.Push(new HassiumMethod() { Parent = classStack.Peek() });
                inherit.Visit(this);
                emit(inherit.SourceLocation, InstructionType.Return);
                clazz.Inherits.Add(methodStack.Pop());
            }

            classStack.Push(clazz);
            table.EnterScope();

            node.Body.Visit(this);

            table.LeaveScope();
            handleObject(classStack.Pop());

            classStack.Peek().AddAttribute(node.Name, clazz);
        }
        public void Accept(CodeBlockNode node)
        {
            table.EnterScope();
            node.VisitChildren(this);
            table.LeaveScope();
        }
        public void Accept(ContinueNode node)
        {
            emit(node.SourceLocation, InstructionType.Jump, methodStack.Peek().ContinueLabels.Pop());
        }
        public void Accept(DictionaryDeclarationNode node)
        {
            for (int i = 0; i < node.Keys.Count; i++)
            {
                node.Values[i].Visit(this);
                node.Keys[i].Visit(this);
            }
            emit(node.SourceLocation, InstructionType.BuildDictionary, node.Keys.Count);
        }
        public void Accept(DoWhileNode node)
        {
            var bodyLabel = nextLabel();
            var endLabel = nextLabel();

            int breakLabelCount = methodStack.Peek().BreakLabels.Count;
            int continueLabelCount = methodStack.Peek().ContinueLabels.Count;

            methodStack.Peek().BreakLabels.Push(endLabel);
            methodStack.Peek().ContinueLabels.Push(bodyLabel);

            emitLabel(node.Body.SourceLocation, bodyLabel);
            node.Body.Visit(this);
            node.Condition.Visit(this);
            emit(node.Body.SourceLocation, InstructionType.JumpIfTrue, bodyLabel);
            emitLabel(node.Condition.SourceLocation, endLabel);

            restoreLabels(breakLabelCount, continueLabelCount);
        }
        public void Accept(ExpressionStatementNode node)
        {
            node.Expression.Visit(this);
            emit(node.SourceLocation, InstructionType.Pop);
        }
        public void Accept(FloatNode node)
        {
            emit(node.SourceLocation, InstructionType.PushObject, handleObject(new HassiumFloat(node.Float)));
        }
        public void Accept(ForNode node)
        {
            var bodyLabel = nextLabel();
            var repeatLabel = nextLabel();
            var endLabel = nextLabel();

            int breakLabelCount = methodStack.Peek().BreakLabels.Count;
            int continueLabelCount = methodStack.Peek().ContinueLabels.Count;

            methodStack.Peek().BreakLabels.Push(endLabel);
            methodStack.Peek().ContinueLabels.Push(repeatLabel);

            table.EnterScope();
            node.InitialStatement.Visit(this);
            emitLabel(node.Condition.SourceLocation, bodyLabel);
            node.Condition.Visit(this);
            emit(node.Condition.SourceLocation, InstructionType.JumpIfFalse, endLabel);
            if (node.Body is CodeBlockNode)
                node.Body.VisitChildren(this);
            else
                node.Body.Visit(this);
            emitLabel(node.RepeatStatement.SourceLocation, repeatLabel);
            node.RepeatStatement.Visit(this);
            emit(node.RepeatStatement.SourceLocation, InstructionType.Jump, bodyLabel);
            emitLabel(node.RepeatStatement.SourceLocation, endLabel);
            table.LeaveScope();

            restoreLabels(breakLabelCount, continueLabelCount);
        }
        public void Accept(ForeachNode node)
        {
            var bodyLabel = nextLabel();
            var endLabel = nextLabel();

            int breakLabelCount = methodStack.Peek().BreakLabels.Count;
            int continueLabelCount = methodStack.Peek().ContinueLabels.Count;

            methodStack.Peek().BreakLabels.Push(endLabel);
            methodStack.Peek().ContinueLabels.Push(bodyLabel);

            table.EnterScope();
            var tmp = table.HandleSymbol(nextLabel().ToString());
            node.Expression.Visit(this);
            emit(node.SourceLocation, InstructionType.Iter);
            emit(node.SourceLocation, InstructionType.StoreLocal, tmp);
            emitLabel(node.SourceLocation, bodyLabel);
            emit(node.SourceLocation, InstructionType.LoadLocal, tmp);
            emit(node.SourceLocation, InstructionType.IterableFull);
            emit(node.SourceLocation, InstructionType.JumpIfTrue, endLabel);
            emit(node.SourceLocation, InstructionType.LoadLocal, tmp);
            emit(node.SourceLocation, InstructionType.IterableNext);
            emit(node.SourceLocation, InstructionType.StoreLocal, table.HandleSymbol(node.Variable));
            if (node.Body is CodeBlockNode)
                node.Body.VisitChildren(this);
            else
                node.Body.Visit(this);
            emit(node.SourceLocation, InstructionType.Jump, bodyLabel);
            emitLabel(node.SourceLocation, endLabel);
            table.LeaveScope();

            restoreLabels(breakLabelCount, continueLabelCount);
        }
        public void Accept(FunctionCallNode node)
        {
            foreach (var param in node.Parameters.Arguments)
                param.Visit(this);
            node.Target.Visit(this);
            emit(node.SourceLocation, InstructionType.Call, node.Parameters.Arguments.Count);
        }
        public void Accept(FunctionDeclarationNode node)
        {
            var method = new HassiumMethod(node.Name);
            methodStack.Push(method);

            handleConstant(node.Name);
            method.Parent = classStack.Peek();

            table.EnterScope();

            foreach (var param in node.Parameters)
            {
                if (param.FunctionParameterType == FunctionParameterType.Enforced)
                {
                    methodStack.Push(new HassiumMethod());
                    param.Type.Visit(this);
                    emit(param.Type.SourceLocation, InstructionType.Return);
                    param.EnforcedType = methodStack.Pop();
                }
                method.Parameters.Add(param, table.AddSymbol(param.Name));
            }
            if (node.Body is CodeBlockNode)
                node.Body.VisitChildren(this);
            else
                node.Body.Visit(this);

            if (node.EnforcedReturnType != null)
            {
                methodStack.Push(new HassiumMethod());
                node.EnforcedReturnType.Visit(this);
                emit(node.EnforcedReturnType.SourceLocation, InstructionType.Return);
                method.ReturnType = methodStack.Pop();
            }

            table.LeaveScope();
            classStack.Peek().AddAttribute(method.Name, methodStack.Pop());
        }
        public void Accept(IdentifierNode node)
        {
            if (node.Identifier == "this")
                emit(node.SourceLocation, InstructionType.SelfReference);
            else if (table.ContainsGlobalSymbol(node.Identifier))
                emit(node.SourceLocation, InstructionType.LoadGlobalVariable, table.GetGlobalSymbol(node.Identifier));
            else if (!table.ContainsSymbol(node.Identifier))
                emit(node.SourceLocation, InstructionType.LoadGlobal, handleConstant(node.Identifier));
            else
                emit(node.SourceLocation, InstructionType.LoadLocal, table.GetSymbol(node.Identifier));
        }
        public void Accept(IfNode node)
        {
            var elseLabel = nextLabel();
            var endLabel = nextLabel();

            node.Condition.Visit(this);
            emit(node.Condition.SourceLocation, InstructionType.JumpIfFalse, elseLabel);
            node.IfBody.Visit(this);
            emit(node.IfBody.SourceLocation, InstructionType.Jump, endLabel);
            emitLabel(node.ElseBody.SourceLocation, elseLabel);
            node.ElseBody.Visit(this);
            emitLabel(node.ElseBody.SourceLocation, endLabel);
        }
        public void Accept(IntegerNode node)
        {
            emit(node.SourceLocation, InstructionType.PushObject, handleObject(new HassiumInt(node.Integer)));
        }
        public void Accept(IterableAccessNode node)
        {
            node.Index.Visit(this);
            node.Target.Visit(this);
            emit(node.SourceLocation, InstructionType.LoadIterableElement);
        }
        public void Accept(LambdaNode node)
        {
            var lambda = new HassiumMethod();
            methodStack.Push(lambda);
            lambda.Parent = classStack.Peek();

            table.EnterScope();

            foreach (var param in node.Parameters.Arguments)
            {
                string name = ((IdentifierNode)param).Identifier;
                lambda.Parameters.Add(new FunctionParameter(FunctionParameterType.Normal, name), table.HandleSymbol(name));
            }

            node.Body.VisitChildren(this);

            table.LeaveScope();
            methodStack.Pop();

            emit(node.SourceLocation, InstructionType.PushObject, handleObject(lambda));
            emit(node.SourceLocation, InstructionType.BuildClosure);
        }
        public void Accept(ListDeclarationNode node)
        {
            foreach (var element in node.Elements)
                element.Visit(this);
            emit(node.SourceLocation, InstructionType.BuildList, node.Elements.Count);
        }
        public void Accept(MultipleAssignmentNode node)
        {

        }
        public void Accept(RaiseNode node)
        {
            node.Exception.Visit(this);
            emit(node.SourceLocation, InstructionType.Raise);
        }
        public void Accept(ReturnNode node)
        {
            node.Value.Visit(this);
            emit(node.SourceLocation, InstructionType.Return);
        }
        public void Accept(StringNode node)
        {
            emit(node.SourceLocation, InstructionType.PushObject, handleObject(new HassiumString(node.String)));
        }
        public void Accept(TernaryOperationNode node)
        {

        }
        public void Accept(TryCatchNode node)
        {
            var endLabel = nextLabel();
            var temp = methodStack.Peek();
            methodStack.Push(new HassiumMethod("catch"));
            table.EnterScope();
            methodStack.Peek().Parameters.Add(new FunctionParameter(FunctionParameterType.Normal, "value"), table.HandleSymbol("value"));
            node.CatchBody.VisitChildren(this);
            var handler = new HassiumExceptionHandler(temp, methodStack.Peek(), endLabel);
            handleObject(handler);
            methodStack.Pop();
            emit(node.SourceLocation, InstructionType.PushHandler, handler.GetHashCode());
            node.TryBody.Visit(this);
            emit(node.SourceLocation, InstructionType.PopHandler);
            emitLabel(node.SourceLocation, endLabel);
        }
        public void Accept(TupleNode node)
        {
            foreach (var element in node.Elements)
                element.Visit(this);
            emit(node.SourceLocation, InstructionType.BuildTuple, node.Elements.Count);
        }
        public void Accept(UnaryOperationNode node)
        {
            switch (node.UnaryOperation)
            {
                case UnaryOperation.BitwiseNot:
                    node.Target.Visit(this);
                    emit(node.SourceLocation, InstructionType.UnaryOperation, (int)UnaryOperation.BitwiseNot);
                    break;
                case UnaryOperation.LogicalNot:
                    node.Target.Visit(this);
                    emit(node.SourceLocation, InstructionType.UnaryOperation, (int)UnaryOperation.LogicalNot);
                    break;
                case UnaryOperation.Negate:
                    node.Target.Visit(this);
                    emit(node.SourceLocation, InstructionType.UnaryOperation, (int)UnaryOperation.Negate);
                    break;
                case UnaryOperation.PostDecrement:
                case UnaryOperation.PostIncrement:
                case UnaryOperation.PreDecrement:
                case UnaryOperation.PreIncrement:
                    if (node.Target is IdentifierNode)
                    {
                        string identifier = ((IdentifierNode)node.Target).Identifier;
                        HassiumInstruction loadInstruction, storeInstruction;
                        if (table.ContainsGlobalSymbol(identifier))
                        {
                            loadInstruction = new HassiumInstruction(node.SourceLocation, InstructionType.LoadGlobalVariable, table.GetGlobalSymbol(identifier));
                            storeInstruction = new HassiumInstruction(node.SourceLocation, InstructionType.StoreGlobalVariable, table.GetGlobalSymbol(identifier));
                        }
                        else
                        {
                            loadInstruction = new HassiumInstruction(node.SourceLocation, InstructionType.LoadLocal, table.GetSymbol(identifier));
                            storeInstruction = new HassiumInstruction(node.SourceLocation, InstructionType.StoreLocal, table.GetSymbol(identifier));
                        }
                        methodStack.Peek().Instructions.Add(loadInstruction);
                        if (node.UnaryOperation == UnaryOperation.PostDecrement || node.UnaryOperation == UnaryOperation.PostIncrement)
                            emit(node.SourceLocation, InstructionType.Duplicate);
                        emit(node.SourceLocation, InstructionType.Push, 1);
                        emit(node.SourceLocation, InstructionType.BinaryOperation,
                            node.UnaryOperation == UnaryOperation.PostIncrement || node.UnaryOperation == UnaryOperation.PreIncrement ? (int)BinaryOperation.Addition : (int)BinaryOperation.Subtraction);
                        methodStack.Peek().Instructions.Add(storeInstruction);
                        if (node.UnaryOperation == UnaryOperation.PreDecrement || node.UnaryOperation == UnaryOperation.PreIncrement)
                            methodStack.Peek().Instructions.Add(loadInstruction);
                    }
                    break;
            }
        }
        public void Accept(WhileNode node)
        {
            var bodyLabel = nextLabel();
            var endLabel = nextLabel();

            int breakLabelCount = methodStack.Peek().BreakLabels.Count;
            int continueLabelCount = methodStack.Peek().ContinueLabels.Count;

            methodStack.Peek().BreakLabels.Push(endLabel);
            methodStack.Peek().ContinueLabels.Push(bodyLabel);

            emitLabel(node.Condition.SourceLocation, bodyLabel);
            node.Condition.Visit(this);
            emit(node.Condition.SourceLocation, InstructionType.JumpIfFalse, endLabel);
            node.Body.Visit(this);
            emit(node.Body.SourceLocation, InstructionType.Jump, bodyLabel);
            emitLabel(node.Body.SourceLocation, endLabel);

            restoreLabels(breakLabelCount, continueLabelCount);
        }

        private void emit(SourceLocation location, InstructionType instructionType, int arg = -1)
        {
            methodStack.Peek().Emit(location, instructionType, arg);
        }
        private void emitLabel(SourceLocation location, int label)
        {
            methodStack.Peek().EmitLabel(location, label);
        }

        private int handleConstant(string constant)
        {
            int hashcode = constant.GetHashCode();
            if (!module.ConstantPool.ContainsKey(hashcode))
                module.ConstantPool.Add(hashcode, constant);
            return hashcode;
        }
        private int handleObject(HassiumObject obj)
        {
            int hashcode = obj.GetHashCode();
            if (!module.ObjectPool.ContainsKey(hashcode))
                module.ObjectPool.Add(hashcode, obj);
            return hashcode;
        }

        private int label = 0;
        private int nextLabel()
        {
            return label++;
        }

        private void restoreLabels(int breakLabelCount, int continueLabelCount)
        {
            while (methodStack.Peek().BreakLabels.Count > breakLabelCount)
                methodStack.Peek().BreakLabels.Pop();
            while (methodStack.Peek().ContinueLabels.Count > continueLabelCount)
                methodStack.Peek().ContinueLabels.Pop();
        }
    }
}

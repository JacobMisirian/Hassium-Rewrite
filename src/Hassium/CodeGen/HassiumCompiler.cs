using System;
using System.Collections.Generic;

using Hassium.Parser;
using Hassium.Runtime.StandardLibrary;

namespace Hassium.CodeGen
{
    public class HassiumCompiler : IVisitor
    {
        private SymbolTable table;
        private HassiumModule module;
        private MethodBuilder currentMethod;

        public HassiumModule Compile(AstNode ast, SymbolTable table, string name)
        {
            this.table = table;
            module = new HassiumModule(name);
           
            foreach (AstNode child in ast.Children)
            {
                if (child is FuncNode)
                {
                    child.Visit(this);
                    module.Attributes.Add(currentMethod.Name, currentMethod);
                }
                else if (child is ClassNode)
                {
                    child.Visit(this);
                }
            }
            return module;
        }

        public void Accept(ArgListNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(ArrayAccessNode node)
        {
            node.VisitChildren(this);
            currentMethod.Emit(InstructionType.Load_List_Element);
        }
        public void Accept(ArrayDeclarationNode node)
        {
            node.VisitChildren(this);
            currentMethod.Emit(InstructionType.Create_List, node.Children.Count);
        }
        public void Accept(AttributeAccessNode node)
        {
            node.Left.Visit(this);
            if (!module.ConstantPool.Contains(node.Right))
                module.ConstantPool.Add(node.Right);
            currentMethod.Emit(InstructionType.Load_Attribute, findIndex(node.Right));
        }
        public void Accept(BinaryOperationNode node)
        {
            if (node.BinaryOperation != BinaryOperation.Assignment)
            {
                node.Left.Visit(this);
                node.Right.Visit(this);
            }
            switch (node.BinaryOperation)
            {
                case BinaryOperation.Assignment:
                    node.Right.Visit(this);
                    if (node.Left is IdentifierNode)
                    {
                        string identifier = ((IdentifierNode)node.Left).Identifier;
                        if (!table.FindSymbol(identifier))
                            table.AddSymbol(identifier);
                        currentMethod.Emit(InstructionType.Store_Local, table.GetIndex(identifier));
                        currentMethod.Emit(InstructionType.Load_Local, table.GetIndex(identifier));
                    }
                    else if (node.Left is AttributeAccessNode)
                    {
                        AttributeAccessNode accessor = node.Left as AttributeAccessNode;
                        accessor.Left.Visit(this);
                        if (!module.ConstantPool.Contains(accessor.Right))
                            module.ConstantPool.Add(accessor.Right);
                        currentMethod.Emit(InstructionType.Store_Attribute, findIndex(accessor.Right));
                        accessor.Left.Visit(this);
                    }
                    else if (node.Left is ArrayAccessNode)
                    {
                        ArrayAccessNode access = node.Left as ArrayAccessNode;
                        access.Target.Visit(this);
                        access.Expression.Visit(this);
                        currentMethod.Emit(InstructionType.Store_List_Element);
                    }
                    break;
                case BinaryOperation.Addition:
                    currentMethod.Emit(InstructionType.Binary_Operation, 0);
                    break;
                case BinaryOperation.Subtraction:
                    currentMethod.Emit(InstructionType.Binary_Operation, 1);
                    break;
                case BinaryOperation.Multiplication:
                    currentMethod.Emit(InstructionType.Binary_Operation, 2);
                    break;
                case BinaryOperation.Division:
                    currentMethod.Emit(InstructionType.Binary_Operation, 3);
                    break;
                case BinaryOperation.Modulus:
                    currentMethod.Emit(InstructionType.Binary_Operation, 4);
                    break;
                case BinaryOperation.XOR:
                    currentMethod.Emit(InstructionType.Binary_Operation, 5);
                    break;
                case BinaryOperation.OR:
                    currentMethod.Emit(InstructionType.Binary_Operation, 6);
                    break;
                case BinaryOperation.XAnd:
                    currentMethod.Emit(InstructionType.Binary_Operation, 7);
                    break;
                case BinaryOperation.EqualTo:
                    currentMethod.Emit(InstructionType.Binary_Operation, 8);
                    break;
                case BinaryOperation.NotEqualTo:
                    currentMethod.Emit(InstructionType.Binary_Operation, 9);
                    break;
                case BinaryOperation.GreaterThan:
                    currentMethod.Emit(InstructionType.Binary_Operation, 10);
                    break;
                case BinaryOperation.GreaterThanOrEqual:
                    currentMethod.Emit(InstructionType.Binary_Operation, 11);
                    break;
                case BinaryOperation.LesserThan:
                    currentMethod.Emit(InstructionType.Binary_Operation, 12);
                    break;
                case BinaryOperation.LesserThanOrEqual:
                    currentMethod.Emit(InstructionType.Binary_Operation, 13);
                    break;
            }
        }
        public void Accept(BoolNode node)
        {
            currentMethod.Emit(InstructionType.Push_Bool, node.Value ? 1 : 0);
        }
        public void Accept(BreakNode node)
        {
            currentMethod.Emit(InstructionType.Jump, currentMethod.BreakLabels.Pop());
        }
        public void Accept(CharNode node)
        {
            currentMethod.Emit(InstructionType.Push_Char, node.Char);
        }
        public void Accept(ClassNode node)
        {
            if (!module.ConstantPool.Contains(node.Name))
                module.ConstantPool.Add(node.Name);
            HassiumClass clazz = new HassiumClass();
            foreach (AstNode child in node.Body.Children)
            {
                child.Visit(this);
                if (child is FuncNode)
                {
                    currentMethod.Parent = clazz;
                    clazz.Attributes.Add(currentMethod.Name, currentMethod);
                }
                if (child is PropertyNode)
                {
                    PropertyNode propNode = child as PropertyNode;
                    if (!module.ConstantPool.Contains(propNode.Identifier))
                        module.ConstantPool.Add(propNode.Identifier);
                    UserDefinedProperty property = new UserDefinedProperty(propNode.Identifier);
                    currentMethod = new MethodBuilder();
                    currentMethod.Name =  "__get__" + propNode.Identifier;
                    currentMethod.Parent = clazz;
                    currentMethod.Emit(InstructionType.Push_Frame);
                    table.EnterScope();
                    propNode.GetBody.Visit(this);
                    currentMethod.Emit(InstructionType.Pop_Frame);
                    table.PopScope();
                    property.GetMethod = currentMethod;

                    if (node.Children.Count > 2)
                    {
                        currentMethod = new MethodBuilder();
                        currentMethod.Name = "__set__" + propNode.Identifier;
                        currentMethod.Parent = clazz;
                        currentMethod.Emit(InstructionType.Push_Frame);
                        table.EnterScope();
                        if (!table.FindSymbol("value"))
                            table.AddSymbol("value");
                        currentMethod.Parameters.Add("value", table.GetIndex("value"));
                        propNode.SetBody.Visit(this);
                        currentMethod.Emit(InstructionType.Pop_Frame);
                        table.PopScope();
                        property.SetMethod = currentMethod;
                    }
                    clazz.Attributes.Add(propNode.Identifier, property);
                }
            }
            module.Attributes.Add(node.Name, clazz);
        }
        public void Accept(CodeBlockNode node)
        {
            currentMethod.Emit(InstructionType.Push_Frame);
            node.VisitChildren(this);
            currentMethod.Emit(InstructionType.Pop_Frame);
        }
        public void Accept(ConditionalNode node)
        {
            double endLabel = generateSymbol();
            node.Predicate.Visit(this);
            currentMethod.Emit(InstructionType.Jump_If_False, endLabel);
            node.Body.Visit(this);
            currentMethod.Emit(InstructionType.Label, endLabel);
            if (node.Children.Count > 2)
                node.ElseBody.Visit(this);
        }
        public void Accept(ContinueNode node)
        {
            currentMethod.Emit(InstructionType.Jump, currentMethod.ContinueLabels.Pop());
        }
        public void Accept(ExpressionNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(ForNode node)
        {
            double forLabel = generateSymbol();
            double endLabel = generateSymbol();
            currentMethod.ContinueLabels.Push(forLabel);
            currentMethod.BreakLabels.Push(endLabel);
            node.SingleStatement.Visit(this);
            currentMethod.Emit(InstructionType.Label, forLabel);
            node.Predicate.Visit(this);
            currentMethod.Emit(InstructionType.Jump_If_False, endLabel);
            node.Body.Visit(this);
            node.RepeatStatement.Visit(this);
            currentMethod.Emit(InstructionType.Jump, forLabel);
            currentMethod.Emit(InstructionType.Label, endLabel);
        }
        public void Accept(FuncNode node)
        {
            if (!module.ConstantPool.Contains(node.Name))
                module.ConstantPool.Add(node.Name);

            currentMethod = new MethodBuilder();
            currentMethod.Name = node.Name;

            currentMethod.Emit(InstructionType.Push_Frame);
            table.EnterScope();

            for (int i = node.Parameters.Count - 1; i >= 0; i--)
            {
                table.AddSymbol(node.Parameters[i]);
                currentMethod.Parameters.Add(node.Parameters[i], table.GetIndex(node.Parameters[i]));
            }

            node.Children[0].VisitChildren(this);

            table.PopScope();
            currentMethod.Emit(InstructionType.Pop_Frame);
        }
        public void Accept(FunctionCallNode node)
        {
            for (int i = node.Arguments.Children.Count - 1; i >= 0; i--)
                node.Arguments.Children[i].Visit(this);
            node.Target.Visit(this);
            currentMethod.Emit(InstructionType.Call, node.Arguments.Children.Count);
        }
        public void Accept(IdentifierNode node)
        {
            if (!table.FindSymbol(node.Identifier))
            {
                if (!module.ConstantPool.Contains(node.Identifier))
                    module.ConstantPool.Add(node.Identifier);
                currentMethod.Emit(InstructionType.Load_Global, findIndex(node.Identifier));
            }
            else
                currentMethod.Emit(InstructionType.Load_Local, table.GetIndex(node.Identifier));
        }
        public void Accept(NewNode node)
        {
            node.Call.IsConstructorCall = true;
            node.Call.Visit(this);
        }
        public void Accept(NumberNode node)
        {
            currentMethod.Emit(InstructionType.Push, node.Number);
        }
        public void Accept(PropertyNode node)
        {
        }
        public void Accept(ReturnNode node)
        {
            node.VisitChildren(this);
            currentMethod.Emit(InstructionType.Return);
        }
        public void Accept(StatementNode node)
        {
        }
        public void Accept(StringNode node)
        {
            if (!module.ConstantPool.Contains(node.String))
                module.ConstantPool.Add(node.String);
            currentMethod.Emit(InstructionType.Push_String, findIndex(node.String));
        }
        public void Accept(ThisNode node)
        {
            currentMethod.Emit(InstructionType.Self_Reference, findIndex(currentMethod.Name));
        }
        public void Accept(UnaryOperationNode node)
        {
        }
        public void Accept(WhileNode node)
        {
            double whileLabel = generateSymbol();
            double endLabel = generateSymbol();
            currentMethod.ContinueLabels.Push(whileLabel);
            currentMethod.BreakLabels.Push(endLabel);
            currentMethod.Emit(InstructionType.Label, whileLabel);
            node.Predicate.Visit(this);
            currentMethod.Emit(InstructionType.Jump_If_False, endLabel);
            node.Body.Visit(this);
            currentMethod.Emit(InstructionType.Jump, whileLabel);
            currentMethod.Emit(InstructionType.Label, endLabel);
        }

        private int findIndex(string constant)
        {
            for (int i = 0; i < module.ConstantPool.Count; i++)
                if (module.ConstantPool[i] == constant)
                    return i;
            return -1;
        }

        private double nextSymbol = 0;
        private double generateSymbol()
        {
            return ++nextSymbol;
        }
    }
}

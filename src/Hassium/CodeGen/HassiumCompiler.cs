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
                    break;
                case BinaryOperation.Addition:
                    currentMethod.Emit(InstructionType.Add);
                    break;
                case BinaryOperation.Subtraction:
                    currentMethod.Emit(InstructionType.Sub);
                    break;
                case BinaryOperation.Multiplication:
                    currentMethod.Emit(InstructionType.Mul);
                    break;
                case BinaryOperation.Division:
                    currentMethod.Emit(InstructionType.Div);
                    break;
                case BinaryOperation.Modulus:
                    currentMethod.Emit(InstructionType.Mod);
                    break;
                case BinaryOperation.EqualTo:
                    currentMethod.Emit(InstructionType.Equal);
                    break;
                case BinaryOperation.NotEqualTo:
                    currentMethod.Emit(InstructionType.Not_Equal);
                    break;
                case BinaryOperation.GreaterThan:
                    currentMethod.Emit(InstructionType.Greater_Than);
                    break;
                case BinaryOperation.GreaterThanOrEqual:
                    currentMethod.Emit(InstructionType.Greater_Than_Or_Equal);
                    break;
                case BinaryOperation.LesserThan:
                    currentMethod.Emit(InstructionType.Lesser_Than);
                    break;
                case BinaryOperation.LesserThanOrEqual:
                    currentMethod.Emit(InstructionType.Lesser_Than_Or_Equal);
                    break;
            }
        }
        public void Accept(CharNode node)
        {
            currentMethod.Emit(InstructionType.Push, node.Char);
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
                    clazz.Attributes.Add(currentMethod.Name, currentMethod);
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
        public void Accept(ExpressionNode node)
        {
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
            node.Arguments.Visit(this);
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
        public void Accept(StatementNode node)
        {
        }
        public void Accept(StringNode node)
        {
            if (!module.ConstantPool.Contains(node.String))
                module.ConstantPool.Add(node.String);
            currentMethod.Emit(InstructionType.Push_String, findIndex(node.String));
        }
        public void Accept(UnaryOperationNode node)
        {
        }
        public void Accept(WhileNode node)
        {
            double whileLabel = generateSymbol();
            double endLabel = generateSymbol();
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

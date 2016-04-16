using System;
using System.Collections.Generic;

using Hassium.Parser;

namespace Hassium.CodeGen
{
    public class ModuleCompiler : IVisitor
    {
        private SymbolTable table;
        private HassiumModule module;

        public HassiumModule Compile(AstNode ast, SymbolTable table, string name)
        {
            this.table = table;
            module = new HassiumModule(name);
            ast.Visit(this);
            return module;
        }

        public void Accept(ArgListNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(AttributeAccessNode node)
        {
            node.Left.Visit(this);

        }
        public void Accept(BinaryOperationNode node)
        {
            double falseLabel = 0;
            double endLabel = 0;

            if (node.BinaryOperation != BinaryOperation.Assignment)
            {
                node.Left.Visit(this);
                node.Right.Visit(this);
            }
            switch (node.BinaryOperation)
            {
                case BinaryOperation.Assignment:
                    node.Right.Visit(this);
                    string identifier = ((IdentifierNode)node.Left).Identifier;
                    if (!table.FindSymbol(identifier))
                        table.AddSymbol(identifier);
                    module.Emit(InstructionType.Store_Local, table.GetIndex(identifier));
                    module.Emit(InstructionType.Load_Local, table.GetIndex(identifier));
                    break;
                case BinaryOperation.Addition:
                    module.Emit(InstructionType.Add);
                    break;
                case BinaryOperation.Subtraction:
                    module.Emit(InstructionType.Sub);
                    break;
                case BinaryOperation.Multiplication:
                    module.Emit(InstructionType.Mul);
                    break;
                case BinaryOperation.Division:
                    module.Emit(InstructionType.Div);
                    break;
                case BinaryOperation.Modulus:
                    module.Emit(InstructionType.Mod);
                    break;
                case BinaryOperation.EqualTo:
                    falseLabel = generateSymbol();
                    endLabel = generateSymbol();
                    module.Emit(InstructionType.Compare);
                    module.Emit(InstructionType.JumpIfNotEqual, falseLabel);
                    module.Emit(InstructionType.Push, 1);
                    module.Emit(InstructionType.Jump, endLabel);
                    module.Emit(InstructionType.Label, falseLabel);
                    module.Emit(InstructionType.Push, 0);
                    module.Emit(InstructionType.Label, endLabel);
                    break;
                case BinaryOperation.NotEqualTo:
                    falseLabel = generateSymbol();
                    endLabel = generateSymbol();
                    module.Emit(InstructionType.Compare);
                    module.Emit(InstructionType.JumpIfEqual, falseLabel);
                    module.Emit(InstructionType.Push, 1);
                    module.Emit(InstructionType.Jump, endLabel);
                    module.Emit(InstructionType.Label, falseLabel);
                    module.Emit(InstructionType.Push, 0);
                    module.Emit(InstructionType.Label, endLabel);
                    break;
                case BinaryOperation.GreaterThan:
                    falseLabel = generateSymbol();
                    endLabel = generateSymbol();
                    module.Emit(InstructionType.Compare);
                    module.Emit(InstructionType.JumpIfLesserOrEqual, falseLabel);
                    module.Emit(InstructionType.Push, 1);
                    module.Emit(InstructionType.Jump, endLabel);
                    module.Emit(InstructionType.Label, falseLabel);
                    module.Emit(InstructionType.Push, 0);
                    module.Emit(InstructionType.Label, endLabel);
                    break;
                case BinaryOperation.GreaterThanOrEqual:
                    falseLabel = generateSymbol();
                    endLabel = generateSymbol();
                    module.Emit(InstructionType.Compare);
                    module.Emit(InstructionType.JumpIfLesser, falseLabel);
                    module.Emit(InstructionType.Push, 1);
                    module.Emit(InstructionType.Jump, endLabel);
                    module.Emit(InstructionType.Label, falseLabel);
                    module.Emit(InstructionType.Push, 0);
                    module.Emit(InstructionType.Label, endLabel);
                    break;
                case BinaryOperation.LesserThan:
                    falseLabel = generateSymbol();
                    endLabel = generateSymbol();
                    module.Emit(InstructionType.Compare);
                    module.Emit(InstructionType.JumpIfGreaterOrEqual, falseLabel);
                    module.Emit(InstructionType.Push, 1);
                    module.Emit(InstructionType.Jump, endLabel);
                    module.Emit(InstructionType.Label, falseLabel);
                    module.Emit(InstructionType.Push, 0);
                    module.Emit(InstructionType.Label, endLabel);
                    break;
                case BinaryOperation.LesserThanOrEqual:
                    falseLabel = generateSymbol();
                    endLabel = generateSymbol();
                    module.Emit(InstructionType.Compare);
                    module.Emit(InstructionType.JumpIfGreater, falseLabel);
                    module.Emit(InstructionType.Push, 1);
                    module.Emit(InstructionType.Jump, endLabel);
                    module.Emit(InstructionType.Label, falseLabel);
                    module.Emit(InstructionType.Push, 0);
                    module.Emit(InstructionType.Label, endLabel);
                    break;
            }
        }
        public void Accept(CharNode node)
        {
            module.Emit(InstructionType.Push, node.Char);
        }
        public void Accept(CodeBlockNode node)
        {
            module.Emit(InstructionType.Push_Frame);
            node.VisitChildren(this);
            module.Emit(InstructionType.Pop_Frame);
        }
        public void Accept(ConditionalNode node)
        {
        }
        public void Accept(ExpressionNode node)
        {
        }
        public void Accept(FuncNode node)
        {
        }
        public void Accept(FunctionCallNode node)
        {
            node.Arguments.Visit(this);
            node.Target.Visit(this);
            module.Emit(InstructionType.Call, node.Arguments.Children.Count);
        }
        public void Accept(IdentifierNode node)
        {
            if (!table.FindSymbol(node.Identifier))
            {
                if (!module.ConstantPool.Contains(node.Identifier))
                    module.ConstantPool.Add(node.Identifier);
                module.Emit(InstructionType.Load_Global, findIndex(node.Identifier));
            }
            else
                module.Emit(InstructionType.Load_Local, table.GetIndex(node.Identifier));
        }
        public void Accept(NumberNode node)
        {
            module.Emit(InstructionType.Push, node.Number);
        }
        public void Accept(StatementNode node)
        {
        }
        public void Accept(StringNode node)
        {
            if (!module.ConstantPool.Contains(node.String))
                module.ConstantPool.Add(node.String);
            module.Emit(InstructionType.Push_String, findIndex(node.String));
        }
        public void Accept(UnaryOperationNode node)
        {
        }
        public void Accept(WhileNode node)
        {
            double whileLabel = generateSymbol();
            double endLabel = generateSymbol();
            module.Emit(InstructionType.Label, whileLabel);
            node.Predicate.Visit(this);
            module.Emit(InstructionType.Push, 1);
            module.Emit(InstructionType.Compare);
            module.Emit(InstructionType.JumpIfNotEqual, endLabel);
            node.Body.Visit(this);
            module.Emit(InstructionType.Jump, whileLabel);
            module.Emit(InstructionType.Label, endLabel);
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
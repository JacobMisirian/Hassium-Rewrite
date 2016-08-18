using System;

using Hassium.Compiler.Parser;
using Hassium.Compiler.Parser.Ast;
using Hassium.Compiler.SemanticAnalysis;
using Hassium.Runtime.Objects;
using Hassium.Runtime.Objects.Types;

namespace Hassium.Compiler.CodeGen
{
    public class Compiler : IVisitor
    {
        private SymbolTable table;
        private HassiumModule module;
        private HassiumMethod method;

        private int labelIndex = 0;
        private int nextLabel()
        {
            return labelIndex++;
        }

        public HassiumModule Compile(AstNode ast, SymbolTable table)
        {
            this.table = table;
            module = new HassiumModule();
            method = new HassiumMethod();

            foreach (AstNode child in ast.Children)
            {
                if (child is FuncNode)
                {
                    child.Visit(this);

                    module.Attributes.Add(method.Name, method);
                }
                else if (child is ClassNode)
                {
                    var clazz = compileClass(child as ClassNode);
                    module.Attributes.Add(clazz.Name, clazz);
                }
            }

            return module;
        }

        public void Accept(ArgumentListNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(AttributeAccessNode node)
        {
            node.Left.Visit(this);
            if (!module.ConstantPool.ContainsValue(node.Right))
                module.ConstantPool.Add(node.Right.GetHashCode(), node.Right);
            method.Emit(node.SourceLocation, InstructionType.LoadAttribute, node.Right.GetHashCode());
        }
        public void Accept(BinaryOperationNode node)
        {
            if (node.BinaryOperation != BinaryOperation.Assignment)
                node.VisitChildren(this);
            switch (node.BinaryOperation)
            {
                case BinaryOperation.Addition:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.Addition);
                    break;
                case BinaryOperation.Assignment:
                    node.Right.Visit(this);
                    if (node.Left is IdentifierNode)
                    {
                        string identifier = ((IdentifierNode)node.Left).Identifier;
                        if (table.ContainsGlobalSymbol(identifier))
                        {
                            method.Emit(node.SourceLocation, InstructionType.StoreGlobalVariable, table.GetGlobalSymbol(identifier));
                            method.Emit(node.SourceLocation, InstructionType.LoadGlobalVariable, table.GetGlobalSymbol(identifier));
                        }
                        else
                        {
                            if (!table.ContainsSymbol(identifier))
                                table.AddSymbol(identifier);
                            method.Emit(node.SourceLocation, InstructionType.StoreLocal, table.GetSymbol(identifier));
                            method.Emit(node.SourceLocation, InstructionType.LoadLocal, table.GetSymbol(identifier));
                        }
                    }
                    else if (node.Left is AttributeAccessNode)
                    {
                        AttributeAccessNode accessor = node.Left as AttributeAccessNode;
                        accessor.Left.Visit(this);
                        if (!module.ConstantPool.ContainsValue(accessor.Right))
                            module.ConstantPool.Add(accessor.Right.GetHashCode(), accessor.Right);
                        method.Emit(node.SourceLocation, InstructionType.StoreAttribute, accessor.Right.GetHashCode());
                        accessor.Left.Visit(this);
                    }
                    else if (node.Left is ListAccessNode)
                    {
                        ListAccessNode access = node.Left as ListAccessNode;
                        access.Element.Visit(this);
                        access.Target.Visit(this);
                        method.Emit(node.SourceLocation, InstructionType.StoreListElement);
                    }
                    break;
                case BinaryOperation.BitshiftLeft:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.BitshiftLeft);
                    break;
                case BinaryOperation.BitshiftRight:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.BitshiftRight);
                    break;
                case BinaryOperation.BitwiseAnd:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.BitwiseAnd);
                    break;
                case BinaryOperation.BitwiseOr:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.BitwiseOr);
                    break;
                case BinaryOperation.Division:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.Division);
                    break;
                case BinaryOperation.EqualTo:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.EqualTo);
                    break;
                case BinaryOperation.GreaterThan:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.GreaterThan);
                    break;
                case BinaryOperation.GreaterThanOrEqual:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.GreaterThanOrEqual);
                    break;
                case BinaryOperation.IntegerDivision:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.IntegerDivision);
                    break;
                case BinaryOperation.LesserThan:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.LesserThan);
                    break;
                case BinaryOperation.LesserThanOrEqual:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.LesserThanOrEqual);
                    break;
                case BinaryOperation.LogicalAnd:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.LogicalAnd);
                    break;
                case BinaryOperation.LogicalOr:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.LogicalOr);
                    break;
                case BinaryOperation.Modulus:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.Modulus);
                    break;
                case BinaryOperation.Multiplication:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.Multiplication);
                    break;
                case BinaryOperation.NotEqualTo:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.NotEqualTo);
                    break;
                case BinaryOperation.Power:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.Power);
                    break;
                case BinaryOperation.Subraction:
                    method.Emit(node.SourceLocation, InstructionType.BinaryOperation, (int)BinaryOperation.Subraction);
                    break;
            }
        }
        public void Accept(BreakNode node)
        {
            method.Emit(node.SourceLocation, InstructionType.Jump, method.BreakLabels.Pop());
        }
        public void Accept(CharNode node)
        {
            var ch = new HassiumChar(node.Char);
            if (!module.ObjectPool.ContainsKey(ch.GetHashCode()))
                module.ObjectPool.Add(ch.GetHashCode(), ch);
            method.Emit(node.SourceLocation, InstructionType.PushObject, ch.GetHashCode());
        }
        public void Accept(ClassNode node)
        {
            module.Attributes.Add(node.Name, compileClass(node));
        }
        private HassiumClass compileClass(ClassNode node)
        {
            if (!module.ConstantPool.ContainsValue(node.Name))
                module.ConstantPool.Add(node.Name.GetHashCode(), node.Name);
            HassiumClass clazz = new HassiumClass();
            clazz.Name = node.Name;
            clazz.TypeDefinition = new HassiumTypeDefinition(clazz.Name);
            clazz.Types.Add(clazz.TypeDefinition);

            foreach (AstNode child in node.Body.Children)
            {
                if (child is FuncNode)
                {
                    child.Visit(this);
                    method.Parent = clazz;
                    if (clazz.Attributes.ContainsKey(method.Name))
                        clazz.Attributes[method.Name] = method;
                    else
                        clazz.Attributes.Add(method.Name, method);
                }
                else if (child is ClassNode)
                    clazz.Attributes.Add(((ClassNode)child).Name, compileClass(child as ClassNode));
            }
            return clazz;
        }
        public void Accept(CodeBlockNode node)
        {
            table.PushScope();
            node.VisitChildren(this);
            table.PopScope();
        }
        public void Accept(ContinueNode node)
        {
            method.Emit(node.SourceLocation, InstructionType.Jump, method.ContinueLabels.Pop());
        }
        public void Accept(ExpressionStatementNode node)
        {
            node.VisitChildren(this);
            method.Emit(node.SourceLocation, InstructionType.Pop);
        }
        public void Accept(FloatNode node)
        {
            var fl = new HassiumFloat(node.Number);
            if (!module.ObjectPool.ContainsKey(fl.GetHashCode()))
                module.ObjectPool.Add(fl.GetHashCode(), fl);
            method.Emit(node.SourceLocation, InstructionType.PushObject, fl.GetHashCode());
        }
        public void Accept(ForNode node)
        {
            var startLabel = nextLabel();
            var endLabel = nextLabel();
            method.ContinueLabels.Push(startLabel);
            method.ContinueLabels.Push(endLabel);

            node.StartStatement.Visit(this);
            method.EmitLabel(node.SourceLocation, startLabel);
            node.Predicate.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.JumpIfFalse, endLabel);
            node.Body.Visit(this);
            node.RepeatStatement.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Jump, startLabel);
            method.EmitLabel(node.SourceLocation, endLabel);
        }
        public void Accept(ForeachNode node)
        {
            var startLabel = nextLabel();
            var endLabel = nextLabel();
            method.ContinueLabels.Push(startLabel);
            method.BreakLabels.Push(endLabel);

            int tmp, variable;
            if (table.ContainsSymbol("__tmp__"))
                tmp = table.GetSymbol("__tmp__");
            else
                tmp = table.AddSymbol("__tmp__");
            if (table.ContainsSymbol(node.Variable))
                variable = table.GetSymbol(node.Variable);
            else
                variable = table.AddSymbol(node.Variable);
            
            node.Target.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Iter);
            method.Emit(node.SourceLocation, InstructionType.StoreLocal, tmp);
            method.EmitLabel(node.SourceLocation, startLabel);
            method.Emit(node.SourceLocation, InstructionType.LoadLocal, tmp);
            method.Emit(node.SourceLocation, InstructionType.IterableFull);
            method.Emit(node.SourceLocation, InstructionType.JumpIfTrue, endLabel);
            method.Emit(node.SourceLocation, InstructionType.LoadLocal, tmp);
            method.Emit(node.SourceLocation, InstructionType.IterableNext);
            method.Emit(node.SourceLocation, InstructionType.StoreLocal, variable);
            node.Body.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Jump, startLabel);
            method.EmitLabel(node.SourceLocation, endLabel);
        }
        public void Accept(FuncNode node)
        {
            if (!module.ConstantPool.ContainsKey(node.Name.GetHashCode()))
                module.ConstantPool.Add(node.Name.GetHashCode(), node.Name);

            method = new HassiumMethod();
            method.Parent = new HassiumClass();
            method.Name = node.Name;
            method.ReturnType = node.ReturnType;

            table.PushScope();

            foreach (var param in node.Parameters)
                method.Parameters.Add(param, table.AddSymbol(param.Name));

            node.Children[0].VisitChildren(this);
            table.PopScope();
        }
        public void Accept(FunctionCallNode node)
        {
            foreach (AstNode param in node.Parameters.Children)
                param.Visit(this);
            node.Target.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Call, node.Parameters.Children.Count);
        }
        public void Accept(IdentifierNode node)
        {
            if (table.ContainsGlobalSymbol(node.Identifier))
                method.Emit(node.SourceLocation, InstructionType.LoadGlobalVariable, table.GetGlobalSymbol(node.Identifier));
            else if (!table.ContainsSymbol(node.Identifier))
            {
                if (!module.ConstantPool.ContainsValue(node.Identifier))
                    module.ConstantPool.Add(node.Identifier.GetHashCode(), node.Identifier);
                method.Emit(node.SourceLocation, InstructionType.LoadGlobal, node.Identifier.GetHashCode());
            }
            else
                method.Emit(node.SourceLocation, InstructionType.LoadLocal, table.GetSymbol(node.Identifier));
        }
        public void Accept(IfNode node)
        {
            var elseLabel = nextLabel();
            var endLabel = nextLabel();

            node.Predicate.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.JumpIfFalse, elseLabel);
            node.Body.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Jump, endLabel);
            method.EmitLabel(node.SourceLocation, elseLabel);
            if (node.Children.Count > 2)
                node.ElseBody.Visit(this);
            method.EmitLabel(node.SourceLocation, endLabel);
        }
        public void Accept(IntegerNode node)
        {
            var i = new HassiumInt(node.Number);
            if (!module.ObjectPool.ContainsKey(i.GetHashCode()))
                module.ObjectPool.Add(i.GetHashCode(), i);
            method.Emit(node.SourceLocation, InstructionType.PushObject, i.GetHashCode());
        }
        public void Accept(ListAccessNode node)
        {
            node.Element.Visit(this);
            node.Target.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.LoadListElement);
        }
        public void Accept(ListDeclarationNode node)
        {
            foreach (var val in node.InitialValues)
                val.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.BuildList, node.InitialValues.Count);
        }
        public void Accept(ReturnNode node)
        {
            node.Value.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Return);
        }
        public void Accept(StatementNode node) {}
        public void Accept(StringNode node)
        {
            var str = new HassiumString(node.String);
            if (!module.ObjectPool.ContainsKey(str.GetHashCode()))
                module.ObjectPool.Add(str.GetHashCode(), str);
            method.Emit(node.SourceLocation, InstructionType.PushObject, str.GetHashCode());
        }
        public void Accept(UnaryOperationNode node)
        {
            switch (node.UnaryOperation)
            {
                case UnaryOperation.BitwiseNot:
                    node.Target.Visit(this);
                    method.Emit(node.SourceLocation, InstructionType.UnaryOperation, (int)UnaryOperation.BitwiseNot);
                    break;
                case UnaryOperation.LogicalNot:
                    node.Target.Visit(this);
                    method.Emit(node.SourceLocation, InstructionType.UnaryOperation, (int)UnaryOperation.LogicalNot);
                    break;
                case UnaryOperation.Negate:
                    node.Target.Visit(this);
                    method.Emit(node.SourceLocation, InstructionType.UnaryOperation, (int)UnaryOperation.Negate);
                    break;
                case UnaryOperation.PostDecrement:
                case UnaryOperation.PostIncrement:
                case UnaryOperation.PreDecrement:
                case UnaryOperation.PreIncrement:
                    if (node.Target is IdentifierNode)
                    {
                        string identifier = ((IdentifierNode)node.Target).Identifier;
                        Instruction loadInstruction, storeInstruction;
                        if (table.ContainsGlobalSymbol(identifier))
                        {
                            loadInstruction = new Instruction(node.SourceLocation, InstructionType.LoadGlobalVariable, table.GetGlobalSymbol(identifier));
                            storeInstruction = new Instruction(node.SourceLocation, InstructionType.StoreGlobalVariable, table.GetGlobalSymbol(identifier));
                        }
                        else
                        {
                            loadInstruction = new Instruction(node.SourceLocation, InstructionType.LoadLocal, table.GetSymbol(identifier));
                            storeInstruction = new Instruction(node.SourceLocation, InstructionType.StoreLocal, table.GetSymbol(identifier));
                        }
                        method.Instructions.Add(loadInstruction);
                        if (node.UnaryOperation == UnaryOperation.PostDecrement || node.UnaryOperation == UnaryOperation.PostIncrement)
                            method.Emit(node.SourceLocation, InstructionType.Duplicate);
                        method.Emit(node.SourceLocation, InstructionType.Push, 1);
                        method.Emit(node.SourceLocation, InstructionType.BinaryOperation, 
                            node.UnaryOperation == UnaryOperation.PostIncrement || node.UnaryOperation == UnaryOperation.PreIncrement ? (int)BinaryOperation.Addition : (int)BinaryOperation.Subraction);
                        method.Instructions.Add(storeInstruction);
                        if (node.UnaryOperation == UnaryOperation.PreDecrement || node.UnaryOperation == UnaryOperation.PreIncrement)
                            method.Instructions.Add(loadInstruction);
                    }
                    break;
            }
        }
        public void Accept(WhileNode node)
        {
            var startLabel = nextLabel();
            var endLabel = nextLabel();
            method.ContinueLabels.Push(startLabel);
            method.BreakLabels.Push(endLabel);

            method.EmitLabel(node.SourceLocation, startLabel);
            node.Predicate.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.JumpIfFalse, endLabel);
            node.Body.Visit(this);
            method.Emit(node.SourceLocation, InstructionType.Jump, startLabel);
            method.EmitLabel(node.SourceLocation, endLabel);
        }
    }
}
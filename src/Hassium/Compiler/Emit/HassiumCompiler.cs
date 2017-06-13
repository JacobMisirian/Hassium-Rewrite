using System;
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

            classStack.Push(new HassiumClass());
            methodStack.Push(new HassiumMethod("__init__") { Parent = classStack.Peek() } );

            ast.Visit(this);

            var globalClass = classStack.Pop();
            globalClass.AddAttribute("__init__", methodStack.Pop());
            module.AddAttribute("__global__", globalClass);
            return module;
        }

        public void Accept(ArgumentListNode node)
        {

        }
        public void Accept(AttributeAccessNode node)
        {

        }
        public void Accept(BinaryOperationNode node)
        {

        }
        public void Accept(BreakNode node)
        {

        }
        public void Accept(CharNode node)
        {

        }
        public void Accept(CodeBlockNode node)
        {
            table.EnterScope();
            node.VisitChildren(this);
            table.LeaveScope();
        }
        public void Accept(ContinueNode node)
        {

        }
        public void Accept(DictionaryDeclarationNode node)
        {

        }
        public void Accept(ExpressionStatementNode node)
        {
            node.Expression.Visit(this);
            emit(node.SourceLocation, InstructionType.Pop);
        }
        public void Accept(FloatNode node)
        {

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
                method.Parameters.Add(param, table.AddSymbol(param.Name));
            if (node.Body is CodeBlockNode)
                node.Body.VisitChildren(this);
            else
                node.Body.Visit(this);

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

        }
        public void Accept(IntegerNode node)
        {

        }
        public void Accept(IterableAccessNode node)
        {

        }
        public void Accept(LambdaNode node)
        {

        }
        public void Accept(ListDeclarationNode node)
        {

        }
        public void Accept(MultipleAssignmentNode node)
        {

        }
        public void Accept(StringNode node)
        {
            emit(node.SourceLocation, InstructionType.PushObject, handleObject(new HassiumString(node.Value)));
        }
        public void Accept(TernaryOperationNode node)
        {

        }
        public void Accept(TupleNode node)
        {

        }
        public void Accept(UnaryOperationNode node)
        {

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
    }
}

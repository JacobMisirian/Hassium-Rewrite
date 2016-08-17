using System;

using Hassium.Compiler.Parser.Ast;

namespace Hassium.Compiler.Parser
{
    public interface IVisitor
    {
        void Accept(ArgumentListNode node);
        void Accept(AttributeAccessNode node);
        void Accept(BinaryOperationNode node);
        void Accept(BoolNode node);
        void Accept(CharNode node);
        void Accept(ClassNode node);
        void Accept(CodeBlockNode node);
        void Accept(ExpressionStatementNode node);
        void Accept(FloatNode node);
        void Accept(ForNode node);
        void Accept(FuncNode node);
        void Accept(FunctionCallNode node);
        void Accept(IdentifierNode node);
        void Accept(IfNode node);
        void Accept(IntegerNode node);
        void Accept(ListAccessNode node);
        void Accept(ListDeclarationNode node);
        void Accept(ReturnNode node);
        void Accept(StatementNode node);
        void Accept(StringNode node);
        void Accept(UnaryOperationNode node);
        void Accept(WhileNode node);
    }
}
   
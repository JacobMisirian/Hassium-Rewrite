using System;

namespace Hassium.Parser
{
    public interface IVisitor
    {
        void Accept(ArgListNode node);
        void Accept(ArrayAccessNode node);
        void Accept(ArrayDeclarationNode node);
        void Accept(AttributeAccessNode node);
        void Accept(BinaryOperationNode node);
        void Accept(BoolNode node);
        void Accept(BreakNode node);
        void Accept(CharNode node);
        void Accept(ContinueNode node);
        void Accept(ClassNode node);
        void Accept(ConditionalNode node);
        void Accept(CodeBlockNode node);
        void Accept(EnumNode node);
        void Accept(ExpressionNode node);
        void Accept(ForNode node);
        void Accept(FuncNode node);
        void Accept(FunctionCallNode node);
        void Accept(IdentifierNode node);
        void Accept(NewNode node);
        void Accept(NumberNode node);
        void Accept(PropertyNode node);
        void Accept(ReturnNode node);
        void Accept(StatementNode node);
        void Accept(StringNode node);
        void Accept(ThisNode node);
        void Accept(UseNode node);
        void Accept(UnaryOperationNode node);
        void Accept(WhileNode node);
    }
}


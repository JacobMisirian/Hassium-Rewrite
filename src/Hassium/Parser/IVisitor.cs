using System;

namespace Hassium.Parser
{
    public interface IVisitor
    {
        void Accept(ArgListNode node);
        void Accept(AttributeAccessNode node);
        void Accept(BinaryOperationNode node);
        void Accept(CharNode node);
        void Accept(ClassNode node);
        void Accept(ConditionalNode node);
        void Accept(CodeBlockNode node);
        void Accept(ExpressionNode node);
        void Accept(FuncNode node);
        void Accept(FunctionCallNode node);
        void Accept(IdentifierNode node);
        void Accept(NewNode node);
        void Accept(NumberNode node);
        void Accept(StatementNode node);
        void Accept(StringNode node);
        void Accept(UnaryOperationNode node);
        void Accept(WhileNode node);
    }
}


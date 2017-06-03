using Hassium.Compiler.Parser.Ast;

namespace Hassium.Compiler
{
    public interface IVisitor
    {
        void Accept(BinaryOperationNode node);
        void Accept(BreakNode node);
        void Accept(CharNode node);
        void Accept(CodeBlockNode node);
        void Accept(ContinueNode node);
        void Accept(FloatNode node);
        void Accept(IdentifierNode node);
        void Accept(IfNode node);
        void Accept(IntegerNode node);
        void Accept(StringNode node);
        void Accept(UnaryOperationNode node);
    }
}

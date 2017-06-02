using Hassium.Compiler.Parser.Ast;

namespace Hassium.Compiler
{
    public interface IVisitor
    {
        void Accept(BinaryOperationNode node);
        void Accept(CharNode node);
        void Accept(FloatNode node);
        void Accept(IdentifierNode node);
        void Accept(IntegerNode node);
        void Accept(StringNode node);
        void Accept(UnaryOperationNode node);
    }
}

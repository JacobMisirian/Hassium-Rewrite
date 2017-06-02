using System;

namespace Hassium.Compiler.Parser.Ast
{
    public class UnaryOperationNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public AstNode Target { get; private set; }

        public UnaryOperationNode(SourceLocation location, AstNode target)
        {
            SourceLocation = location;

            Target = target;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            Target.Visit(visitor);
        }
    }

    public enum UnaryOperation
    {
        BitwiseNot,
        LogicalNot,
        Negate
    }
}
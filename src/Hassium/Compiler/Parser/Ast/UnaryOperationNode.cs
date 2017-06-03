using System;

namespace Hassium.Compiler.Parser.Ast
{
    public class UnaryOperationNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public UnaryOperation UnaryOperation { get; private set; }

        public AstNode Target { get; private set; }

        public UnaryOperationNode(SourceLocation location, UnaryOperation operation, AstNode target)
        {
            SourceLocation = location;

            UnaryOperation = operation;

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
        Negate,
        PostDecrement,
        PostIncrement,
        PreDecrement,
        PreIncrement
    }
}
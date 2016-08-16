using System;

namespace Hassium.Compiler.Parser.Ast
{
    public class BinaryOperationNode: AstNode
    {
        public BinaryOperation BinaryOperation { get; private set; }
        public AstNode Left { get { return Children[0]; } }
        public AstNode Right { get { return Children[1]; } }

        public BinaryOperationNode(SourceLocation location, BinaryOperation operation, AstNode left, AstNode right)
        {
            this.SourceLocation = location;
            BinaryOperation = operation;
            Children.Add(left);
            Children.Add(right);
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            foreach (AstNode child in Children)
                child.Visit(visitor);
        }
    }

    public enum BinaryOperation
    {
        Assignment,
        Addition,
        Subraction,
        Multiplication,
        Division,
        Modulus,
        Power,
        IntegerDivision,
        BitshiftLeft,
        BitshiftRight,
        EqualTo,
        NotEqualTo,
        GreaterThan,
        GreaterThanOrEqual,
        LesserThan,
        LesserThanOrEqual,
        LogicalAnd,
        LogicalOr,
        BitwiseAnd,
        BitwiseOr
    }
}


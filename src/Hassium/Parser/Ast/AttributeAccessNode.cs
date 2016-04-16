using System;

namespace Hassium.Parser
{
    public class AttributeAccessNode: AstNode
    {
        public AstNode Left { get { return Children[0]; } }
        public string Right { get; private set; }

        public AttributeAccessNode(AstNode left, string right)
        {
            Children.Add(left);
            Right = right;
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
}


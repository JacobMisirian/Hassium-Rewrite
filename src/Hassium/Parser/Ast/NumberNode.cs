using System;

namespace Hassium.Parser
{
    public class NumberNode: AstNode
    {
        public Double Number { get; private set; }
        public NumberNode(string value)
        {
            Number = Convert.ToDouble(value);
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


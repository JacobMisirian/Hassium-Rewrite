using System;

namespace Hassium.Parser
{
    public class IdentifierNode: AstNode
    {
        public string Identifier { get; private set; }
        public IdentifierNode(string identifier)
        {
            Identifier = identifier;
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


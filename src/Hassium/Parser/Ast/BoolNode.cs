using System;

namespace Hassium.Parser
{
    public class BoolNode: AstNode
    {
        public bool Value { get; private set; }
        public BoolNode(string boolString)
        {
            Value = boolString.ToUpper() == "TRUE";
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


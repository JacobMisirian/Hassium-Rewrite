using System;

namespace Hassium.Parser
{
    public class FunctionCallNode: AstNode
    {
        public AstNode Target { get { return Children[0]; } }
        public AstNode Arguments { get { return Children[1]; } }

        public FunctionCallNode(AstNode target, ArgListNode arguments)
        {
            Children.Add(target);
            Children.Add(arguments);
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


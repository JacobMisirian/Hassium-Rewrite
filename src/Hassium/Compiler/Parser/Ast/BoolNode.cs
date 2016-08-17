using System;

namespace Hassium.Compiler.Parser.Ast
{
    public class BoolNode: AstNode
    {
        public bool Bool { get; private set; }

        public BoolNode(SourceLocation location, bool val)
        {
            this.SourceLocation = location;
            Bool = val;
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


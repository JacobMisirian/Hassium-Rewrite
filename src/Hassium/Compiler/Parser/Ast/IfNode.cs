using System;

namespace Hassium.Compiler.Parser.Ast
{
    public class IfNode: AstNode
    {
        public AstNode Condition { get { return Children[0]; } }
        public AstNode Body { get { return Children[1]; } }
        public AstNode ElseBody { get { return Children[2]; } }

        public IfNode(SourceLocation location, AstNode condition, AstNode body)
        {
            this.SourceLocation = location;
            Children.Add(condition);
            Children.Add(body);
        }
        public IfNode(SourceLocation location, AstNode condition, AstNode body, AstNode elseBody)
        {
            this.SourceLocation = location;
            Children.Add(condition);
            Children.Add(body);
            Children.Add(elseBody);
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


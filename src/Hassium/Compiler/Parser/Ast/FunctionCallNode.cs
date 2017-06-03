using System;

namespace Hassium.Compiler.Parser.Ast
{
    public class FunctionCallNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public AstNode Target { get; private set; }
        public ArgumentListNode Parameters { get; private set; }

        public FunctionCallNode(SourceLocation location, AstNode target, ArgumentListNode parameters)
        {
            SourceLocation = location;

            Target = target;
            Parameters = parameters;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            Target.Visit(visitor);
            Parameters.Visit(visitor);
        }
    }
}

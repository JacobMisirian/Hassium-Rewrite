using System;
using System.Collections.Generic;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class ArgListNode: AstNode
    {
        public ArgListNode(List<AstNode> nodes)
        {
            foreach (AstNode child in nodes)
                Children.Add(child);
        }

        public static ArgListNode Parse(Parser parser)
        {
            List<AstNode> nodes = new List<AstNode>();
            nodes.Add(ExpressionNode.Parse(parser));
            while (parser.AcceptToken(TokenType.Comma))
                nodes.Add(ExpressionNode.Parse(parser));
            return new ArgListNode(nodes);
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


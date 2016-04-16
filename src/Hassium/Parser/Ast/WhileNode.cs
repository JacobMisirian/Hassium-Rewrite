using System;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class WhileNode: AstNode
    {
        public AstNode Predicate { get { return Children[0]; } }
        public AstNode Body { get { return Children[1]; } }
        public WhileNode(AstNode predicate, AstNode body)
        {
            Children.Add(predicate);
            Children.Add(body);
        }

        public static WhileNode Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Identifier, "while");
            parser.ExpectToken(TokenType.LeftParentheses);
            AstNode predicate = ExpressionNode.Parse(parser);
            parser.ExpectToken(TokenType.RightParentheses);
            AstNode body = StatementNode.Parse(parser);

            return new WhileNode(predicate, body);
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


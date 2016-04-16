using System;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class StatementNode: AstNode
    {
        public static AstNode Parse(Parser parser)
        {
            if (parser.MatchToken(TokenType.Identifier, "func"))
                return FuncNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "if"))
                return ConditionalNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "while"))
                return WhileNode.Parse(parser);
            else
                return ExpressionNode.Parse(parser);
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
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
            else if (parser.MatchToken(TokenType.Identifier, "class"))
                return ClassNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "if"))
                return ConditionalNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "while"))
                return WhileNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "for"))
                return ForNode.Parse(parser);
            else if (parser.AcceptToken(TokenType.Identifier, "break"))
                return new BreakNode();
            else if (parser.AcceptToken(TokenType.Identifier, "continue"))
                return new ContinueNode();
            else if (parser.MatchToken(TokenType.Identifier) && parser.GetToken(1).TokenType == TokenType.LeftBrace)
                return PropertyNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "return"))
                return ReturnNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "use"))
                return UseNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Identifier, "enum"))
                return EnumNode.Parse(parser);
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
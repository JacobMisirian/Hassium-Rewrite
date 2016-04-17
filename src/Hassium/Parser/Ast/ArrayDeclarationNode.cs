using System;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class ArrayDeclarationNode: AstNode
    {
        public static ArrayDeclarationNode Parse(Parser parser)
        {
            ArrayDeclarationNode ret = new ArrayDeclarationNode();
            parser.ExpectToken(TokenType.LeftSquare);
            ret.Children.Add(ExpressionNode.Parse(parser));
            while (parser.AcceptToken(TokenType.Comma))
                ret.Children.Add(ExpressionNode.Parse(parser));
            parser.ExpectToken(TokenType.RightSquare);

            return ret;
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


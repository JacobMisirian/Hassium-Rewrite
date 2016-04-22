using System;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class ClassNode: AstNode
    {
        public string Name { get; private set; }
        public AstNode Body { get { return Children[0]; } }
        public ClassNode(string name, AstNode body, SourceLocation location)
        {
            Name = name;
            Children.Add(body);
            this.SourceLocation = location;
        }

        public static ClassNode Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Identifier, "class");
            string name = parser.ExpectToken(TokenType.Identifier).Value;
            AstNode body = StatementNode.Parse(parser);

            return new ClassNode(name, body, parser.Location);
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


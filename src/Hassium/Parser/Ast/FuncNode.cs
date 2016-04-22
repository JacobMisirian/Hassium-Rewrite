using System;
using System.Collections.Generic;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class FuncNode: AstNode
    {
        public string Name { get; private set; }
        public List<string> Parameters { get; private set; }

        public FuncNode(string name, List<string> parameters, AstNode body, SourceLocation location)
        {
            Name = name;
            Parameters = parameters;
            Children.Add(body);
            this.SourceLocation = location;
        }

        public static FuncNode Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Identifier, "func");
            string name = parser.ExpectToken(TokenType.Identifier).Value;
            List<string> parameters = new List<string>();
            ArgListNode args = ArgListNode.Parse(parser);
            foreach (AstNode child in args.Children)
                parameters.Add(((IdentifierNode)child).Identifier);
            AstNode body = StatementNode.Parse(parser);

            return new FuncNode(name, parameters, body, parser.Location);
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


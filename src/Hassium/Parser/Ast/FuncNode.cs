using System;
using System.Collections.Generic;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class FuncNode: AstNode
    {
        public string Name { get; private set; }
        public List<string> Parameters { get; private set; }
        public FuncNode(string name, List<string> parameters, AstNode body)
        {
            Name = name;
            Parameters = parameters;
            Children.Add(body);
        }

        public static FuncNode Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Identifier, "func");
            string name = parser.ExpectToken(TokenType.Identifier).Value;
            parser.ExpectToken(TokenType.LeftParentheses);
            List<string> parameters = new List<string>();
            if (!parser.MatchToken(TokenType.RightParentheses))
                while (parser.AcceptToken(TokenType.Comma))
                    parameters.Add(parser.ExpectToken(TokenType.Identifier).Value);
            parser.ExpectToken(TokenType.RightParentheses);
            AstNode body = StatementNode.Parse(parser);

            return new FuncNode(name, parameters, body);
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


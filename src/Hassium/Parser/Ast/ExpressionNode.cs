using System;

using Hassium.Lexer;

namespace Hassium.Parser
{
    public class ExpressionNode: AstNode
    {
        public static AstNode Parse(Parser parser)
        {
            return parseAssignment(parser);
        }

        private static AstNode parseAssignment(Parser parser)
        {
            AstNode left = parseAdditive(parser);

            if (parser.AcceptToken(TokenType.Assignment))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, parseAssignment(parser));
            else
                return left;
        }

        private static AstNode parseAdditive(Parser parser)
        {
            AstNode left = parseMultiplicitive(parser);
            while (parser.MatchToken(TokenType.BinaryOperation))
            {
                switch (parser.GetToken().Value)
                {
                    case "+":
                        parser.AcceptToken(TokenType.BinaryOperation);
                        left = new BinaryOperationNode(BinaryOperation.Addition, left, parseMultiplicitive(parser));
                        continue;
                    case "-":
                        parser.AcceptToken(TokenType.BinaryOperation);
                        left = new BinaryOperationNode(BinaryOperation.Subtraction, left, parseMultiplicitive(parser));
                        continue;
                    default:
                        break;
                }
                break;
            }
            return left;
        }

        private static AstNode parseMultiplicitive(Parser parser)
        {
            AstNode left = parseUnary(parser);
            while (parser.MatchToken(TokenType.BinaryOperation))
            {
                switch (parser.GetToken().Value)
                {
                    case "*":
                        parser.AcceptToken(TokenType.BinaryOperation);
                        left = new BinaryOperationNode(BinaryOperation.Multiplication, left, parseUnary(parser));
                        continue;
                    case "/":
                        parser.AcceptToken(TokenType.BinaryOperation);
                        left = new BinaryOperationNode(BinaryOperation.Division, left, parseUnary(parser));
                        continue;
                    case "%":
                        parser.AcceptToken(TokenType.BinaryOperation);
                        left = new BinaryOperationNode(BinaryOperation.Modulus, left, parseUnary(parser));
                        continue;
                    case "=":
                        parser.AcceptToken(TokenType.Assignment);
                        left = new BinaryOperationNode(BinaryOperation.Assignment, left, parseUnary(parser));
                        continue;
                     default:
                        break;
                }
                break;
            }
            return left;
        }

        private static AstNode parseUnary(Parser parser)
        {
            if (parser.MatchToken(TokenType.UnaryOperation))
            {
                switch (parser.GetToken().Value)
                {
                    case "!":
                        parser.ExpectToken(TokenType.UnaryOperation);
                        return new UnaryOperationNode(UnaryOperation.Not, parseUnary(parser));
                    case "++":
                        parser.ExpectToken(TokenType.UnaryOperation);
                        return new UnaryOperationNode(UnaryOperation.Increment, parseUnary(parser));
                    case "--":
                        parser.ExpectToken(TokenType.UnaryOperation);
                        return new UnaryOperationNode(UnaryOperation.Decrement, parseUnary(parser));
                }
            }
            return parseComparison(parser);
        }

        private static AstNode parseComparison(Parser parser)
        {
            AstNode left = parseFunctionCall(parser);
            while (parser.MatchToken(TokenType.Comparison))
            {
                switch (parser.GetToken().Value)
                {
                    case ">":
                        parser.AcceptToken(TokenType.Comparison);
                        left = new BinaryOperationNode(BinaryOperation.GreaterThan, left, parseFunctionCall(parser));
                        continue;
                    case "<":
                        parser.AcceptToken(TokenType.Comparison);
                        left = new BinaryOperationNode(BinaryOperation.LesserThan, left, parseFunctionCall(parser));
                        continue;
                    case ">=":
                        parser.AcceptToken(TokenType.Comparison);
                        left = new BinaryOperationNode(BinaryOperation.GreaterThanOrEqual, left, parseFunctionCall(parser));
                        continue;
                    case "<=":
                        parser.AcceptToken(TokenType.Comparison);
                        left = new BinaryOperationNode(BinaryOperation.LesserThanOrEqual, left, parseFunctionCall(parser));
                        continue;
                    case "!=":
                        parser.AcceptToken(TokenType.Comparison);
                        left = new BinaryOperationNode(BinaryOperation.NotEqualTo, left, parseFunctionCall(parser));
                        continue;
                    case "==":
                        parser.AcceptToken(TokenType.Comparison);
                        left = new BinaryOperationNode(BinaryOperation.EqualTo, left, parseFunctionCall(parser));
                        continue;
                    default:
                        break;
                }
                break;
            }
            return left;
        }

        private static AstNode parseFunctionCall(Parser parser)
        {
            return parseFunctionCall(parser, parseAttributeAccess(parser));
        }
        private static AstNode parseFunctionCall(Parser parser, AstNode left)
        {
            if (parser.MatchToken(TokenType.LeftParentheses))
                return parseFunctionCall(parser, new FunctionCallNode(left, ArgListNode.Parse(parser)));
            else
                return left;
        }

        private static AstNode parseAttributeAccess(Parser parser)
        {
            AstNode left = parseTerm(parser);
            while (parser.MatchToken(TokenType.BinaryOperation))
            {
                switch (parser.GetToken().Value)
                {
                    case ".":
                        parser.ExpectToken(TokenType.BinaryOperation);
                        left = new AttributeAccessNode(left, parser.ExpectToken(TokenType.Identifier).Value);
                        continue;
                    default:
                        break;
                }
                break;
            }
            return left;
        }

        private static AstNode parseTerm(Parser parser)
        {
            if (parser.AcceptToken(TokenType.Identifier, "new"))
                return new NewNode((FunctionCallNode)parseFunctionCall(parser));
            else if (parser.AcceptToken(TokenType.Identifier, "this"))
                return new ThisNode();
            else if (parser.MatchToken(TokenType.Identifier, "true") || parser.MatchToken(TokenType.Identifier, "false"))
                return new BoolNode(parser.ExpectToken(TokenType.Identifier).Value);
            else if (parser.MatchToken(TokenType.Identifier))
                return new IdentifierNode(parser.ExpectToken(TokenType.Identifier).Value);
            else if (parser.MatchToken(TokenType.Number))
                return new NumberNode(parser.ExpectToken(TokenType.Number).Value);
            else if (parser.MatchToken(TokenType.String))
                return new StringNode(parser.ExpectToken(TokenType.String).Value);
            else if (parser.MatchToken(TokenType.Char))
                return new CharNode(parser.ExpectToken(TokenType.Char).Value);
            else if (parser.AcceptToken(TokenType.LeftParentheses))
            {
                AstNode expression = Parse(parser);
                parser.ExpectToken(TokenType.RightParentheses);
                return expression;
            }
            else if (parser.AcceptToken(TokenType.LeftBrace))
            {
                CodeBlockNode block = new CodeBlockNode();
                while (!parser.AcceptToken(TokenType.RightBrace))
                {
                    block.Children.Add(StatementNode.Parse(parser));
                    parser.AcceptToken(TokenType.Semicolon);
                }
                return block;
            }
            else if (parser.MatchToken(TokenType.LeftSquare))
                return ArrayDeclarationNode.Parse(parser);
            else if (parser.AcceptToken(TokenType.Semicolon))
                return new StatementNode();
            else
                throw new Exception(string.Format("Unexpected type {0} with value {1} encountered in parser!", parser.GetToken().TokenType, parser.GetToken().Value));
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


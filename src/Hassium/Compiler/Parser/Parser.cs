using System;
using System.Collections.Generic;

using Hassium.Compiler;
using Hassium.Compiler.Parser.Ast;
using Hassium.Compiler.Scanner;

namespace Hassium.Compiler.Parser
{
    public class Parser
    {
        public List<Token> Tokens { get; private set; }
        public int Position { get; set; }
        public bool EndOfStream { get { return Position >= Tokens.Count; } }
        public SourceLocation Location { get { return Tokens[Position].SourceLocation; } }

        public AstNode Parse()
        {
            CodeBlockNode code = new CodeBlockNode();
            while (!EndOfStream)
                code.Children.Add(parseStatement());
            return code;
        }

        private AstNode parseStatement()
        {
            if (MatchToken(TokenType.Identifier, "if"))
                return parseIf();
            else
                return parseExpression();
        }
        private IfNode parseIf()
        {
            ExpectToken(TokenType.Identifier, "if");
            ExpectToken(TokenType.OpenParentheses);
            AstNode expression = parseExpression();
            ExpectToken(TokenType.CloseParentheses);
            AstNode body = parseStatement();
            AstNode elseBody = null;
            if (AcceptToken(TokenType.Identifier, "else"))
                elseBody = parseStatement();
            return elseBody == null ? new IfNode(Location, expression, body) : new IfNode(Location, expression, body, elseBody);
        }
        private ArgumentListNode parseArgList()
        {
            ExpectToken(TokenType.OpenParentheses);
            List<AstNode> elements = new List<AstNode>();
            while (true)
            {
                elements.Add(parseExpression());
                if (!AcceptToken(TokenType.Comma))
                    break;
            }
            ExpectToken(TokenType.CloseParentheses);
            return new ArgumentListNode(Location, elements);
        }

        private AstNode parseExpression()
        {
            return parseAssignment();
        }
        private AstNode parseAssignment()
        {
            AstNode left = parseLogicalOr();
            if (MatchToken(TokenType.Assignment))
            {
                switch (Tokens[Position].Value)
                {
                    case "=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, parseAssignment());
                    case "+=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.Addition, left, parseAssignment()));
                    case "-=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.Subraction, left, parseAssignment()));
                    case "*=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.Multiplication, left, parseAssignment()));
                    case "/=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.Division, left, parseAssignment()));
                    case "%=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.Modulus, left, parseAssignment()));
                    case "<<=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.BitshiftLeft, left, parseAssignment()));
                    case ">>=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.BitshiftRight, left, parseAssignment()));
                    case "&=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.LogicalAnd, left, parseAssignment()));
                    case "|=":
                        AcceptToken(TokenType.Assignment);
                        return new BinaryOperationNode(Location, BinaryOperation.Assignment, left, new BinaryOperationNode(Location, BinaryOperation.LogicalOr, left, parseAssignment()));
                    default:
                        break;
                }
            }
            return left;
        }
        private AstNode parseLogicalOr()
        {
            AstNode left = parseEquality();
            while (AcceptToken(TokenType.Operation, "||"))
                left = new BinaryOperationNode(Location, BinaryOperation.LogicalOr, left, parseLogicalOr());
            return left;
        }
        private AstNode parseLogicalAnd()
        {
            AstNode left = parseEquality();
            while (AcceptToken(TokenType.Operation, "&&"))
                left = new BinaryOperationNode(Location, BinaryOperation.LogicalAnd, left, parseLogicalAnd());
            return left;
        }
        private AstNode parseEquality()
        {
            AstNode left = parseOr();
            while (MatchToken(TokenType.Comparison))
            {
                switch (Tokens[Position].Value)
                {
                    case "==":
                        AcceptToken(TokenType.Comparison);
                        return new BinaryOperationNode(Location, BinaryOperation.EqualTo, left, parseEquality());
                    case "!=":
                        AcceptToken(TokenType.Comparison);
                        return new BinaryOperationNode(Location, BinaryOperation.NotEqualTo, left, parseEquality());
                    default:
                        break;
                }
                break;
            }
            return left;
        }
        private AstNode parseOr()
        {
            AstNode left = parseAnd();
            while (AcceptToken(TokenType.Operation, "|"))
                left = new BinaryOperationNode(Location, BinaryOperation.BitwiseOr, left, parseOr());
            return left;
        }
        private AstNode parseAnd()
        {
            AstNode left = parseBitshift();
            while (AcceptToken(TokenType.Operation, "&"))
                left = new BinaryOperationNode(Location, BinaryOperation.BitwiseAnd, left, parseAnd());
            return left;
        }
        private AstNode parseBitshift()
        {
            AstNode left = parseAdditive();
            while (MatchToken(TokenType.Operation))
            {
                switch (Tokens[Position].Value)
                {
                    case "<<":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.BitshiftLeft, left, parseBitshift());
                    case ">>":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.BitshiftRight, left, parseBitshift());
                    default:
                        break;
                }
                break;
            }
            return left;
        }
        private AstNode parseAdditive()
        {
            AstNode left = parseMultiplicative();
            while (MatchToken(TokenType.Operation))
            {
                switch (Tokens[Position].Value)
                {
                    case "+":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.Addition, left, parseAdditive());
                    case "-":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.Subraction, left, parseAdditive());
                    default:
                        break;
                }
                break;
            }
            return left;
        }
        private AstNode parseMultiplicative()
        {
            AstNode left = parseUnary();
            while (MatchToken(TokenType.Operation))
            {
                switch (Tokens[Position].Value)
                {
                    case "*":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.Multiplication, left, parseMultiplicative());
                    case "/":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.Division, left, parseMultiplicative());
                    case "%":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.Modulus, left, parseMultiplicative());
                    case "**":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.Power, left, parseMultiplicative());
                    case "//":
                        AcceptToken(TokenType.Operation);
                        return new BinaryOperationNode(Location, BinaryOperation.IntegerDivision, left, parseMultiplicative());
                    default:
                        break;
                }
                break;
            }
            return left;
        }
        private AstNode parseUnary()
        {
            if (MatchToken(TokenType.Operation))
            {
                switch (Tokens[Position].Value)
                {
                    case "!":
                        AcceptToken(TokenType.Operation);
                        return new UnaryOperationNode(Location, parseUnary(), UnaryOperation.LogicalNot);
                    case "~":
                        AcceptToken(TokenType.Operation);
                        return new UnaryOperationNode(Location, parseUnary(), UnaryOperation.BitwiseNot);
                    case "--":
                        AcceptToken(TokenType.Operation);
                        return new UnaryOperationNode(Location, parseUnary(), UnaryOperation.PreDecrement);
                    case "++":
                        AcceptToken(TokenType.Operation);
                        return new UnaryOperationNode(Location, parseUnary(), UnaryOperation.PreIncrement);
                    case "-":
                        AcceptToken(TokenType.Operation);
                        return new UnaryOperationNode(Location, parseUnary(), UnaryOperation.Negate);
                }
            }
            return parseAccess();
        }
        private AstNode parseAccess()
        {
            return parseAccess(parseTerm());
        }
        private AstNode parseAccess(AstNode left)
        {
            if (MatchToken(TokenType.OpenParentheses))
                return parseAccess(new FunctionCallNode(Location, left, parseArgList()));
            else if (AcceptToken(TokenType.OpenSquare))
            {
                AstNode expression = parseExpression();
                ExpectToken(TokenType.CloseSquare);
                return parseAccess(new ListAccessNode(Location, left, expression));
            }
            else if (AcceptToken(TokenType.Operation, "--"))
                return new UnaryOperationNode(Location, left, UnaryOperation.PostDecrement);
            else if (AcceptToken(TokenType.Operation, "++"))
                return new UnaryOperationNode(Location, left, UnaryOperation.PostIncrement);
            else if (AcceptToken(TokenType.Dot))
                return parseAccess(new AttributeAccessNode(Location, left, ExpectToken(TokenType.Identifier).Value));
            else
                return left;
        }
        private AstNode parseTerm()
        {
            if (MatchToken(TokenType.Identifier))
                return new IdentifierNode(Location, ExpectToken(TokenType.Identifier).Value);
            else if (MatchToken(TokenType.Integer))
                return new IntegerNode(Location, Convert.ToInt64(ExpectToken(TokenType.Integer).Value));
            else if (MatchToken(TokenType.Float))
                return new FloatNode(Location, Convert.ToDouble(ExpectToken(TokenType.Float).Value));
            else if (MatchToken(TokenType.Char))
                return new CharNode(Location, Convert.ToChar(ExpectToken(TokenType.Char).Value));
            else if (AcceptToken(TokenType.Semicolon))
                return new StatementNode(Location);
            else
                throw new UnexpectedTokenException(Tokens[Position]);
        }

        public bool MatchToken(TokenType tokenType)
        {
            return !EndOfStream && Tokens[Position].TokenType == tokenType;
        }
        public bool MatchToken(TokenType tokenType, string value)
        {
            return !EndOfStream && Tokens[Position].TokenType == tokenType && Tokens[Position].Value == value;
        }

        public bool AcceptToken(TokenType tokenType)
        {
            bool ret = MatchToken(tokenType);
            if (ret)
                Position++;
            return ret;
        }
        public bool AcceptToken(TokenType tokenType, string value)
        {
            bool ret = MatchToken(tokenType, value);
            if (ret)
                Position++;
            return ret;
        }

        public Token ExpectToken(TokenType tokenType)
        {
            if (MatchToken(tokenType))
                return Tokens[Position++];
            throw new ExpectedTokenException(tokenType, tokenType.ToString(), Tokens[Position]);
        }
        public Token ExpectToken(TokenType tokenType, string value)
        {
            if (MatchToken(tokenType, value))
                return Tokens[Position++];
            throw new ExpectedTokenException(tokenType, value, Tokens[Position]);
        }
    }
}


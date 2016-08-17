﻿using System;
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

        public AstNode Parse(List<Token> tokens)
        {
            Tokens = tokens;
            Position = 0;
            CodeBlockNode code = new CodeBlockNode();
            while (!EndOfStream)
                code.Children.Add(parseStatement());
            return code;
        }

        private AstNode parseStatement()
        {
            if (MatchToken(TokenType.Identifier, "class"))
                return parseClass();
            else if (MatchToken(TokenType.Identifier, "func"))
                return parseFunc();
            else if (MatchToken(TokenType.Identifier, "if"))
                return parseIf();
            else if (MatchToken(TokenType.Identifier, "while"))
                return parseWhile();
            else
                return parseExpression();
        }
        private ArgumentListNode parseArgList()
        {
            ExpectToken(TokenType.OpenParentheses);
            List<AstNode> elements = new List<AstNode>();
            while (!AcceptToken(TokenType.CloseParentheses))
            {
                elements.Add(parseExpression());
                AcceptToken(TokenType.Comma);
            }
            return new ArgumentListNode(Location, elements);
        }
        private ClassNode parseClass()
        {
            ExpectToken(TokenType.Identifier, "class");
            string name = ExpectToken(TokenType.Identifier).Value;
            List<string> inherits = new List<string>();
            if (AcceptToken(TokenType.Colon))
            {
                inherits.Add(ExpectToken(TokenType.Identifier).Value);
                while (AcceptToken(TokenType.Comma))
                    inherits.Add(ExpectToken(TokenType.Identifier).Value);
            }
            AstNode body = parseStatement();

            return new ClassNode(Location, name, inherits, body);
        }
        private FuncNode parseFunc()
        {
            ExpectToken(TokenType.Identifier, "func");
            string name = ExpectToken(TokenType.Identifier).Value;
            List<FuncParameter> parameters = new List<FuncParameter>();
            ExpectToken(TokenType.OpenParentheses);
            while (!AcceptToken(TokenType.CloseParentheses))
            {
                parameters.Add(parseParameter());
                AcceptToken(TokenType.CloseParentheses);
            }
            if (AcceptToken(TokenType.Colon))
            {
                string returnType = ExpectToken(TokenType.Identifier).Value;
                return new FuncNode(Location, name, parameters, parseStatement(), returnType);
            }
            return new FuncNode(Location, name, parameters, parseStatement());
        }
        private IfNode parseIf()
        {
            ExpectToken(TokenType.Identifier, "if");
            ExpectToken(TokenType.OpenParentheses);
            AstNode predicate = parseExpression();
            ExpectToken(TokenType.CloseParentheses);
            AstNode body = parseStatement();
            if (AcceptToken(TokenType.Identifier, "else"))
                return new IfNode(Location, predicate, body, parseStatement());
            return new IfNode(Location, predicate, body);
        }
        private ListDeclarationNode parseListDeclaration()
        {
            ExpectToken(TokenType.OpenSquare);
            List<AstNode> initial = new List<AstNode>();
            while (!AcceptToken(TokenType.CloseSquare))
            {
                initial.Add(parseExpression());
                AcceptToken(TokenType.Comma);
            }
            return new ListDeclarationNode(Location, initial);
        }
        private FuncParameter parseParameter()
        {
            string name = ExpectToken(TokenType.Identifier).Value;
            if (AcceptToken(TokenType.Colon))
                return new FuncParameter(name, ExpectToken(TokenType.Identifier).Value);
            return new FuncParameter(name);
        }
        private WhileNode parseWhile()
        {
            ExpectToken(TokenType.Identifier, "while");
            ExpectToken(TokenType.OpenParentheses);
            AstNode predicate = parseExpression();
            ExpectToken(TokenType.CloseParentheses);
            AstNode body = parseStatement();
            if (AcceptToken(TokenType.Identifier, "else"))
                return new WhileNode(Location, predicate, body, parseStatement());
            return new WhileNode(Location, predicate, body);
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
            AstNode left = parseComparison();
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
        private AstNode parseComparison()
        {
            AstNode left = parseOr();
            while (MatchToken(TokenType.Comparison))
            {
                switch (Tokens[Position].Value)
                {
                    case ">":
                        AcceptToken(TokenType.Comparison);
                        return new BinaryOperationNode(Location, BinaryOperation.GreaterThan, left, parseComparison());
                    case ">=":
                        AcceptToken(TokenType.Comparison);
                        return new BinaryOperationNode(Location, BinaryOperation.GreaterThanOrEqual, left, parseComparison());
                    case "<":
                        AcceptToken(TokenType.Comparison);
                        return new BinaryOperationNode(Location, BinaryOperation.LesserThan, left, parseComparison());
                    case "<=":
                        AcceptToken(TokenType.Comparison);
                        return new BinaryOperationNode(Location, BinaryOperation.LesserThanOrEqual, left, parseComparison());
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
            else if (MatchToken(TokenType.String))
                return new StringNode(Location, ExpectToken(TokenType.String).Value);
            else if (MatchToken(TokenType.Integer))
                return new IntegerNode(Location, Convert.ToInt64(ExpectToken(TokenType.Integer).Value));
            else if (MatchToken(TokenType.Float))
                return new FloatNode(Location, Convert.ToDouble(ExpectToken(TokenType.Float).Value));
            else if (MatchToken(TokenType.Char))
                return new CharNode(Location, Convert.ToChar(ExpectToken(TokenType.Char).Value));
            else if (AcceptToken(TokenType.Semicolon))
                return new StatementNode(Location);
            else if (MatchToken(TokenType.OpenSquare))
                return parseListDeclaration();
            else if (AcceptToken(TokenType.OpenBracket))
            {
                var block = new CodeBlockNode();
                while (!AcceptToken(TokenType.CloseBracket))
                    block.Children.Add(parseStatement());
                return block;
            }
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

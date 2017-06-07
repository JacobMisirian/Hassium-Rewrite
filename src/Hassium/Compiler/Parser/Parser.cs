using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler.Exceptions;
using Hassium.Compiler.Lexer;
using Hassium.Compiler.Parser.Ast;

namespace Hassium.Compiler.Parser
{
    public class Parser
    {
        private List<Token> tokens;
        private int position;
        private bool endOfStream { get { return position >= tokens.Count; } }

        private SourceLocation location
        {
            get
            {
                try
                {
                    return tokens[position].SourceLocation;
                }
                catch
                {
                    return tokens[position - 1].SourceLocation;
                }
            }
        }

        public AstNode Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            position = 0;

            CodeBlockNode ast = new CodeBlockNode(location);
            while (!endOfStream)
                ast.Children.Add(parseStatement());
            return ast;
        }

        private AstNode parseStatement()
        {
            return parseExpressionStatement();
        }

        private ArgumentListNode parseArgumentList()
        {
            var location = this.location;

            expectToken(TokenType.OpenParentheses);
            List<AstNode> parameters = new List<AstNode>();
            while (!acceptToken(TokenType.CloseParentheses))
            {
                parameters.Add(parseExpression());
                acceptToken(TokenType.Comma);
            }
            expectToken(TokenType.CloseParentheses);

            return new ArgumentListNode(location, parameters);
        }

        private DictionaryDeclarationNode parseDictionaryDeclaration()
        {
            var location = this.location;
            var keys = new List<AstNode>();
            var values = new List<AstNode>();
            expectToken(TokenType.OpenCurlyBrace);
            while (!acceptToken(TokenType.CloseCurlyBrace))
            {
                keys.Add(parseExpression());
                expectToken(TokenType.Colon);
                values.Add(parseExpression());
                acceptToken(TokenType.Comma);
            }
            expectToken(TokenType.CloseCurlyBrace);

            return new DictionaryDeclarationNode(location, keys, values);
        }

        private LambdaNode parseLambda()
        {
            var location = this.location;
            expectToken(TokenType.Identifier, "lambda");
            var parameters = parseArgumentList();
            var body = parseStatement();
            return new LambdaNode(location, parameters, body);
        }

        private ListDeclarationNode parseListDeclaration()
        {
            var location = this.location;
            var elements = new List<AstNode>();
            expectToken(TokenType.OpenSquareBrace);
            while (!acceptToken(TokenType.CloseSquareBrace))
            {
                elements.Add(parseExpression());
                acceptToken(TokenType.Comma);
            }
            expectToken(TokenType.CloseSquareBrace);

            return new ListDeclarationNode(location, elements);
        }

        private TupleNode parseTuple(AstNode init)
        {
            var location = this.location;
            var elements = new List<AstNode>();
            elements.Add(init);
            acceptToken(TokenType.Comma);
            while (!acceptToken(TokenType.CloseParentheses))
            {
                elements.Add(parseExpression());
                acceptToken(TokenType.Comma);
            }
            expectToken(TokenType.CloseParentheses);

            return new TupleNode(location, elements);
        }

        private AstNode parseExpressionStatement()
        {
            return parseExpression();
        }

        private AstNode parseExpression()
        {
            return parseTerm();
        }

        private AstNode parseTerm()
        {
            var location = this.location;

            if (matchToken(TokenType.Char))
                return new CharNode(location, expectToken(TokenType.Char).Value);
            else if (matchToken(TokenType.Float))
                return new FloatNode(location, expectToken(TokenType.Float).Value);
            else if (matchToken(TokenType.Integer))
                return new IntegerNode(location, expectToken(TokenType.Integer).Value);
            else if (matchToken(TokenType.String))
                return new StringNode(location, expectToken(TokenType.String).Value);
            else if (matchToken(TokenType.OpenCurlyBrace))
                return parseDictionaryDeclaration();
            else if (acceptToken(TokenType.OpenParentheses))
            {
                var expr = parseExpression();
                if (matchToken(TokenType.Comma))
                    return parseTuple(expr);
                expectToken(TokenType.CloseParentheses);
                return expr;
            }
            else if (matchToken(TokenType.OpenSquareBrace))
                return parseListDeclaration();
            else if (acceptToken(TokenType.Semicolon))
                return new CodeBlockNode(location);
            else if (matchToken(TokenType.Identifier, "lambda"))
                return parseLambda();
            else if (acceptToken(TokenType.Identifier, "new"))
                return parseExpression();
            else if (matchToken(TokenType.Identifier))
                return new IdentifierNode(location, expectToken(TokenType.Identifier).Value);
            throw new ParserException(location, "Unexpected token of type '{0}' with value '{1}'", tokens[position].TokenType, tokens[position].Value);
        }

        private bool matchToken(TokenType tokenType)
        {
            return !endOfStream && tokens[position].TokenType == tokenType;
        }
        private bool matchToken(TokenType tokenType, string value)
        {
            return !endOfStream && tokens[position].TokenType == tokenType && tokens[position].Value == value;
        }

        private bool acceptToken(TokenType tokenType)
        {
            bool ret = matchToken(tokenType);
            if (ret)
                position++;
            return ret;
        }
        private bool acceptToken(TokenType tokenType, string value)
        {
            bool ret = matchToken(tokenType, value);
            if (ret)
                position++;
            return ret;
        }

        private Token expectToken(TokenType tokenType)
        {
            if (matchToken(tokenType))
                return tokens[position++];
            throw new ParserException(location, "Expected token of type '{0}', got token of type '{1}' with value '{2}'", tokenType, tokens[position].TokenType, tokens[position].Value);
        }
        private Token expectToken(TokenType tokenType, string value)
        {
            if (matchToken(tokenType, value))
                return tokens[position++];
            throw new ParserException(location, "Expected token of type '{0}' with value '{1}', got token of type '{2}' with value '{3}'", tokenType, value, tokens[position].TokenType, tokens[position].Value);
        }

        private BinaryOperation stringToBinaryOperation(string operation)
        {
            switch (operation)
            {
                case ">":
                    return BinaryOperation.GreaterThan;
                case ">=":
                    return BinaryOperation.GreaterThanOrEqual;
                case "<":
                    return BinaryOperation.LesserThan;
                case "<=":
                    return BinaryOperation.LesserThanOrEqual;
                case "!=":
                    return BinaryOperation.NotEqualTo;
                default:
                    return BinaryOperation.EqualTo;
            }
        }
    }
}

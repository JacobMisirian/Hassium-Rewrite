using System;
using System.Collections.Generic;
using System.Text;

namespace Hassium.Compiler.Scanner
{
    public class Lexer
    {
        private SourceLocation location;
        private string code;
        private int position;
        private List<Token> result;

        public List<Token> Scan(string source)
        {
            location = new SourceLocation(0, 0);
            code = source;
            position = 0;
            result = new List<Token>();
            char c;

            while (peekChar() != -1)
            {
                whiteSpace();
                if (char.IsLetter((char)peekChar()))
                    scanIdentifier();
                else if (char.IsDigit((char)peekChar()))
                    scanNumber();
                else
                {
                    switch ((char)peekChar())
                    {
                        case '\"':
                            scanString();
                            break;
                        case '\'':
                            readChar();
                            add(TokenType.Char, ((char)readChar()).ToString());
                            readChar();
                            break;
                        case '(':
                            add(TokenType.OpenParentheses, ((char)readChar()).ToString());
                            break;
                        case ')':
                            add(TokenType.CloseParentheses, ((char)readChar()).ToString());
                            break;
                        case '{':
                            add(TokenType.OpenBracket, ((char)readChar()).ToString());
                            break;
                        case '}':
                            add(TokenType.CloseBracket, ((char)readChar()).ToString());
                            break;
                        case '[':
                            add(TokenType.OpenSquare, ((char)readChar()).ToString());
                            break;
                        case ']':
                            add(TokenType.CloseSquare, ((char)readChar()).ToString());
                            break;
                        case ',':
                            add(TokenType.Comma, ((char)readChar()).ToString());
                            break;
                        case '.':
                            add(TokenType.Dot, ((char)readChar()).ToString());
                            break;
                        case ':':
                            add(TokenType.Colon, ((char)readChar()).ToString());
                            break;
                        case ';':
                            add(TokenType.Semicolon, ((char)readChar()).ToString());
                            break;
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                        case '%':
                            c = (char)readChar();
                            if ((char)peekChar() == c)
                                add(TokenType.Operation, c + "" + (char)readChar());
                            else if ((char)peekChar() == '=')
                                add(TokenType.Assignment, c + "" + (char)readChar());
                            else
                                add(TokenType.Operation, c.ToString());
                            break;
                        case '>':
                        case '<':
                            c = (char)readChar();
                            if ((char)peekChar() == '=')
                                add(TokenType.Comparison, c + "" + (char)readChar());
                            else if ((char)peekChar() == c)
                                add(TokenType.Operation, c + "" + (char)readChar());
                            else
                                add(TokenType.Comparison, c.ToString());
                            break;
                        case '!':
                            readChar();
                            if ((char)peekChar() == '=')
                                add(TokenType.Comparison, "!" + (char)readChar());
                            else
                                add(TokenType.Operation, "!");
                            break;
                        case '&':
                            c = (char)readChar();
                            if ((char)peekChar() == c)
                                add(TokenType.Operation, c + "" + (char)readChar());
                            else
                                add(TokenType.Operation, c.ToString());
                            break;
                        case '|':
                            c = (char)readChar();
                            if ((char)peekChar() == c)
                                add(TokenType.Operation, c + "" + (char)readChar());
                            else
                                add(TokenType.Operation, c.ToString());
                            break;
                        case '=':
                            readChar();
                            if ((char)peekChar() == '=')
                                add(TokenType.Comparison, "=" + (char)readChar());
                            else
                                add(TokenType.Assignment, "=");
                            break;
                        default:
                            if (peekChar() == -1)
                                break;
                            Console.WriteLine("Unknown char, {0}, {1}!", peekChar(), (char)readChar());
                            break;
                    }
                }
            }
            return result;
        }

        private void scanNumber()
        {
            StringBuilder sb = new StringBuilder();
            while ((char.IsDigit((char)peekChar()) || (char)peekChar() == '.') && peekChar() != -1)
                sb.Append((char)readChar());
            try
            {
                add(TokenType.Integer, Convert.ToInt64(sb.ToString()).ToString());
            }
            catch
            {
                add(TokenType.Float, Convert.ToInt64(sb.ToString()).ToString());
            }
        }

        private void scanIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            while ((char.IsLetterOrDigit((char)peekChar()) || (char)peekChar() == '_') && peekChar() != -1)
                sb.Append((char)readChar());
            add(TokenType.Identifier, sb.ToString());
        }

        private void scanString()
        {
            StringBuilder sb = new StringBuilder();
            readChar(); // "
            while ((char)peekChar() != '\"' && peekChar() != -1)
                sb.Append((char)readChar());
            readChar(); // "
            add(TokenType.String, sb.ToString());
        }

        private void whiteSpace()
        {
            while (char.IsWhiteSpace((char)peekChar()))
                readChar();
        }

        private int peekChar(int n = 0)
        {
            return position + n < code.Length ? code[position + n] : -1;
        }
        private int readChar()
        {
            if (peekChar() == '\n')
                location = new SourceLocation(location.Row + 1, 0);
            else
                location = new SourceLocation(location.Row, location.Column + 1);
            return position < code.Length ? code[position++] : -1;
        }

        private void add(TokenType tokenType, string value)
        {
            result.Add(new Token(tokenType, value, location));
        }
    }
}


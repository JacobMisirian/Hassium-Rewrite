using System;
using System.Collections.Generic;

namespace Hassium.Lexer
{
    public class Lexer
    {
        private List<Token> result;
        private int position;
        private string code;
        private SourceLocation location;

        public List<Token> Scan(string source)
        {
            result = new List<Token>();
            position = 0;
            code = source;
            location = new SourceLocation(1, 0);

            whiteSpace();
            while (position < code.Length)
            {
                if (char.IsLetter((char)peekChar()))
                    result.Add(scanIdentifier());
                else if (char.IsDigit((char)peekChar()))
                    result.Add(scanNumber());
                else
                {
                    switch ((char)peekChar())
                    {
                        case '\"':
                            result.Add(scanString());
                            break;
                        case '\'':
                            position++;
                            result.Add(new Token(TokenType.Char, ((char)readChar()).ToString(), location));
                            position++;
                            break;
                        case ';':
                            result.Add(new Token(TokenType.Semicolon, string.Empty, location));
                            position++;
                            break;
                        case ':':
                            result.Add(new Token(TokenType.Colon, string.Empty, location));
                            position++;
                            break;
                        case ',':
                            result.Add(new Token(TokenType.Comma, string.Empty, location));
                            position++;
                            break;
                        case '(':
                            result.Add(new Token(TokenType.LeftParentheses, string.Empty, location));
                            position++;
                            break;
                        case ')':
                            result.Add(new Token(TokenType.RightParentheses, string.Empty, location));
                            position++;
                            break;
                        case '{':
                            result.Add(new Token(TokenType.LeftBrace, string.Empty, location));
                            position++;
                            break;
                        case '}':
                            result.Add(new Token(TokenType.RightBrace, string.Empty, location));
                            position++;
                            break;
                        case '.':
                            result.Add(new Token(TokenType.BinaryOperation, ".", location));
                            position++;
                            break;
                        case '+':
                        case '-':
                            char orig = (char)readChar();
                            if ((char)peekChar() == orig)
                                result.Add(new Token(TokenType.UnaryOperation, orig.ToString() + ((char)readChar()).ToString(), location));
                            else
                                result.Add(new Token(TokenType.BinaryOperation, orig.ToString(), location));
                            break;
                        case '*':
                        case '/':
                        case '%':
                        case '^':
                        case '|':
                        case '&':
                            result.Add(new Token(TokenType.BinaryOperation, ((char)readChar()).ToString(), location));
                            break;
                        case '=':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, "==", location));
                            }
                            else
                                result.Add(new Token(TokenType.Assignment, "=", location));
                            break;
                        case '!':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, "!=", location));
                            }
                            else
                                result.Add(new Token(TokenType.UnaryOperation, "!", location));
                            break;
                        case '<':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, "<=", location));
                            }
                            else
                                result.Add(new Token(TokenType.Comparison, "<", location));
                            break;
                        case '>':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, ">", location));
                            }
                            else
                                result.Add(new Token(TokenType.Comparison, ">=", location));
                            break;
                        case '[':
                            position++;
                            result.Add(new Token(TokenType.LeftSquare, "[", location));
                            break;
                        case ']':
                            position++;
                            result.Add(new Token(TokenType.RightSquare, "]", location));
                            break;
                        default:
                            throw new Exception("Caught unknown char in lexer: " + readChar());
                    }
                }
                whiteSpace();
            }

            return result;
        }

        private void whiteSpace()
        {
            while (char.IsWhiteSpace((char)peekChar()) && peekChar() != -1)
                readChar();
        }

        private Token scanIdentifier()
        {
            string str = "";
            while (char.IsLetterOrDigit((char)peekChar()) || (char)peekChar() == '_' && peekChar() != -1)
                str += (char)readChar();
            return new Token(TokenType.Identifier, str, location);
        }

        private Token scanNumber()
        {
            string data = "";
            while (char.IsDigit((char)peekChar()) || (char)peekChar() == '.' && peekChar() != -1)
                data += (char)readChar();
            try
            {
                return new Token(TokenType.Int64, Convert.ToInt64(data).ToString(), location);
            }
            catch
            {
                return new Token(TokenType.Double, Convert.ToDouble(data).ToString(), location);
            }
        }

        private Token scanString()
        {
            string str = "";
            position++;
            while ((char)peekChar() != '\"' && peekChar() != -1)
                str += (char)readChar();
            position++;

            return new Token(TokenType.String, str, location);
        }

        private int peekChar(int n = 0)
        {
            return position + n < code.Length ? code[position + n] : -1;
        }
        private int readChar()
        {
            if (position >= code.Length)
                return -1;
            if (peekChar() == '\n')
                location = new SourceLocation(location.Line + 1, 0);
            else
                location = new SourceLocation(location.Line, location.Letter + 1);

            return code[position++];
        }
    }
}


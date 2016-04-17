using System;
using System.Collections.Generic;

namespace Hassium.Lexer
{
    public class Lexer
    {
        private List<Token> result;
        private int position;
        private string code;

        public List<Token> Scan(string source)
        {
            result = new List<Token>();
            position = 0;
            code = source;

            whiteSpace();
            while (position < code.Length)
            {
                if (char.IsLetterOrDigit((char)peekChar()))
                    result.Add(scanData());
                else
                {
                    switch ((char)peekChar())
                    {
                        case '\"':
                            result.Add(scanString());
                            break;
                        case '\'':
                            position++;
                            result.Add(new Token(TokenType.Char, ((char)readChar()).ToString()));
                            position++;
                            break;
                        case ';':
                            result.Add(new Token(TokenType.Semicolon, string.Empty));
                            position++;
                            break;
                        case ':':
                            result.Add(new Token(TokenType.Colon, string.Empty));
                            position++;
                            break;
                        case ',':
                            result.Add(new Token(TokenType.Comma, string.Empty));
                            position++;
                            break;
                        case '(':
                            result.Add(new Token(TokenType.LeftParentheses, string.Empty));
                            position++;
                            break;
                        case ')':
                            result.Add(new Token(TokenType.RightParentheses, string.Empty));
                            position++;
                            break;
                        case '{':
                            result.Add(new Token(TokenType.LeftBrace, string.Empty));
                            position++;
                            break;
                        case '}':
                            result.Add(new Token(TokenType.RightBrace, string.Empty));
                            position++;
                            break;
                        case '.':
                            result.Add(new Token(TokenType.BinaryOperation, "."));
                            position++;
                            break;
                        case '+':
                        case '-':
                            char orig = (char)readChar();
                            if ((char)peekChar() == orig)
                                result.Add(new Token(TokenType.UnaryOperation, orig.ToString() + ((char)readChar()).ToString()));
                            else
                                result.Add(new Token(TokenType.BinaryOperation, orig.ToString()));
                            break;
                        case '*':
                        case '/':
                        case '%':
                            result.Add(new Token(TokenType.BinaryOperation, ((char)readChar()).ToString()));
                            break;
                        case '=':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, "=="));
                            }
                            else
                                result.Add(new Token(TokenType.Assignment, "="));
                            break;
                        case '!':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, "!="));
                            }
                            else
                                result.Add(new Token(TokenType.UnaryOperation, "!"));
                            break;
                        case '<':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, "<="));
                            }
                            else
                                result.Add(new Token(TokenType.Comparison, "<"));
                            break;
                        case '>':
                            position++;
                            if ((char)peekChar() == '=')
                            {
                                position++;
                                result.Add(new Token(TokenType.Comparison, ">"));
                            }
                            else
                                result.Add(new Token(TokenType.Comparison, ">="));
                            break;
                        case '[':
                            position++;
                            result.Add(new Token(TokenType.LeftSquare, "["));
                            break;
                        case ']':
                            position++;
                            result.Add(new Token(TokenType.RightSquare, "]"));
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
                position++;
        }

        private Token scanData()
        {
            string data = "";
            while (char.IsLetterOrDigit((char)peekChar()) && peekChar() != -1)
                data += (char)readChar();
            try
            {
                return new Token(TokenType.Number, Convert.ToDouble(data).ToString());
            }
            catch
            {
                return new Token(TokenType.Identifier, data);
            }
        }

        private Token scanString()
        {
            string str = "";
            position++;
            while ((char)peekChar() != '\"' && peekChar() != -1)
                str += (char)readChar();
            position++;

            return new Token(TokenType.String, str);
        }

        private int peekChar(int n = 0)
        {
            return position + n < code.Length ? code[position + n] : -1;
        }
        private int readChar()
        {
            return position < code.Length ? code[position++] : -1;
        }
    }
}


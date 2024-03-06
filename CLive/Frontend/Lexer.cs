using System.Text;

namespace CLive.Frontend;

public class Lexer
{
    public static Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>()
    {
        //["let"] = TokenType.Let,
        ["const"] = TokenType.Const,
        ["int"] = TokenType.ValueType,
        ["float"] = TokenType.ValueType,
        ["string"] = TokenType.ValueType,
        ["class"] = TokenType.Class,
        ["public"] = TokenType.Public,
        ["private"] = TokenType.Private,
        ["new"] = TokenType.New,
    };

    public static List<Token> Tokenize(string sourceCode)
    {
        List<Token> tokens = new List<Token>();

        List<char> src = sourceCode.ToList();

        while (src.Count > 0)
        {
            if (src[0] == '\"') // string token TODO: allow $"{val}" string (i forgor how they are called)
            {
                StringBuilder str = new StringBuilder();
                src.Shift();

                while (src.Count > 0 && src[0] != '\"')
                {
                    str.Append(src.Shift());
                }

                src.Shift();

                if (src.Count == 0)
                {
                    break;
                }

                tokens.Add(new Token(str.ToString(), TokenType.String));
            }
            else if (src[0] == '/' && src[1] == '/') // handle comments
            {
                src.Shift();
                src.Shift();

                while (src.Count > 0 && (src[0] != '\n'))
                {
                    src.Shift();
                }

                if (src.Count == 0)
                    break;
            }
            else if (src[0] == '/' && src[1] == '.')
            {
                src.Shift();
                src.Shift();

                while (src.Count > 0 && (src[0] != '.' || src[1] != '/'))
                {
                    src.Shift();
                }

                if (src.Count > 2)
                {
                    src.Shift();
                    src.Shift();
                }
                else
                {
                    break;
                }
            }

            switch (src[0])
            {
                case '(':
                    tokens.Add(new Token(src.Shift(), TokenType.OpenParen));
                    break;

                case ')':
                    tokens.Add(new Token(src.Shift(), TokenType.CloseParen));
                    break;

                case '{':
                    tokens.Add(new Token(src.Shift(), TokenType.OpenBrace));
                    break;

                case '}':
                    tokens.Add(new Token(src.Shift(), TokenType.CloseBrace));
                    break;

                case '[':
                    tokens.Add(new Token(src.Shift(), TokenType.OpenBracket));
                    break;

                case ']':
                    tokens.Add(new Token(src.Shift(), TokenType.CloseBracket));
                    break;

                case ',':
                    tokens.Add(new Token(src.Shift(), TokenType.Comma));
                    break;

                case '.':
                    tokens.Add(new Token(src.Shift(), TokenType.Dot));
                    break;

                case ':':
                    tokens.Add(new Token(src.Shift(), TokenType.Colon));
                    break;

                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                    tokens.Add(new Token(src.Shift(), TokenType.BinaryOperator));
                    break;

                case '=':
                    tokens.Add(new Token(src.Shift(), TokenType.Equals));
                    break;

                default:
                    if (IsNumeric(src[0]))
                    {
                        StringBuilder number = new StringBuilder();

                        while (src.Count > 0 && (IsNumeric(src[0]) || src[0] == '.'))
                        {
                            number.Append(src.Shift());
                        }

                        tokens.Add(new Token(number.ToString(), TokenType.Number));
                    }
                    else if (IsAlphabetic(src[0]))
                    {
                        StringBuilder identifierBuilder = new StringBuilder();
                        while (src.Count > 0 && IsAlphabetic(src[0]))
                        {
                            identifierBuilder.Append(src.Shift());
                        }

                        string identifier = identifierBuilder.ToString();

                        TokenType reservedToken = TokenType.None;
                        if (KEYWORDS.ContainsKey(identifier))
                        {
                            reservedToken = KEYWORDS[identifier];
                        }

                        if (reservedToken == TokenType.None)
                        {
                            tokens.Add(new Token(identifier, TokenType.Identifier));
                        }
                        else
                        {
                            tokens.Add(new Token(identifier, reservedToken));
                        }
                    }
                    else if (IsSkippable(src[0]))
                    {
                        src.Shift();
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized character: " + src[0]);
                    }

                    break;
            }
        }

        tokens.Add(new Token("EndOfFile", TokenType.EOF));
        return tokens;
    }

    public static bool IsAlphabetic(char src)
        => char.ToUpper(src) != char.ToLower(src);

    public static bool IsNumeric(char src)
        => src >= '0' && src <= '9';

    public static bool IsSkippable(char src)
        => src == ' ' || src == '\n' || src == '\t' || src == '\r';

}

public class SourceLine
{
    public int Line;
    public int Locale;
    public int Marked;
}

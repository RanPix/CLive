namespace CLive.Frontend;

public class Token
{
    public string Value = "";
    public int line;
    public TokenType Type;

    public Token() { }

    public Token(string value, TokenType type)
    {
        Value = value;
        Type = type;
    }

    public Token(char value, TokenType type)
    {
        Value = new string(value, 1);
        Type = type;
    }

    public override string ToString()
    {
        return $"{Value} {Type}";
    }
}

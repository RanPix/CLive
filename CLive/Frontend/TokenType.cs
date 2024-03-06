namespace CLive.Frontend;

public enum TokenType : byte
{
    Number,
    Identifier,
    Equals,


    Let,
    Const,

    Comma, Dot, Colon,
    OpenParen, CloseParen, // ()
    OpenBrace, CloseBrace, // {}
    OpenBracket, CloseBracket, // []
    BinaryOperator,

    //types
    ValueType,
    String,
    //types

    Class,
    New,
    Public,
    Private,

    EOF, // End Of File
    None,
}

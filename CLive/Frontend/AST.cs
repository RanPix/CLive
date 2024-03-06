using System.Reflection.Metadata;

namespace CLive.Frontend;

public enum NodeType : byte
{
    //STATEMENTS
    Program,
    VariableDeclaration,

    // EXPRESSIONS
    AssignmentExpr,

    // LITERALS
    Property,
    ObjectLiteral,
    NumericLiteral,
    StringLiteral,
    Identifier,
    BinaryExpr,
}

public class Statement
{
    public NodeType Kind { get; init; }
}

public class SProgram : Statement
{
    public List<Statement> Body;

    public SProgram(List<Statement> body)
    {
        Kind = NodeType.Program;
        Body = body;
    }
}

public class VariableDeclaration : Statement
{
    public bool IsConstant;
    public string Type;
    public string Identifier;
    public Expression? Value;

    public VariableDeclaration(bool isConstant, string type, string identifier, Expression value)
    {
        Kind = NodeType.VariableDeclaration;
        IsConstant = isConstant;
        Identifier = identifier;
        Type = type;
        Value = value;
    }
}

public class Expression : Statement { }

public class AssignmentExpression : Expression
{
    public Expression Assigne;
    public Expression Value;

    public AssignmentExpression(Expression assigne, Expression value)
    {
        Kind = NodeType.AssignmentExpr;
        Assigne = assigne;
        Value = value;
    }

    public override string ToString()
    {
        return $"Kind: {Kind}\n\t Left: {Assigne}\n\t Right: {Value}\n";
    }
}

public class BinaryExpression : Expression
{
    public Expression Left;
    public Expression Right;
    public string Operator;

    public BinaryExpression(Expression left, Expression right, string _operator)
    {
        Kind = NodeType.BinaryExpr;
        Left = left;
        Right = right;
        Operator = _operator;
    }

    public override string ToString()
    {
        return $"Kind: {Kind}\n\t Left: {Left}\n\t Right: {Right}\n\t Operator: {Operator}\n";
    }
}

public class Identifier : Expression
{
    public string Symbol;

    public Identifier(string symbol)
    {
        Kind = NodeType.Identifier;
        Symbol = symbol;
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Symbol: {Symbol}";
    }
}

public class NumericLiteral : Expression
{
    public string Value;

    public NumericLiteral(string value)
    {
        Kind = NodeType.NumericLiteral;
        Value = value;
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Number: {Value}";
    }
}

public class StringLiteral : Expression
{
    public string Value;

    public StringLiteral(string value)
    {
        Kind = NodeType.StringLiteral;
        Value = value;
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Value: {Value}";
    }
}

public class ObjectLiteral : Expression
{
    public List<Property> Properties;

    public ObjectLiteral(List<Property> properties)
    {
        Kind = NodeType.ObjectLiteral;
        Properties = properties;
    }

    public override string ToString()
    {
        return $"Kind: {Kind}";
    }
}

public class Property : Expression
{
    public string Type;
    public string Symbol;
    public Expression Value;

    public Property(string type, string key, Expression value)
    {
        Kind = NodeType.Property;
        Symbol = key;
        Value = value;
        Type = type;
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Number: {Symbol}";
    }
}



namespace CLive.Frontend;

public enum ActionType : byte
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
    FloatLiteral,
    IntegerLiteral,
    StringLiteral,
    Identifier,
    BinaryExpr,
}

public class ActionStatement
{
    public ActionType Kind { get; init; }
}

public class ActionProgram : ActionStatement
{
    public List<ActionStatement> Body;

    public ActionProgram(List<ActionStatement> body)
    {
        Kind = ActionType.Program;
        Body = body;
    }
}

public class ActionVariableDeclaration : ActionStatement
{
    public bool IsConstant;
    public string Type;
    public string Identifier;
    public ActionExpression? Value;

    public ActionVariableDeclaration(bool isConstant, string type, string identifier, ActionExpression? value)
    {
        Kind = ActionType.VariableDeclaration;
        IsConstant = isConstant;
        Identifier = identifier;
        Type = type;
        Value = value;
    }
}

public class ActionExpression : ActionStatement 
{
    public string Type = "";
}

public class ActionAssignmentExpression : ActionExpression
{
    public ActionExpression Assigne;
    public ActionExpression Value;

    public ActionAssignmentExpression(ActionExpression assigne, ActionExpression value)
    {
        Kind = ActionType.AssignmentExpr;
        Assigne = assigne;
        Value = value;
    }

    public override string ToString()
    {
        return $"Kind: {Kind}\n\t Left: {Assigne}\n\t Right: {Value}\n";
    }
}

public class ActionBinaryExpression : ActionExpression
{
    public ActionExpression Left;
    public ActionExpression Right;
    public string Operator;

    public ActionBinaryExpression(ActionExpression left, ActionExpression right, string type, string _operator)
    {
        Kind = ActionType.BinaryExpr;
        Left = left;
        Right = right;
        Operator = _operator;
        Type = type;
    }

    public override string ToString()
    {
        return $"Kind: {Kind}\n\t Left: {Left}\n\t Right: {Right}\n\t Operator: {Operator}\n";
    }
}

public class ActionIdentifier : ActionExpression
{
    public string Symbol;

    public ActionIdentifier(string type, string symbol)
    {
        Kind = ActionType.Identifier;
        Symbol = symbol;
        Type = type;
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Symbol: {Symbol}";
    }
}

public class ActionIntegerLiteral : ActionExpression
{
    public int Value;

    public ActionIntegerLiteral(int value)
    {
        Kind = ActionType.IntegerLiteral;
        Value = value;
        Type = "int";
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Number: {Value}";
    }
}

public class ActionFloatLiteral : ActionExpression
{
    public float Value;

    public ActionFloatLiteral(float value)
    {
        Kind = ActionType.FloatLiteral;
        Value = value;
        Type = "float";
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Number: {Value}";
    }
}

public class ActionStringLiteral : ActionExpression
{
    public string Value;

    public ActionStringLiteral(string value)
    {
        Kind = ActionType.StringLiteral;
        Value = value;
        Type = "string";
    }

    public override string ToString()
    {
        return $"Kind: {Kind} Value: {Value}";
    }
}

public class ActionObjectLiteral : ActionExpression
{
    public List<ActionProperty> Properties = new List<ActionProperty>();

    public ActionObjectLiteral(string type)
    {
        Kind = ActionType.ObjectLiteral;
        Type = type;
    }
}

public class ActionProperty : ActionExpression
{
    public string Symbol;
    public ActionExpression Value;

    public ActionProperty(string type, string symbol, ActionExpression value)
    {
        Kind = ActionType.Property;
        Type = type;
        Symbol = symbol;
        Value = value;
    }
}
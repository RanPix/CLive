
namespace CLive.Runtime;

public enum ValueType
{
    Null,
    Number,
    Integer,
    Float,
    Fixedpoint,
    Boolean,
    String,
    Class,
}

public class RuntimeValue
{
    public ValueType Type;
}

public class NullValue : RuntimeValue
{
    public string Value = "null";

    public NullValue() 
    { 
        Type = ValueType.Null;
    }
}

public class BooleanValue : RuntimeValue
{
    public bool Value;

    public BooleanValue(bool value)
    {
        Type = ValueType.Boolean;
        Value = value;
    }

    public override string ToString()
    {
        return $"Value: {Value} Type: {Type}";
    }
}

public class NumberValue : RuntimeValue
{
    public string Value;

    public NumberValue(string value)
    {
        Type = ValueType.Number;
        Value = value;
    }

    public override string ToString()
    {
        return $"Type: {Type} Value: {Value}";
    }
}

public class FloatValue : RuntimeValue
{
    public float Value;

    public FloatValue(float value)
    {
        Type = ValueType.Float;
        Value = value;
    }

    public override string ToString()
    {
        return $"Value: {Value} Type: {Type}";
    }
}

public class IntegerValue : RuntimeValue
{
    public int Value;

    public IntegerValue(int value)
    {
        Type = ValueType.Integer;
        Value = value;
    }

    public override string ToString()
    {
        return $"Value: {Value} Type: {Type}";
    }
}

//public class FixedpointValue : NumberValue
//{
//    public int Value; // TODO: implemet fixedpoints

//    public FixedpointValue(int value)
//    {
//        Type = ValueType.Integer;
//        Value = value;
//    }

//    public override string ToString()
//    {
//        return $"Value: {Value} Type: {Type}";
//    }
//}

internal class StringValue : RuntimeValue
{
    public string Value;

    public StringValue(string value)
    {
        Type = ValueType.String;
        Value = value;
    }

    public override string ToString()
    {
        return $"Value: {Value} Type: {Type}";
    }
}

internal class ObjectValue : RuntimeValue
{
    public Environment Environment = new Environment();
    public string strType = "";

    public ObjectValue(string type)
    {
        Type = ValueType.Class;
        strType = type;
    }

}

namespace CLive.Runtime;

public class Environment
{
    private Environment? Parent;

    private Dictionary<string, RuntimeValue> Variables;
    private Dictionary<string, (string, ValueType)> Types;
    private HashSet<string> Constants;

    public Environment(Environment? parent)
    {
        bool isGlobal = parent == null; 
        Parent = parent;
        Variables = new Dictionary<string, RuntimeValue>();
        Types = new Dictionary<string, (string, ValueType)>();
        Constants = new HashSet<string>();

        if (isGlobal)
        {
            SetupScope(this);
        }
    }

    public Environment()
    {
        Parent = null;
        Variables = new Dictionary<string, RuntimeValue>();
        Types = new Dictionary<string, (string, ValueType)>();
        Constants = new HashSet<string>();
    }

    public RuntimeValue DeclareVariable(string strType, string name, RuntimeValue value, bool isConstant = false)
    {
        if (Variables.ContainsKey(name))
        {
            throw new Exception($"Cannot declare variable {name} as it already exists.");
        }

        if (isConstant)
        {
            Constants.Add(name);
        }

        Types.Add(name, (strType, value.Type));
        Variables.Add(name, value);

        return value;
    }
    
    public RuntimeValue AssignVariable(string name, RuntimeValue value)
    {
        Environment env = Resolve(name);

        if (Constants.Contains(name))
        {
            throw new Exception($"Cannot reassign to {name} as it is a constant.");
        }

        env.Variables[name] = value;

        return value;
    }

    public RuntimeValue LookupVariable(string name)
    {
        Environment env = Resolve(name);

        return env.Variables[name];
    }

    public Environment Resolve(string varName)
    {
        if (Variables.ContainsKey(varName))
        {
            return this;
        }

        if (Parent == null) 
        {
            throw new Exception($"Cannot resolve {varName} as it does not exist.");
        }

        return Parent.Resolve(varName);
    }

    public static void SetupScope(Environment environment)
    {
        environment.DeclareVariable("bool", "true", new BooleanValue(true), true);
        environment.DeclareVariable("bool", "false", new BooleanValue(false), true);
        environment.DeclareVariable("null", "null", new NullValue(), true);
    }
}

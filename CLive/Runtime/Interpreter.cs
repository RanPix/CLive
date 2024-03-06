using CLive.Frontend;

namespace CLive.Runtime;

public class Interpreter
{

    public RuntimeValue EvaluateProgram(SProgram program, Environment env)
    {
        RuntimeValue lastEvaluated = new NullValue();

        foreach (Statement statement in program.Body)
        {
            lastEvaluated = Evaluate(statement, env);
        }

        return lastEvaluated;
    }

    public RuntimeValue EvaluateBinaryExpr(BinaryExpression binop, Environment env)
    {
        RuntimeValue lhs = Evaluate(binop.Left, env);
        RuntimeValue rhs = Evaluate(binop.Right, env);
        
        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Number)
        {
            return EvaluateNumberBinaryExpression((NumberValue)lhs, (NumberValue)rhs, binop.Operator);
        }

        return new NullValue();
    }

    private NumberValue EvaluateNumberBinaryExpression(NumberValue lhs, NumberValue rhs, string _operator)
    {
        int result = 0;

        //if (_operator == "+")
        //    result =  lhs.Value + rhs.Value;
        //else if (_operator == "-")
        //    result =  lhs.Value - rhs.Value;
        //else if (_operator == "*")
        //    result =  lhs.Value * rhs.Value;
        //else if (_operator == "/")
        //    result =  lhs.Value / rhs.Value;
        //else if (_operator == "%")
        //    result =  lhs.Value % rhs.Value;


        return new NumberValue(result.ToString());
    }

    //private StringValue EvaluateString

    private RuntimeValue EvaluateIdentifier(Identifier ident, Environment env)
        => env.LookupVariable(ident.Symbol);

    private RuntimeValue EvaluateVariableDeclaration(VariableDeclaration declaration, Environment env)
    {
        RuntimeValue value = declaration.Value != null ? Evaluate(declaration.Value, env) : new NullValue();
        return env.DeclareVariable(declaration.Type, declaration.Identifier, value);
    }

    private RuntimeValue EvaluateAssignment(AssignmentExpression node, Environment env)
    {
        if (node.Assigne.Kind != NodeType.Identifier)
        {
            throw new Exception("Invalid thing inside assignment expression.");
        }

        string varName = ((Identifier)node.Assigne).Symbol;
        return env.AssignVariable(varName, Evaluate(node.Value, env));
    }

    public RuntimeValue Evaluate(Statement astNode, Environment env)
    {
        switch (astNode.Kind)
        {
            case NodeType.NumericLiteral:
                return new NumberValue(((NumericLiteral)astNode).Value);

            case NodeType.StringLiteral:
                return new StringValue(((StringLiteral)astNode).Value);

            case NodeType.Identifier:
                return EvaluateIdentifier((Identifier)astNode, env);

            case NodeType.AssignmentExpr:
                return EvaluateAssignment((AssignmentExpression)astNode, env);

            case NodeType.BinaryExpr:
                return EvaluateBinaryExpr((BinaryExpression)astNode, env);

            case NodeType.Program:
                return EvaluateProgram((SProgram)astNode, env);

            case NodeType.VariableDeclaration:
                return EvaluateVariableDeclaration((VariableDeclaration)astNode, env);

            default:
                Console.WriteLine($"This AST Node has not been yet setup for interpretation. {astNode}");
                return new NullValue();
                throw new Exception($"This AST Node has not been yet setup for interpretation. {astNode}");
        }
    }
}

public class Interpreter2
{

    public RuntimeValue EvaluateProgram(ActionProgram program, Environment env)
    {
        RuntimeValue lastEvaluated = new NullValue();

        foreach (ActionStatement statement in program.Body)
        {
            lastEvaluated = Evaluate(statement, env);
        }

        return lastEvaluated;
    }

    public RuntimeValue EvaluateBinaryExpr(ActionBinaryExpression binop, Environment env)
    {
        RuntimeValue lhs = Evaluate(binop.Left, env);
        RuntimeValue rhs = Evaluate(binop.Right, env);

        if (lhs.Type is ValueType.Integer or ValueType.Float && rhs.Type is ValueType.Integer or ValueType.Float)
        {
            return EvaluateNumberBinaryExpression(lhs, rhs, binop.Operator);
        }

        return new NullValue();
    }

    /// <summary>
    /// this is not how types should be handled, but im too bad for better solutions
    /// </summary>
    private RuntimeValue EvaluateNumberBinaryExpression(RuntimeValue lhs, RuntimeValue rhs, string _operator)
    {
        RuntimeValue result = new RuntimeValue();

        if (lhs.Type == ValueType.Integer && rhs.Type == ValueType.Integer)
        {
            var intLhs = (IntegerValue)lhs;
            var intRhs = (IntegerValue)rhs;

            if (_operator == "+")
                return new IntegerValue(intLhs.Value + intRhs.Value);
            else if (_operator == "-")
                return new IntegerValue(intLhs.Value - intRhs.Value);
            else if (_operator == "*")
                return new IntegerValue(intLhs.Value * intRhs.Value);
            else if (_operator == "/")
                return new IntegerValue(intLhs.Value / intRhs.Value);
            else if (_operator == "%")
                return new IntegerValue(intLhs.Value % intRhs.Value);
        }
        else if (lhs.Type is ValueType.Float && rhs.Type == ValueType.Float)
        {
            var intLhs = (FloatValue)lhs;
            var intRhs = (FloatValue)rhs;

            if (_operator == "+")
                return new FloatValue(intLhs.Value + intRhs.Value);
            else if (_operator == "-")
                return new FloatValue(intLhs.Value - intRhs.Value);
            else if (_operator == "*")
                return new FloatValue(intLhs.Value * intRhs.Value);
            else if (_operator == "/")
                return new FloatValue(intLhs.Value / intRhs.Value);
            else if (_operator == "%")
                return new FloatValue(intLhs.Value % intRhs.Value);
        }

        return result;
    }

    //private StringValue EvaluateString

    //TODO: make action object literal
    private RuntimeValue EvaluateObjectExpression(ActionObjectLiteral objectLiteral, Environment env)
    {
        ObjectValue objectValue = new ObjectValue(objectLiteral.Type);

        foreach (ActionProperty prop in objectLiteral.Properties)
        {
            RuntimeValue runtimeVal = prop.Value is null ? new NullValue() : Evaluate(prop.Value, env);

            objectValue.Environment.DeclareVariable(prop.Type, prop.Symbol, runtimeVal);
        }

        return objectValue;
    }

    private RuntimeValue EvaluateIdentifier(ActionIdentifier ident, Environment env)
        => env.LookupVariable(ident.Symbol);

    private RuntimeValue EvaluateVariableDeclaration(ActionVariableDeclaration declaration, Environment env)
    {
        if (declaration.Value is null)
        {

        }
        else if (declaration.Type != declaration.Value.Type)
        {
            throw new Exception($"Cannot declare a variable of type {declaration.Type} with a value of type {declaration.Value.Type}");
        }

        RuntimeValue value = declaration.Value != null ? Evaluate(declaration.Value, env) : new NullValue();


        return env.DeclareVariable(declaration.Type, declaration.Identifier, value);
    }

    private RuntimeValue EvaluateAssignment(ActionAssignmentExpression node, Environment env)
    {
        //if (node.Assigne.Kind != ActionType.Identifier)
        //{
        //    throw new Exception("Invalid thing inside assignment expression.");
        //}

        string varName = ((ActionIdentifier)node.Assigne).Symbol;
        string varType = ((ActionIdentifier)node.Assigne).Type;

        if (varType != node.Value.Type)
        {
            throw new Exception($"Cannot assign a value of type {node.Value.Type} to {varType}"); // FIX
        }

        return env.AssignVariable(varName, Evaluate(node.Value, env));
    }

    public RuntimeValue Evaluate(ActionStatement astNode, Environment env)
    {
        switch (astNode.Kind)
        {
            case ActionType.IntegerLiteral:
                return new IntegerValue(((ActionIntegerLiteral)astNode).Value);

            case ActionType.FloatLiteral:
                return new FloatValue(((ActionFloatLiteral)astNode).Value);

            case ActionType.StringLiteral:
                return new StringValue(((ActionStringLiteral)astNode).Value);

            case ActionType.ObjectLiteral:
                return EvaluateObjectExpression((ActionObjectLiteral)astNode, env);

            case ActionType.Identifier:
                return EvaluateIdentifier((ActionIdentifier)astNode, env);

            case ActionType.AssignmentExpr:
                return EvaluateAssignment((ActionAssignmentExpression)astNode, env);

            case ActionType.BinaryExpr:
                return EvaluateBinaryExpr((ActionBinaryExpression)astNode, env);

            case ActionType.Program:
                return EvaluateProgram((ActionProgram)astNode, env);

            case ActionType.VariableDeclaration:
                return EvaluateVariableDeclaration((ActionVariableDeclaration)astNode, env);

            default:
                Console.WriteLine($"This AST Node has not been yet setup for interpretation. {astNode}");
                return new NullValue();
                throw new Exception($"This AST Node has not been yet setup for interpretation. {astNode}");
        }
    }
}

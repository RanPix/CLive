

using CLive.Runtime;
using System.Globalization;

namespace CLive.Frontend; 

/// <summary>
/// Intermediate Representation Parser
/// </summary>

public class IRParser
{
    private SProgram? ASTProgram;
    private Dictionary<string, ActionIdentifier> thing = new(); 

    public ActionProgram ProduceActionTree(SProgram astProgram)
    {
        ASTProgram = astProgram;
        ActionProgram program = new ActionProgram(new List<ActionStatement>());

        foreach (Statement s in ASTProgram.Body)
        {
            program.Body.Add(Represent(s));
        }

        return program;
    }

    public ActionStatement RepresentBinaryExpr(BinaryExpression binop)
    {
        ActionExpression lhs = (ActionExpression)Represent(binop.Left);
        ActionExpression rhs = (ActionExpression)Represent(binop.Right);

        ActionBinaryExpression actionBinop;
        if (lhs.Type == rhs.Type)
        {
            actionBinop = new ActionBinaryExpression(lhs, rhs, lhs.Type, binop.Operator);
        }
        else
        {
            throw new Exception("What the hellll");
        }


        return actionBinop;
    }

    private ActionStatement RepresentNumber(NumericLiteral number)
    {
        if (int.TryParse(number.Value, out int intValue))
        {
            return new ActionIntegerLiteral(intValue);
        }
        else if (float.TryParse(number.Value, new CultureInfo("en-us"), out float floatValue))
        {
            return new ActionFloatLiteral(floatValue);
        }
        // TODO: fixedpoint
        else
        {
            throw new Exception($"Incorrect number formatting! Number: {number.Value}");
        }
    }

    private ActionObjectLiteral RepresentObjectExpression(ObjectLiteral objectLiteral)
    {
        ActionObjectLiteral actionObject = new ActionObjectLiteral("object");

        foreach (Property prop in objectLiteral.Properties)
        {
            ActionProperty property = new ActionProperty(prop.Type, prop.Symbol, (ActionExpression)Represent(prop.Value));

            actionObject.Properties.Add(property);
        }

        return actionObject;
    }

    private ActionStatement RepresentIdentifier(Identifier ident)
    {
        return thing[ident.Symbol];
    }

    private ActionStatement RepresentVariableDeclaration(VariableDeclaration declaration)
    {
        ActionVariableDeclaration value = new ActionVariableDeclaration(
            declaration.IsConstant,
            declaration.Type,
            declaration.Identifier, 
            (ActionExpression)Represent(declaration.Value));

        thing.Add(declaration.Identifier, new ActionIdentifier(declaration.Type, declaration.Identifier));

        return value;
    }

    private ActionStatement RepresentAssignment(AssignmentExpression node)
    {
        if (node.Assigne.Kind != NodeType.Identifier)
        {
            throw new Exception("Invalid thing inside assignment expression.");
        }

       
        return new ActionAssignmentExpression((ActionExpression)Represent(node.Assigne), (ActionExpression)Represent(node.Value));
    }

    public ActionStatement Represent(Statement astNode)
    {
        switch (astNode.Kind)
        {
            case NodeType.NumericLiteral:
                return RepresentNumber((NumericLiteral)astNode);

            case NodeType.StringLiteral:
                return new ActionStringLiteral(((StringLiteral)astNode).Value);

            case NodeType.ObjectLiteral:
                return RepresentObjectExpression((ObjectLiteral)astNode);

            case NodeType.Identifier:
                return RepresentIdentifier((Identifier)astNode);

            case NodeType.AssignmentExpr:
                return RepresentAssignment((AssignmentExpression)astNode);

            case NodeType.BinaryExpr:
                return RepresentBinaryExpr((BinaryExpression)astNode);

            case NodeType.VariableDeclaration:
                return RepresentVariableDeclaration((VariableDeclaration)astNode);

            default:
                Console.WriteLine($"This AST Node has not been yet setup for interpretation. {astNode}");
                return new ActionStatement();//new NullValue();
                throw new Exception($"This AST Node has not been yet setup for interpretation. {astNode}");
        }
    }
}
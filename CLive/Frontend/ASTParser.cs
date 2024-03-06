namespace CLive.Frontend;

/// <summary>
/// Abstract Syntax Tree Parser
/// </summary>
/// 

public class ASTParser
{
    private List<Token> tokens = new List<Token>();
    
    private Token At(int offset = 0)
        => tokens[0 + offset];

    private Token Next()
        => tokens.Shift();

    private Token Expect(TokenType type, string err)
    {
        Token prev = tokens.Shift();
        if (prev.Type != type)
        {
            Console.WriteLine("Parse Error: " + err + " " + prev + " Expecting: ", type);
        }

        return prev;
    }

    public bool NotEOF()
        => tokens[0].Type != TokenType.EOF;

    public SProgram ProduceAST(string sourceCode)
    {
        tokens = Lexer.Tokenize(sourceCode);

        SProgram program = new SProgram(new List<Statement>());

        while (NotEOF())
        {
            program.Body.Add(ParseStatement());
        }

        return program;
    }

    private Statement ParseStatement()
    {
        //skip to parse expression
        switch (At().Type)
        {
            case TokenType.ValueType:
            case TokenType.Const:
                return ParseVariableDeclaration();
        }

        return ParseExpression();
    }

    private Statement ParseVariableDeclaration()
    {
        bool isConstant = false;
        if (At().Type == TokenType.Const)
        {
            isConstant = true;
            Next();
        }

        string type = Expect(TokenType.ValueType, "Expected variable initialization type.").Value;

        string identifier = Expect(TokenType.Identifier, "Expected identifier name following let or const keywords.").Value;

        //if (At().Type != (TokenType.Number | TokenType.Identifier))
        //{
        //    Next();
        //    if (isConstant)
        //    {
        //        throw new Exception("Must assign value to a constant expression. No value provided");
        //    }
        //}

        Expect(TokenType.Equals, "Expected equals token following identifier in variable declaration.");

        return new VariableDeclaration(isConstant, type, identifier, ParseExpression());
    }

    private Expression ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private Expression ParseAssignmentExpression()
    {
        Expression left = ParseObjectExpression(); // switch this out with objects

        if (At().Type == TokenType.Equals)
        {
            Next();
            Expression value = ParseAssignmentExpression();
            return new AssignmentExpression(left, value);
        }

        return left;
    }

    private Expression ParseObjectExpression()
    {
        if (At().Type != TokenType.OpenBrace) 
        { 
            return ParseAdditiveExpression();
        }
        Next();

        List<Property> properties = new List<Property>();

        while (NotEOF() && At().Type != TokenType.CloseBrace)
        {
            string type = Expect(TokenType.ValueType, "Class member type expected.").Value;
            string key = Expect(TokenType.Identifier, "Class member identifier expected.").Value;

            if (At().Type == TokenType.Comma)
            {
                Next();
                properties.Add(new Property(type, key, null));
                continue;
            }
            else if (At().Type == TokenType.CloseBrace)
            {
                Next();
                properties.Add(new Property(type, key, null));
                continue;
            }

            Expect(TokenType.Equals, "Missing equals sign following identifier in object expression.");
            Expression value = ParseExpression();

            properties.Add(new Property(type, key, value));

            if (At().Type != TokenType.CloseBrace)
            {
                Expect(TokenType.Comma, "Expected comma or closing brace following property.");
                
            }
        }

        Expect(TokenType.CloseBrace, "Object literal missing closing brace.");
        return new ObjectLiteral(properties);
    }

    private Expression ParseAdditiveExpression()
    {
        Expression left = ParseMultiplicativeExpression();

        while (At().Value == "+" || At().Value == "-")
        {
            string _operator = Next().Value;
            Expression right = ParseMultiplicativeExpression();
            left = new BinaryExpression(
                left,
                right,
                _operator);
        }

        return left;
    }

    private Expression ParseMultiplicativeExpression()
    {
        Expression left = ParsePrimaryExpression();

        while (At().Value == "*" || At().Value == "/" || At().Value == "%")
        {
            string _operator = Next().Value;
            Expression right = ParsePrimaryExpression();
            left = new BinaryExpression(
                left,
                right,
                _operator);
        }

        return left;
    }

    private Expression ParsePrimaryExpression()
    {
        TokenType tk = At().Type;

        switch (tk)
        {
            case TokenType.Identifier:
                return new Identifier(Next().Value);

            case TokenType.Number:
                return new NumericLiteral(Next().Value);

            case TokenType.String:
                return new StringLiteral(Next().Value);

            case TokenType.OpenParen:
                Next();
                Expression value = ParseExpression();
                Expect(TokenType.CloseParen, "Unexpected token found inside parentesised expression. Expected closing parenthesis");
                return value;

            default:
                Console.WriteLine("Unexpected token found during parsing! " + At());
                return new Expression();
        }
    }
}

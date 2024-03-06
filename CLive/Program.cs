using CLive.Frontend;
using CLive.Runtime;
using System.Diagnostics;
using Environment = CLive.Runtime.Environment;

namespace CLive;

internal class Program
{


    static void Main(string[] args)
    {
        while (true)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.R)
            {
                Run();
                Console.WriteLine("\n\n\n\n");
            }
        }

        //string sourceCode = "";
        //using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "/script.pps"))
        //{
        //    sourceCode = reader.ReadToEnd();
        //}

        //List<Token> tokens = Lexer.Tokenize(sourceCode);

        //foreach (Token t in tokens)
        //{
        //    Console.WriteLine(t.ToString());
        //}

        //Parser parser = new Parser();
        //Interpreter interpreter = new Interpreter();
        //Environment env = new Environment();
        //env.DeclareVariable("x", new NumberValue(210));
        //env.DeclareVariable("true", new BooleanValue(true), true);
        //env.DeclareVariable("false", new BooleanValue(false), true);
        //env.DeclareVariable("null", new NullValue(), true);
        ////parser.ProduceAST(sourceCode);

        //while (true)
        //{
        //    Console.Write("> ");
        //    string input = Console.ReadLine() ?? "";

        //    if (input == "exit")
        //    {
        //        return;
        //    }

        //    SProgram program = parser.ProduceAST(input);
        //    //foreach (Statement s in program.Body)
        //    //{
        //    //    Console.WriteLine(s.ToString());
        //    //}

        //    var value = interpreter.Evaluate(program, env);

        //    Console.WriteLine(value);
        //}

        //Parse();
        //Dictionary<string, Func<>>
    }

    public static void Run()
    {
        string sourceCode = "";
        using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "/script.cl"))
        {
            sourceCode = reader.ReadToEnd();
        }

        //List<Token> tokens = Lexer.Tokenize(sourceCode);

        //foreach (Token t in tokens)
        //{
        //    Console.WriteLine(t.ToString());
        //}


        ASTParser parser = new ASTParser();
        IRParser iRParser = new IRParser();
        Interpreter2 interpreter = new Interpreter2();
        Environment env = new Environment();
        //env.DeclareVariable("x", new NumberValue(210));
        //env.DeclareVariable("true", new BooleanValue(true), true);
        //env.DeclareVariable("false", new BooleanValue(false), true);
        //env.DeclareVariable("null", new NullValue(), true);

        SProgram astProgram = parser.ProduceAST(sourceCode);
        ActionProgram program = iRParser.ProduceActionTree(astProgram);

        Stopwatch sw = new Stopwatch();
        sw.Start();

        RuntimeValue value = interpreter.Evaluate(program, env);

        Console.WriteLine(value);
        Console.WriteLine(sw.ElapsedMilliseconds);
        sw.Reset();
    }
}


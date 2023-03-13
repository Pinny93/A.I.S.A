// See https://aka.ms/new-console-template for more information

using CommandDotNet;
using CommandDotNet.Tokens;

namespace A.I.S.A_;

public class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("Initializing A.I.S.A. System ...");

        Console.WriteLine("Welcome to A.I.S.A. Artificial Intelligence Support Assistant v.0.1!");

        if (args.Length > 0)
        {
            return HandleCommand(Console.Out, args);
        }

        int result = Program.HandleInteractiveMode();

        Console.WriteLine("Application terminated.");
        return result;
    }

    public static int HandleCommand(TextWriter outWriter, params string[] args)
    {
        var runner = new AppRunner<AISA>();

        AISA.AnswerWriter = outWriter;
        if (outWriter != Console.Out) { AISA.ErrorWriter = outWriter; }

        Console.SetOut(outWriter);

        return runner.Run(args);
    }

    private static int HandleInteractiveMode()
    {
        AISA.IsInteractive = true;

        while (true)
        {
            Console.Write("A.I.S.A./> ");
            string command = Console.ReadLine() ?? String.Empty;

            switch (command)
            {
                case "exit":
                    return 0;

                default:
                    CommandLineStringSplitter spl = new();
                    HandleCommand(Console.Out, spl.Split(command).ToArray());
                    break;
            }
        }
    }
}
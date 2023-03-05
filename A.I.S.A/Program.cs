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
            return new AppRunner<AISA>().Run(args);
        }

        int result = Program.HandleInteractiveMode();

        Console.WriteLine("Application terminated.");
        return result;
    }

    private static int HandleInteractiveMode()
    {
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
                    new AppRunner<AISA>().Run(spl.Split(command).ToArray());
                    break;
            }
        }
    }
}
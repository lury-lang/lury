using System;
using System.IO;
using Lury.Core.Runtime;

namespace Lury
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "-i")
            {
                using (var stream = new StreamReader(Console.OpenStandardInput()))
                    LuryEngine.Run(stream.ReadToEnd());
            }
            else
                RunAsIntaractiveMode();
        }

        private static void RunAsIntaractiveMode()
        {
            Console.WriteLine("Lury 0.3.0 (< Spec 0.3 phase 1), running on {0}", Environment.Is64BitProcess ? "x64" : "x86");
            var engine = new LuryEngine();

            while (true)
            {
                var input = Prompt();

                if (input == null)
                    break;

                try
                {
                    var obj = engine.Execute(input);

                    if (obj == null)
                        continue;

                    Console.WriteLine("{0}", obj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

        private static string Prompt()
        {
            var multiline = false;
            var emptyLine = 0;
            var input = string.Empty;

            do
            {
                Console.Write(input == string.Empty ? ">>> " : "... ");
                var line = Console.ReadLine();

                if (line == null)
                    return null;

                if (line.TrimEnd().EndsWith(":"))
                    multiline = true;

                input += line + "\n";

                if (string.IsNullOrWhiteSpace(line))
                    emptyLine++;
                else
                    emptyLine = 0;

            } while (multiline && emptyLine < 1);

            return input;
        }
    }
}

//
// Program.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Tomona Nanase
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using Lury;
using Lury.Compiling;
using Lury.Compiling.Logger;
using Lury.SampleRunner.Resources;

namespace Lury.SampleRunner
{
    class Program
    {
        private static ProgramOptions options;
        private static SearchOption searchOption;

        public static void Main(string[] args)
        {
            bool failed;

            if (args.Length == 0)
            {
                ShowUsage();
                Environment.Exit(1);
            }

            options = new ProgramOptions(args);
            searchOption = options.Recursive ? SearchOption.AllDirectories :
                                               SearchOption.TopDirectoryOnly;

            foreach (var fi in EnumerateInputFiles(options.TargetFilePaths))
            {
                if (!options.SilentMode)
                    Console.WriteLine(fi.Name);

                string input;
                using (var textReader = fi.OpenText())
                    input = textReader.ReadToEnd();
                    
                var compiler = new Compiler();
                var success = compiler.Compile(input);

                if (!options.SilentMode)
                    ShowLogs(compiler.OutputLogger);

                if (!success)
                {
                    failed = true;

                    Console.WriteLine();
                    Console.WriteLine("{0}: {1}", Language.Program_Compilation_Failed, fi.Name);

                    if (!options.NonStopMode)
                        Environment.Exit(2);
                }
            }

            if (failed)
                Environment.Exit(2);
            else
                Environment.Exit(0);
        }

        private static void ShowLogs(OutputLogger logger)
        {
            foreach (var output in logger.Outputs)
            {
                if (output.Category == OutputCategory.Error && options.SuppressError)
                    continue;

                if (output.Category == OutputCategory.Warn && options.SuppressWarning)
                    continue;

                if (output.Category == OutputCategory.Info && options.SuppressInfo)
                    continue;

                if (output.Category == OutputCategory.Error && options.EnableColor)
                    Console.ForegroundColor = ConsoleColor.Red;

                if (output.Category == OutputCategory.Warn && options.EnableColor)
                    Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("{0}{1}: {2}", GetCategoryName(output.Category), output.Position, output.Message);

                Console.ResetColor();

                if (output.SourceCode != null && options.EnableCodePointing)
                {
                    var strs = output.SourceCode.GeneratePointingStrings(output.Position, output.Code.Length);

                    if (output.Position.Line > 1)
                        Console.WriteLine("| " + output.SourceCode.GetLine(output.Position.Line - 1));

                    foreach (var s in strs)
                        Console.WriteLine("| " + s);
                }
            }
        }

        private static string GetCategoryName(OutputCategory category)
        {
            switch (category)
            {
                case OutputCategory.Error:
                    return Language.Program_Output_Error;
                case OutputCategory.Warn:
                    return Language.Program_Output_Warn;
                default:
                    return Language.Program_Output_Info;
            }
        }

        private static void ShowUsage()
        {
            var executeFilePath = GetExecuteFilePath();
            Console.WriteLine("{0}: {1} [OPTION]... DIRECTORY...", Language.Program_Usage, executeFilePath);
            Console.WriteLine("{0}: {1} [OPTION]... FILE...", Language.Program_Usage_Or, executeFilePath);
        }

        private static IEnumerable<FileInfo> EnumerateInputFiles(IEnumerable<string> filepaths)
        {
            foreach (var filepath in filepaths)
            {
                if (File.Exists(filepath))
                    yield return new FileInfo(filepath);
                else if (Directory.Exists(filepath))
                {
                    foreach (var fi in Directory.GetFiles(filepath, "*.lr", searchOption))
                        yield return new FileInfo(fi);
                }
                else
                {
                    if (!options.SilentMode)
                        Console.WriteLine(Language.Program_Directory_Not_Found, filepath);
                }
            }
        }

        private static string GetExecuteFilePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
        }
    }
}

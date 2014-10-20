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
using Lury.Compiling;
using Lury.SampleRunner.Resources;

namespace Lury.SampleRunner
{
    class Program
    {
        private static ProgramOptions options;
        private static SearchOption searchOption;

        public static void Main(string[] args)
        {
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
                if (options.SilentMode)
                    Console.WriteLine(fi.Name);

                string input;
                using (var textReader = fi.OpenText())
                    input = textReader.ReadToEnd();
                    
                using (var compiler = new Compiler())
                    if (!compiler.Compile(input))
                    {
                        Console.WriteLine("{0}: {1}", Language.Program_Compilation_Failed, fi.Name);

                        if (!options.NonStopMode)
                            Environment.Exit(2);
                    }
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

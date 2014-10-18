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

namespace Lury.SampleRunner
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: SampleRunner.exe DIRECTORY");
                Environment.Exit(1);
            }

            ProgramOptions options = new ProgramOptions(args);
            SearchOption searchOption = options.Recursive ? SearchOption.AllDirectories :
                                                            SearchOption.TopDirectoryOnly;

            foreach (var directory in options.TargetDirectories)
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                if (!di.Exists)
                {
                    Console.WriteLine("ディレクトリ '{0}' は見つかりません.", di.Name);
                    continue;
                }

                foreach (var fi in di.GetFiles("*.lr", searchOption))
                {
                    Console.WriteLine(fi.Name);

                    string input;
                    using (var textReader = fi.OpenText())
                        input = textReader.ReadToEnd();
                    
                    using (var compiler = new Compiler())
                        if (!compiler.Compile(input))
                        {
                            Console.WriteLine("コンパイル失敗: {0}", fi.Name);
                            Environment.Exit(2);
                        }
                } 
            }
        }
    }

    class ProgramOptions
    {
        public bool Recursive { get; private set; }

        public IEnumerable<string> TargetDirectories { get; private set; }

        public ProgramOptions(string[] args)
        {
            List<string> targetDirectories = new List<string>();
            bool commandSwitch = false;

            foreach (var arg in args)
            {
                if (commandSwitch)
                {
                    targetDirectories.Add(arg);
                    continue;
                }

                int commandIndex = -1;
                string command = arg.StartsWith("-", StringComparison.Ordinal) ?
                                 ((commandIndex = arg.IndexOf(":", StringComparison.Ordinal)) < 0 ?
                                      arg : arg.Substring(0, commandIndex))
                                 : string.Empty;
                string parameter = commandIndex < 0 ? 　string.Empty : arg.Substring(commandIndex);

                switch (command)
                {
                    case "-R":
                        this.Recursive = true;
                        break;

                    case "--":
                        commandSwitch = true;
                        break;

                    default:
                        targetDirectories.Add(arg);
                        break;
                }
            }

            this.TargetDirectories = targetDirectories;
        }
    }
}


//
// ProgramOptions.cs
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

namespace Lury.SampleRunner
{
    class ProgramOptions
    {
        public bool Recursive { get; private set; }

        public IEnumerable<string> TargetFilePaths { get; private set; }

        public bool NonStopMode { get; private set; }

        public bool SilentMode { get; private set; }

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
                    case "--recursive":
                        this.Recursive = true;
                        break;

                    case "--":
                        commandSwitch = true;
                        break;

                    case "--non-stop":
                        this.NonStopMode = true;
                        break;

                    case "-s":
                    case "--silent":
                        this.SilentMode = true;
                        break;

                    default:
                        targetDirectories.Add(arg);
                        break;
                }
            }

            this.TargetFilePaths = targetDirectories;
        }
    }
}


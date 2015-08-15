//
// Program.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2015 Tomona Nanase
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

namespace Lury.Compiling.Elements
{
    class Program : Nonterminal
    {
        public readonly ProgramLines ProgramLines;

        public Program(object programLines)
        {
            this.ProgramLines = (ProgramLines)programLines;
        }

        public Program()
            : this(null)
        {
        }

        public override string ToString()
        {
            if (this.ProgramLines != null)
                return this.ProgramLines.ToString();
            else
                return string.Empty;
        }
    }

    class ProgramLines : Nonterminal, IElementList
    {
        public readonly ProgramLine ProgramLine;
        public readonly ProgramLines NextProgramLines;

        public bool HasNextElement { get { return this.NextProgramLines != null; } }

        public ProgramLines(object programLine, object nextProgramLines)
        {
            this.ProgramLine = (ProgramLine)programLine;
            this.NextProgramLines = (ProgramLines)nextProgramLines;
        }

        public ProgramLines(object programLine)
            : this(programLine, null)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}{1}", this.ProgramLine, this.NextProgramLines);
            else
                return this.ProgramLine.ToString();
        }
    }

    class ProgramLine : Nonterminal
    {
        public readonly Statement Statement;

        public ProgramLine(object statement)
        {
            this.Statement = (Statement)statement;
        }

        public ProgramLine()
            : this(null)
        {
        }

        public override string ToString()
        {
            if (this.Statement != null)
                return this.Statement.ToString();
            else
                return "\n";
        }
    }
}

//
// CompileOutput.cs
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

namespace Lury.Compiling.Logger
{
    public class CompileOutput
    {
        public int OutputNumber { get; private set; }

        public OutputCategory Category { get; private set; }

        public CharPosition Position { get; private set; }

        public string Code { get; private set; }

        public string SourceCode { get; private set; }

        public string Appendix { get; private set; }

        public string Message
        {
            get
            {
                // TODO: Implement!
                throw new NotImplementedException();
            }
        }

        public string Suggestion
        {
            get
            {
                // TODO: Implement!
                throw new NotImplementedException();
            }
        }

        public string SiteLink
        {
            get
            {
                // TODO: Implement!
                throw new NotImplementedException();
            }
        }

        internal CompileOutput(OutputCategory category,
                               int number,
                               string code = null,
                               string sourceCode = null,
                               CharPosition position = default(CharPosition),
                               string appendix = null)
        {
            this.Category = category;
            this.OutputNumber = number;
            this.SourceCode = sourceCode;
            this.Code = code;
            this.Position = position;
            this.Appendix = appendix;
        }
    }
}


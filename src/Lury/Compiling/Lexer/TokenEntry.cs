//
// TokenEntry.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2014-2015 Tomona Nanase
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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Lury.Compiling
{
    public class TokenEntry
    {
        #region -- Private Fields --

        private readonly string name;
        private readonly Regex regexPattern;
        private readonly int tokenNumber;
        private readonly bool ignoreAfterSpace;
        private readonly IEnumerable<string> contextSwitchOn;
        private readonly IEnumerable<string> contextSwitchOff;
        private readonly IEnumerable<string> context;

        #endregion

        #region -- Public Properties --

        public string Name { get { return this.name; } }

        public Regex RegexPattern { get { return this.regexPattern; } }

        public int TokenNumber { get { return this.tokenNumber; } }

        public bool IgnoreAfterSpace { get { return this.ignoreAfterSpace; } }

        public IEnumerable<string> ContextSwitchOn { get { return this.contextSwitchOn; } }

        public IEnumerable<string> ContextSwitchOff { get { return this.contextSwitchOff; } }

        public IEnumerable<string> Context { get { return this.context; } }

        #endregion

        #region -- Constructors --

        public TokenEntry(string name = null,
                          string pattern = null,
                          RegexOptions regexOptions = RegexOptions.None,
                          int tokenNumber = Token.yyErrorCode,
                          bool ignoreAfterSpace = false,
                          IEnumerable<string> contextSwitchOn = null,
                          IEnumerable<string> contextSwitchOff = null,
                          IEnumerable<string> context = null)
        {
            this.name = name;
            this.regexPattern = new Regex(pattern ?? String.Empty, regexOptions | RegexOptions.Compiled);
            this.tokenNumber = tokenNumber;
            this.ignoreAfterSpace = ignoreAfterSpace;
            this.contextSwitchOn = contextSwitchOn;
            this.contextSwitchOff = contextSwitchOff;
            this.context = context;
        }

        #endregion

    }
}


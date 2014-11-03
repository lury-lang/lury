//
// TokenizedToken.cs
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

namespace Lury.Compiling
{
    class TokenizedToken : yyParser.IToken
    {
        private string text;
        private int tokenNumber;
        private int indentLevel;
        private int index;

        public string Text { get { return this.text; } }

        public int TokenNumber { get { return this.tokenNumber; } }

        public int IndentLevel { get { return this.indentLevel; } }

        public int Index { get { return this.index; } }

        public TokenizedToken(string text, int tokenNumber, int indentLevel, int index)
        {
            this.text = text;
            this.tokenNumber = tokenNumber;
            this.indentLevel = indentLevel;
            this.index = index;
        }

        public override string ToString()
        {
            return string.Format("{0}> {1} : \"{2}\"", this.indentLevel, this.tokenNumber, this.text);
        }
    }
}


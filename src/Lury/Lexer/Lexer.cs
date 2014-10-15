//
// Lexer.cs
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
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Lury.Utils;

namespace Lury.Compiling
{
    class Lexer : yyParser.yyInput
    {

        #region -- Private Fields --

        private TokenizedToken[] tokens;
        private int position = -1;
        private bool hasError;
        private Stack<int> indentStack;
        private TextWriter errorWriter;

        #endregion

        #region -- Constructor --

        public Lexer(TextWriter errorWriter, string input)
        {
            this.errorWriter = errorWriter;
            this.indentStack = new Stack<int>();
            this.indentStack.Push(0);

            this.tokens = this.Tokenize(input).ToArray();

            if (hasError)
                throw new ArgumentException();
        }

        #endregion

        #region -- Public Methods --

        public bool advance()
        {
            if (this.tokens.Length <= this.position + 1)
            {
                return false;
            }
            else
            {
                this.position++;
                return true;
            }
        }

        public int token()
        {
            return this.tokens[this.position].TokenNumber;
        }

        public object value()
        {
            return 0;
        }

        #endregion

        #region -- Private Methods --

        private IEnumerable<TokenizedToken> Tokenize(string input)
        {
            int index = 0;
            int inputLength = input.Length;
            bool newLine = false;
            TokenizedToken token = null;

            this.hasError = false;

            while (index < inputLength)
            {
                int length;

                if (!this.SkipComments(input, index, out length))
                {
                    if (newLine)
                    {
                        Regex space = new Regex(@"[\t\f\v\x85\p{Z}]*");
                        Match match = space.Match(input, index);

                        int indentSize = match.Length;

                        if (this.indentStack.Peek() != indentSize)
                        {
                            if (this.indentStack.Peek() < indentSize)
                            {
                                this.indentStack.Push(indentSize);
                                yield return new TokenizedToken(input.Substring(index, indentSize), Token.Indent, this.indentStack.Count - 1);
                            }
                            else
                            {
                                do
                                {
                                    this.indentStack.Pop();
                                    yield return new TokenizedToken("", Token.Dedent, this.indentStack.Count - 1);
                                } while (this.indentStack.Count > 0 &&
                                         this.indentStack.Peek() != indentSize);

                                if (this.indentStack.Count == 0)
                                {
                                    this.ShowError("Can not tokenize", input, index);
                                    yield break;
                                }
                            }
                        }

                        index += indentSize;
                        newLine = false;
                    }

                    if ((token = this.TokenizeFor(TokenPairs.Operators, input, index, out length)) != null)
                        yield return token;
                    else if ((token = this.TokenizeFor(TokenPairs.Delimiters, input, index, out length)) != null)
                        yield return token;
                    else if ((token = this.TokenizeFor(TokenPairs.Keywords, input, index, out length)) != null)
                        yield return token;
                    else if ((token = this.TokenizeFor(TokenPairs.Operands, input, index, out length)) != null)
                        yield return token;
                    else
                    {
                        this.ShowError("Can not tokenize", input, index);
                        yield break;
                    }
                }

                if (token != null)
                    index += length;

                if (index > inputLength)
                    throw new ArgumentOutOfRangeException();

                if (token.TokenNumber == Token.NewLine)
                    newLine = true;
            }

            if (this.indentStack.Count == 0)
            {
                this.ShowError("Illiegal indent", input, index);
                yield break;
            }
            else if (this.indentStack.Peek() != 0)
                do
                {
                    this.indentStack.Pop();
                    yield return new TokenizedToken("", Token.Dedent, this.indentStack.Count - 1);
                } while (this.indentStack.Peek() != 0);
        }

        private bool SkipComments(string input, int index, out int length)
        {
            length = 0;

            foreach (var comment in TokenPairs.Comments)
            {
                var match = comment.MatchBeforeSpace(input, index);

                if (match.Success && match.Groups[1].Index == index)
                {
                    length = match.Length;
                    return true;
                }
            }

            return false;
        }

        private TokenizedToken TokenizeFor(IEnumerable<TokenPair> tokenPair, string input, int index, out int length)
        {
            length = 0;

            foreach (var pair in tokenPair)
            {
                var match = pair.MatchBeforeSpace(input, index);

                if (match.Success && match.Groups[1].Index == index)
                {
                    length = match.Length;
                    return new TokenizedToken(match.Groups[1].Value, pair.TokenNumber, this.indentStack.Count - 1);
                }
            }

            return null;
        }

        private void ShowError(string message, string code, int index)
        {
            int line, column;

            var pointing = code.GeneratePointingStrings(index, out line, out column);

            this.errorWriter.WriteLine("Error({0},{1}): {2}", line, column, message);

            if (line > 1)
                this.errorWriter.WriteLine("| " + code.GetLine(line - 1));

            foreach (var s in pointing)
                this.errorWriter.WriteLine("| " + s);

            this.hasError = true;
        }

        #endregion

    }

    class TokenizedToken
    {
        public string Text { get; private set; }

        public int TokenNumber { get; private set; }

        public int IndentLevel { get; private set; }

        public TokenizedToken(string text, int tokenNumber, int indentLevel)
        {
            this.Text = text;
            this.TokenNumber = tokenNumber;
            this.IndentLevel = indentLevel;
        }

        public override string ToString()
        {
            return string.Format("{0}> {1} : \"{2}\"]", this.IndentLevel, this.TokenNumber, this.Text);
        }
    }
}


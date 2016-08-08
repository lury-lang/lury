/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Tomona Nanase
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 */

using System;
using Antlr4.Runtime;

namespace Lury.Core.Error
{
    public abstract class LuryException : Exception
    {
        #region -- Public Properties --

        public int? Column { get; }

        public int? Line { get; }

        public int? Length { get; }

        public string Text { get; }

        public string SourceName { get; }

        #endregion

        #region -- Constructors --

        protected internal LuryException(
            string message,
            int? column = null,
            int? line = null,
            int? length = null,
            string text = null,
            string sourceName = null)
            : base(message)
        {
            Column = column;
            Line = line;
            Length = length;
            Text = text;
            SourceName = sourceName;
        }

        protected internal LuryException(string message, IToken token)
            : this(message,
                  token.Column,
                  token.Line,
                  token.StopIndex - token.StartIndex,
                  token.Text,
                  token.InputStream.SourceName)
        {
        }

        #endregion
    }
}

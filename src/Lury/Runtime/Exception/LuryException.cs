//
// LuryException.cs
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
using Lury.Compiling.Lexer;
using Lury.Compiling.Utils;

namespace Lury.Runtime
{
    public class LuryException : Exception
    {
        #region -- Public Methods --

        public LuryExceptionType ExceptionType { get; private set; }

        public string SourceCode { get; private set; }

        public CharPosition Position { get; private set; }

        public int CharLength { get; private set; }

        #endregion

        #region -- Constructors --

        public LuryException(LuryExceptionType type,
                             params object[] messageParams)
            : this(type, null, CharPosition.Empty, 0, messageParams)
        {
        }

        public LuryException(LuryExceptionType type,
                             Token token,
                             params object[] messageParams)
            : this(type, token.SourceCode, token.Position, token.Length, messageParams)
        {
        }

        public LuryException(LuryExceptionType type,
                             string sourceCode,
                             CharPosition position,
                             int length,
                             params object[] messageParams)
        {
            this.ExceptionType = type;
            this.SourceCode = sourceCode;
            this.Position = position;
            this.CharLength = length;

            this.Message = type.GetMessage(messageParams);
        }

        #endregion

    }
}


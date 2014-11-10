//
// ErrorCategory.cs
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
    /// <summary>
    /// エラーを識別するための列挙体です。
    /// </summary>
    public enum ErrorCategory
    {
        #region Generic Error

        /// <summary>
        /// 不明なエラー。
        /// </summary>
        Unknown = 0,

        #endregion

        #region Lexer Error

        /// <summary>
        /// トークン化できない文字列が検知されました。
        /// </summary>
        Lexer_CannotTokenize = 1000,

        /// <summary>
        /// 不正なインデントが検知されました。
        /// </summary>
        Lexer_IllegalIndent = 1001,

        #endregion

        #region Parser Error

        /// <summary>
        /// 文法エラー。
        /// </summary>
        Parser_SyntaxError = 2000,

        /// <summary>
        /// 文末での文法エラーです。
        /// </summary>
        Parser_SyntaxErrorAtEOF = 2001,

        /// <summary>
        /// 予期しない位置で文末が出現しました。
        /// </summary>
        Parser_UnexpectedEOF = 2002,

        #endregion
    }
}


//
// RegexPattern.cs
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

namespace Lury
{
    /// <summary>
    /// 頻繁に使用される正規表現パターンの文字列を提供します。
    /// </summary>
    static class RegexPattern
    {
        #region -- Public Static Fields --

        #region Strings

        /// <summary>
        /// 空白文字1文字を表す正規表現パターン文字列。
        /// </summary>
        public const string Space = @"[\t\f\v\x85\p{Z}]";

        /// <summary>
        /// 改行文字1文字を表す正規表現パターン文字列。
        /// </summary>
        public const string NewLine = @"(?:(?:\r\n)|\n|\r)";

        // TODO: 1文字目と2文字目以降に使用できる文字パターンの分離
        public const string Identifier = @"[\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Pc}\p{Lm}$]\w";

        #endregion

        #region Regex

        /// <summary>
        /// 空白文字列にマッチする正規表現オブジェクト。
        /// </summary>
        public static readonly Regex SpaceRegex = new Regex(Space + "*", RegexOptions.Compiled);

        #endregion

        #endregion
    }
}


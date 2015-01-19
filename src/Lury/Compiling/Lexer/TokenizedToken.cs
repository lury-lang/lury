//
// TokenizedToken.cs
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

namespace Lury.Compiling
{
    /// <summary>
    /// トークン化され、ソースコードにおける位置が割り当てられたトークンを格納します。
    /// </summary>
    class TokenizedToken : yyParser.IToken
    {
        #region -- Private Fields --

        private string text;
        private int tokenNumber;
        private int indentLevel;
        private int index;

        #endregion

        #region -- Public Properties --

        /// <summary>
        /// トークンの文字列を取得します。
        /// </summary>
        /// <value>トークンの文字列。</value>
        public string Text { get { return this.text; } }

        /// <summary>
        /// トークンの種類を表すトークンナンバーを取得します。
        /// </summary>
        /// <value>トークンの種類を表すトークンナンバー。</value>
        public int TokenNumber { get { return this.tokenNumber; } }

        /// <summary>
        /// トークンのインデントレベルを取得します。
        /// </summary>
        /// <value>トークンのインデントレベル。</value>
        public int IndentLevel { get { return this.indentLevel; } }

        /// <summary>
        /// トークンが出現したソースコードでのインデックス位置を取得します。
        /// </summary>
        /// <value>インデックス位置。</value>
        public int Index { get { return this.index; } }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// 新しい <see cref="Lury.Compiling.TokenizedToken"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="text">トークン文字列。</param>
        /// <param name="tokenNumber">トークンナンバー。</param>
        /// <param name="indentLevel">インデントレベル。</param>
        /// <param name="index">インデックス位置。</param>
        public TokenizedToken(string text, int tokenNumber, int indentLevel, int index)
        {
            this.text = text;
            this.tokenNumber = tokenNumber;
            this.indentLevel = indentLevel;
            this.index = index;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// 現在の <see cref="Lury.Compiling.TokenizedToken"/> インスタンスと等価な表現の文字列に変換します。
        /// </summary>
        /// <returns>現在のインスタンスを表す等価な文字列。</returns>
        public override string ToString()
        {
            return string.Format("{0}> {1} : \"{2}\"", this.indentLevel, this.tokenNumber, this.text);
        }

        #endregion
    }
}


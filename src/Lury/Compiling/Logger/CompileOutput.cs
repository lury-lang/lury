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
    /// <summary>
    /// コンパイル時に発生した出力メッセージを提供します。
    /// </summary>
    public class CompileOutput
    {
        #region -- Public Properties --

        /// <summary>
        /// 出力メッセージの番号を取得します。
        /// </summary>
        /// <value>出力メッセージ番号を表す整数値。</value>
        public int OutputNumber { get; private set; }

        /// <summary>
        /// 出力メッセージのカテゴリを取得します。
        /// </summary>
        /// <value>カテゴリを表す <see cref="Lury.Compiling.Logger.OutputCategoru"/> 列挙体。</value>
        public OutputCategory Category { get; private set; }

        /// <summary>
        /// ソースコード中の発生位置を取得します。
        /// </summary>
        /// <value>発生位置を表す <see cref="Lury.CharPosition"/> 構造体。</value>
        public CharPosition Position { get; private set; }

        /// <summary>
        /// ソースコード中の該当するコードを取得します。
        /// </summary>
        /// <value>該当するコードを表す文字列。</value>
        public string Code { get; private set; }

        /// <summary>
        /// コンパイル中のソースコードを取得します。
        /// </summary>
        /// <value>ソースコードを表す文字列。</value>
        public string SourceCode { get; private set; }

        /// <summary>
        /// 出力メッセージに付随するメッセージを取得します。
        /// </summary>
        /// <value>付随メッセージを表す文字列。</value>
        public string Appendix { get; private set; }

        /// <summary>
        /// カテゴリと出力メッセージ番号に対応したメッセージを取得します。
        /// </summary>
        /// <value>メッセージを表す文字列。</value>
        public string Message
        {
            get
            {
                // TODO: Implement!
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 出力メッセージに対処するための提案メッセージを取得します。
        /// </summary>
        /// <value>提案メッセージを表す文字列。</value>
        public string Suggestion
        {
            get
            {
                // TODO: Implement!
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 出力メッセージに対応するサイトへのリンクを取得します。
        /// </summary>
        /// <value>サイトへのリンクを表す文字列。</value>
        public string SiteLink
        {
            get
            {
                // TODO: Implement!
                throw new NotImplementedException();
            }
        }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// パラメータを指定して
        /// 新しい <see cref="Lury.Compiling.Logger.CompileOutput"/> クラスのインスタンスを取得します。
        /// </summary>
        /// <param name="category">
        ///     カテゴリを表す <see cref="Lury.Compiling.Logger.OutputCategoru"/> 列挙体。</param>
        /// <param name="number">出力メッセージ番号を表す整数値。</param>
        /// <param name="code">該当するコードを表す文字列。</param>
        /// <param name="sourceCode">ソースコードを表す文字列。</param>
        /// <param name="position">発生位置を表す <see cref="Lury.CharPosition"/> 構造体。</param>
        /// <param name="appendix">付随メッセージを表す文字列。</param>
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

        #endregion
    }
}


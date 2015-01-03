//
// OutputLogger.cs
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
using System.Collections.Generic;
using System.Linq;

namespace Lury.Compiling.Logger
{
    /// <summary>
    /// コンパイル出力を保持するためのロガークラスです。
    /// </summary>
    public class OutputLogger
    {
        #region -- Private Fields --

        private readonly ICollection<CompileOutput> outputs;

        #endregion

        #region -- Public Properties --

        /// <summary>
        /// ロガーに格納された全てのコンパイル出力を列挙する列挙子を取得します。
        /// </summary>
        /// <value><see cref="Lury.Compiling.Logger.CompileOutput"/> を列挙する列挙子。</value>
        public IEnumerable<CompileOutput> Outputs { get { return this.outputs; } }

        /// <summary>
        /// ロガーに格納された全てのエラーを列挙する列挙子を取得します。
        /// </summary>
        /// <value><see cref="Lury.Compiling.Logger.CompileOutput"/> を列挙する列挙子。</value>
        public IEnumerable<CompileOutput> ErrorOutputs
        { 
            get { return this.outputs.Where(o => o.Category == OutputCategory.Error); }
        }

        /// <summary>
        /// ロガーに格納された全ての警告を列挙する列挙子を取得します。
        /// </summary>
        /// <value><see cref="Lury.Compiling.Logger.CompileOutput"/> を列挙する列挙子。</value>
        public IEnumerable<CompileOutput> WarnOutputs
        { 
            get { return this.outputs.Where(o => o.Category == OutputCategory.Warn); }
        }

        /// <summary>
        /// ロガーに格納された全ての情報を列挙する列挙子を取得します。
        /// </summary>
        /// <value><see cref="Lury.Compiling.Logger.CompileOutput"/> を列挙する列挙子。</value>
        public IEnumerable<CompileOutput> InfoOutputs
        { 
            get { return this.outputs.Where(o => o.Category == OutputCategory.Info); }
        }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// 新しい <see cref="Lury.Compiling.Logger.OutputLogger"/> クラスのインスタンスを初期化します。
        /// </summary>
        public OutputLogger()
        {
            this.outputs = new List<CompileOutput>();
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// エラーを報告します。
        /// </summary>
        /// <param name="number">エラー番号。</param>
        /// <param name="code">該当するコード。</param>
        /// <param name="sourceCode">発生したソースコード全体。</param>
        /// <param name="position">発生したソースコード上の位置。</param>
        /// <param name="appendix">コンパイル出力に付随する追加の情報</param>
        public void Error(ErrorCategory number,
                          string code = null,
                          string sourceCode = null,
                          CharPosition position = default(CharPosition),
                          string appendix = null)
        {
            this.outputs.Add(new CompileOutput(OutputCategory.Error, (int)number, code, sourceCode, position, appendix));
        }

        /// <summary>
        /// 警告を報告します。
        /// </summary>
        /// <param name="number">警告番号。</param>
        /// <param name="code">該当するコード。</param>
        /// <param name="sourceCode">発生したソースコード全体。</param>
        /// <param name="position">発生したソースコード上の位置。</param>
        /// <param name="appendix">コンパイル出力に付随する追加の情報</param>
        public void Warn(WarnCategory number,
                         string code = null,
                         string sourceCode = null,
                         CharPosition position = default(CharPosition),
                         string appendix = null)
        {
            this.outputs.Add(new CompileOutput(OutputCategory.Warn, (int)number, code, sourceCode, position, appendix));
        }

        /// <summary>
        /// 情報を報告します。
        /// </summary>
        /// <param name="number">情報番号。</param>
        /// <param name="code">該当するコード。</param>
        /// <param name="sourceCode">発生したソースコード全体。</param>
        /// <param name="position">発生したソースコード上の位置。</param>
        /// <param name="appendix">コンパイル出力に付随する追加の情報</param>
        public void Info(InfoCategory number,
                         string code = null,
                         string sourceCode = null,
                         CharPosition position = default(CharPosition),
                         string appendix = null)
        {
            this.outputs.Add(new CompileOutput(OutputCategory.Info, (int)number, code, sourceCode, position, appendix));
        }

        /// <summary>
        /// 格納された全てのコンパイル出力を削除します。
        /// </summary>
        public void Clear()
        {
            this.outputs.Clear();
        }

        #endregion
    }
}


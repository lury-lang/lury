//
// Compiler.cs
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
using System.IO;
using System.Linq;
using Lury.Resources;
using Lury.Compiling.Logger;

namespace Lury.Compiling
{
    /// <summary>
    /// コンパイルとその出力をカプセル化したクラスです。
    /// </summary>
    public class Compiler
    {
        #region -- Public Properties --
        /// <summary>
        /// コンパイル出力を格納した
        /// <see cref="Lury.Compiling.OutputLogger"/> オブジェクトを取得します。
        /// </summary>
        /// <value><see cref="Lury.Compiling.OutputLogger"/> オブジェクト。</value>
        public OutputLogger OutputLogger { get; private set; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// 新しい <see cref="Lury.Compiling.Compiler"/> クラスのインスタンスを初期化します。
        /// </summary>
        public Compiler()
        {
            this.OutputLogger = new OutputLogger();
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// コンパイルするコードを指定してコンパイルします。
        /// </summary>
        /// <param name="code">コンパイルされるコード文字列。</param>
        /// <returns>>コンパイルに成功したとき true、それ以外のとき false。</returns>
        public bool Compile(string code)
        {
            try
            {
                Parser parser = new Parser();
                Lexer lexer = new Lexer(this.OutputLogger, code);
                parser.yyparse(lexer);

                return !this.OutputLogger.Outputs.Any();
            }
            catch (yyParser.yyException ex)
            {
                this.ReportyyException(ex, code);

                return false;
            }
            catch (Exception ex)
            {
                this.OutputLogger.Error(ErrorCategory.Unknown,
                                        sourceCode: code,
                                        appendix: ex.ToString());

                return false;
            }
        }

        #endregion

        #region -- Private Methods --

        private void ReportyyException(yyParser.yyException ex, string sourceCode)
        {
            var position = sourceCode.GetPositionByIndex(ex.Token.Index);
            var appendix = "Token: " + ex.Token.TokenNumber;

            if (ex is yyParser.yySyntaxError)
            {
                this.OutputLogger.Error(ErrorCategory.Parser_SyntaxError,
                                        code: ex.Token.Text,
                                        sourceCode: sourceCode,
                                        position: position,
                                        appendix: appendix);
            }
            else if (ex is yyParser.yySyntaxErrorAtEof)
            {
                this.OutputLogger.Error(ErrorCategory.Parser_SyntaxErrorAtEOF,
                                        code: ex.Token.Text,
                                        sourceCode: sourceCode,
                                        position: position,
                                        appendix: appendix);
            }
            else if (ex is yyParser.yyUnexpectedEof)
            {
                this.OutputLogger.Error(ErrorCategory.Parser_UnexpectedEOF,
                                        code: ex.Token.Text,
                                        sourceCode: sourceCode,
                                        position: position,
                                        appendix: appendix);
            }
            else
            {
                this.OutputLogger.Error(ErrorCategory.Unknown,
                                        code: ex.Token.Text,
                                        sourceCode: sourceCode,
                                        position: position,
                                        appendix: appendix);
            }
        }

        #endregion

    }
}


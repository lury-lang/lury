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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lury.Compiling.Lexer;
using Lury.Compiling.Logger;
using Lury.Compiling.Utils;
using Lury.Resources;
using Lury.Objects;

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
        /// コンパイルするコードを指定してコンパイルし、実行します。
        /// </summary>
        /// <param name="code">コンパイルされるコード文字列。</param>
        /// <returns>コンパイルに成功したとき true、それ以外のとき false。</returns>
        public LuryObject Evaluate(string code)
        {
            var lexer = new Lexer.Lexer(code + '\n');
            var succeedTokenize = lexer.Tokenize();
            lexer.Logger.CopyTo(this.OutputLogger);

            if (!succeedTokenize)
                return null;

            var globalContext = new LuryObject();
            Intrinsic.SetBuiltInFunctions(globalContext);

            var parser = new Parser();
            Routine routine = null;

            // parsing
            try
            {
                routine = (Routine)parser.yyparse(new Lex2yyInput(lexer), new yydebug.yyDebugSimple());
            }
            catch (yyParser.yyException ex)
            {
                this.ReportyyException(ex, code);
                return null;
            }

            // running
            if (routine == null)
                return null;
            else
            {
                var exit = routine.Evaluate(globalContext);

                if (exit.ExitReason == StatementExitReason.Break)
                    throw new InvalidOperationException();

                return exit.ReturnValue;
            }
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// コンパイルエラーをロガーに出力します。
        /// </summary>
        /// <param name="ex">コンパイルエラーを表す <see cref="Lury.Compiling.yyParser.yyException"/> オブジェクト。</param>
        /// <param name="sourceCode">エラーを起こしたソースコード。</param>
        private void ReportyyException(yyParser.yyException ex, string sourceCode)
        {
            var position = sourceCode.GetPositionByIndex(ex.Token.Index);
            var appendix = "Token: " + ex.Token.TokenNumber;

            if (ex is yyParser.yySyntaxError)
            {
                this.OutputLogger.ReportError(CompileError.SyntaxError, ex.Token.Text, sourceCode, position, appendix);
                return;
            }
        }

        #endregion

        class Lex2yyInput : yyParser.yyInput
        {
            private readonly Lexer.Lexer lexer;
            private IEnumerator<Lexer.Token> tokenEnumerator;

            public Lex2yyInput(Lexer.Lexer lexer)
            {
                this.lexer = lexer;
                this.tokenEnumerator = this.lexer.TokenOutput.GetEnumerator();
            }

            public bool Advance()
            {
                return this.tokenEnumerator.MoveNext();

                /*if (!this.tokenEnumerator.MoveNext())
                    return false;

                return (this.tokenEnumerator.Current.Entry.Name != "EndOfFile");*/
            }

            public yyParser.IToken GetToken()
            {
                return new Token2yyToken(this.tokenEnumerator.Current);
            }

            public object GetValue()
            {
                return this.tokenEnumerator.Current;
            }
        }

        class Token2yyToken : yyParser.IToken
        {
            private readonly Lexer.Token token;

            public Token2yyToken(Lexer.Token token)
            {
                this.token = token;
            }

            public string Text
            {
                get { return token.Text; }
            }

            public int TokenNumber
            {
                get
                {
                    if (this.token.Entry.Name.Length == 1)
                        return (int)this.token.Entry.Name[0];

                    return tokenMap[this.token.Entry.Name];
                }
            }

            public int IndentLevel
            {
                get { throw new NotImplementedException(); }
            }

            public int Index
            {
                get { return token.Index; }
            }

            #region -- Private Static Fields --

            private readonly Dictionary<string, int> tokenMap = new Dictionary<string, int>()
            {
                { "NewLine", Token.NewLine },
                { "Indent", Token.Indent },
                { "Dedent", Token.Dedent },
                { "EndOfFile", Token.NewLine },  // EndOfFile -> NewLine
                { "IdentifierGet", Token.IdentifierGet },
                { "IdentifierSet", Token.IdentifierSet },
                { "IdentifierFile", Token.IdentifierFile },
                { "IdentifierLine", Token.IdentifierLine },
                { "IdentifierExit", Token.IdentifierExit },
                { "IdentifierSuccess", Token.IdentifierSuccess },
                { "IdentifierFailure", Token.IdentifierFailure },
                { "KeywordAbstract", Token.KeywordAbstract },
                { "KeywordAnd", Token.KeywordAnd },
                { "KeywordBreak", Token.KeywordBreak },
                { "KeywordCase", Token.KeywordCase },
                { "KeywordCatch", Token.KeywordCatch },
                { "KeywordClass", Token.KeywordClass },
                { "KeywordContinue", Token.KeywordContinue },
                { "KeywordDef", Token.KeywordDef },
                { "KeywordDefault", Token.KeywordDefault },
                { "KeywordDelete", Token.KeywordDelete },
                { "KeywordElif", Token.KeywordElif },
                { "KeywordElse", Token.KeywordElse },
                { "KeywordEnum", Token.KeywordEnum },
                { "KeywordExtended", Token.KeywordExtended },
                { "KeywordFalse", Token.KeywordFalse },
                { "KeywordFinally", Token.KeywordFinally },
                { "KeywordFor", Token.KeywordFor },
                { "KeywordIf", Token.KeywordIf },
                { "KeywordImport", Token.KeywordImport },
                { "KeywordIn", Token.KeywordIn },
                { "KeywordInterface", Token.KeywordInterface },
                { "KeywordInvariant", Token.KeywordInvariant },
                { "KeywordIs", Token.KeywordIs },
                { "KeywordLazy", Token.KeywordLazy },
                { "KeywordNameof", Token.KeywordNameof },
                { "KeywordNew", Token.KeywordNew },
                { "KeywordNil", Token.KeywordNil },
                { "KeywordNot", Token.KeywordNot },
                { "KeywordOr", Token.KeywordOr },
                { "KeywordOut", Token.KeywordOut },
                { "KeywordOverride", Token.KeywordOverride },
                { "KeywordPass", Token.KeywordPass },
                { "KeywordPrivate", Token.KeywordPrivate },
                { "KeywordProperty", Token.KeywordProperty },
                { "KeywordProtected", Token.KeywordProtected },
                { "KeywordPublic", Token.KeywordPublic },
                { "KeywordRef", Token.KeywordRef },
                { "KeywordReflect", Token.KeywordReflect },
                { "KeywordReturn", Token.KeywordReturn },
                { "KeywordScope", Token.KeywordScope },
                { "KeywordSealed", Token.KeywordSealed },
                { "KeywordStatic", Token.KeywordStatic },
                { "KeywordSuper", Token.KeywordSuper },
                { "KeywordSwitch", Token.KeywordSwitch },
                { "KeywordThis", Token.KeywordThis },
                { "KeywordThrow", Token.KeywordThrow },
                { "KeywordTrue", Token.KeywordTrue },
                { "KeywordTry", Token.KeywordTry },
                { "KeywordUnittest", Token.KeywordUnittest },
                { "KeywordUnless", Token.KeywordUnless },
                { "KeywordUntil", Token.KeywordUntil },
                { "KeywordVar", Token.KeywordVar },
                { "KeywordWhile", Token.KeywordWhile },
                { "KeywordWith", Token.KeywordWith },
                { "KeywordYield", Token.KeywordYield },
                { "Identifier", Token.Identifier },
                { "StringLiteral", Token.StringLiteral },
                { "EmbedStringLiteral", Token.EmbedStringLiteral },
                { "WysiwygStringLiteral", Token.WysiwygStringLiteral },
                { "ImaginaryNumber", Token.ImaginaryNumber },
                { "FloatNumber", Token.FloatNumber },
                { "Integer", Token.Integer },
                { "RangeOpen", Token.RangeOpen },
                { "RangeClose", Token.RangeClose },
                { "Increment", Token.Increment },
                { "AssignmentAdd", Token.AssignmentAdd },
                { "Decrement", Token.Decrement },
                { "AssignmentSub", Token.AssignmentSub },
                { "AnnotationReturn", Token.AnnotationReturn },
                { "AssignmentConcat", Token.AssignmentConcat },
                { "AssignmentPower", Token.AssignmentPower },
                { "Power", Token.Power },
                { "AssignmentMultiply", Token.AssignmentMultiply },
                { "AssignmentIntDivide", Token.AssignmentIntDivide },
                { "IntDivide", Token.IntDivide },
                { "AssignmentDivide", Token.AssignmentDivide },
                { "AssignmentModulo", Token.AssignmentModulo },
                { "AssignmentLeftShift", Token.AssignmentLeftShift },
                { "LeftShift", Token.LeftShift },
                { "LessThan", Token.LessThan },
                { "AssignmentRightShift", Token.AssignmentRightShift },
                { "RightShift", Token.RightShift },
                { "MoreThan", Token.MoreThan },
                { "Equal", Token.Equal },
                { "Lambda", Token.Lambda },
                { "NotEqual", Token.NotEqual },
                { "NotIn", Token.NotIn },
                { "IsNot", Token.IsNot },
                { "AndShort", Token.AndShort },
                { "AssignmentAnd", Token.AssignmentAnd },
                { "AssignmentXor", Token.AssignmentXor },
                { "OrShort", Token.OrShort },
                { "AssignmentOr", Token.AssignmentOr },
                { "NilCoalesce", Token.NilCoalesce },
            };

            #endregion
        }
    }
}


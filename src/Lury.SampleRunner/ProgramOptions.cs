//
// ProgramOptions.cs
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

namespace Lury.SampleRunner
{
    class ProgramOptions
    {
        #region -- Public Properties --

        /// <summary>
        /// ディレクトリが指定された場合に子ディレクトリも再帰的に探索するかの真偽値を取得します。
        /// </summary>
        /// <value>再帰するとき true、それ以外のとき false。</value>
        public bool Recursive { get; private set; }

        /// <summary>
        /// 実行対象のファイル名またはディレクトリ名の列挙子を取得します。
        /// </summary>
        /// <value>ファイルパスを列挙する列挙子。</value>
        public IEnumerable<string> TargetFilePaths { get; private set; }

        /// <summary>
        /// エラーが発生した場合でも処理を中断せずに次の処理を続けるかの真偽値を取得します。
        /// </summary>
        /// <value>中断しないとき true、それ以外のとき false。</value>
        public bool NonStopMode { get; private set; }

        /// <summary>
        /// コンパイル出力以外のメッセージを表示しないかどうかの真偽値を取得します。
        /// </summary>
        /// <value>出力しないとき true、それ以外のとき false。</value>
        public bool SilentMode { get; private set; }

        /// <summary>
        /// エラー出力を表示しないかどうかの真偽値を取得します。
        /// </summary>
        /// <value>エラーを表示しないとき true、それ以外のとき false。</value>
        public bool SuppressError { get; private set; }

        /// <summary>
        /// 警告出力を表示しないかどうかの真偽値を取得します。
        /// </summary>
        /// <value>警告を表示しないとき true、それ以外のとき false。</value>
        public bool SuppressWarning { get; private set; }

        /// <summary>
        /// 情報出力を表示しないかどうかの真偽値を取得します。
        /// </summary>
        /// <value>情報を表示しないとき true、それ以外のとき false。</value>
        public bool SuppressInfo { get; private set; }

        /// <summary>
        /// 出力をカラーにするかの真偽値を取得します。
        /// </summary>
        /// <value>カラー出力するとき true、それ以外のとき false。</value>
        public bool EnableColor { get; private set; }

        /// <summary>
        /// コンパイル出力発生箇所のソースを表示するかの真偽値を取得します。
        /// </summary>
        /// <value>ソースを表示するとき true、それ以外のとき false。</value>
        public bool EnableCodePointing { get; private set; }

        /// <summary>
        /// ヘルプを表示するかの真偽値を取得します。
        /// </summary>
        /// <value>ヘルプ表示モードのとき true、それ以外のとき false。</value>
        public bool ShowHelpMode { get; private set; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// コマンドライン引数を指定して
        /// 新しい <see cref="Lury.SampleRunner.ProgramOptions"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        public ProgramOptions(string[] args)
        {
            this.SetDefaultValue();
            this.ParseCommandLine(args);
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// 各プロパティにデフォルト値を設定します。
        /// </summary>
        private void SetDefaultValue()
        {
            this.EnableColor = true;
            this.EnableCodePointing = true;
        }

        /// <summary>
        /// コマンドライン引数を解析して各プロパティに反映します。
        /// </summary>
        /// <param name="args">コマンドライン引数を格納した文字列の配列。</param>
        private void ParseCommandLine(string[] args)
        {
            List<string> targetDirectories = new List<string>();
            bool commandSwitch = false;

            foreach (var arg in args)
            {
                if (commandSwitch)
                {
                    targetDirectories.Add(arg);
                    continue;
                }

                string command, parameter;
                SeparateArg(arg, out command, out parameter);

                switch (command)
                {
                    case "-h":
                    case "--help":
                        this.ShowHelpMode = true;
                        return;

                    case "-R":
                    case "--recursive":
                        this.Recursive = true;
                        break;

                    case "--":
                        commandSwitch = true;
                        break;

                    case "--non-stop":
                        this.NonStopMode = true;
                        break;

                    case "-s":
                    case "--silent":
                        this.SilentMode = true;
                        break;

                    case "-e":
                    case "--suppress-error":
                        this.SuppressError = true;
                        break;

                    case "-w":
                    case "--suppress-warning":
                        this.SuppressWarning = true;
                        break;

                    case "-i":
                    case "--suppress-info":
                        this.SuppressInfo = true;
                        break;

                    case "--color":
                        this.EnableColor = true;
                        break;

                    case "--disable-color":
                        this.EnableColor = false;
                        break;

                    case "--code-pointing":
                        this.EnableCodePointing = true;
                        break;

                    case "--disable-code-pointing":
                        this.EnableCodePointing = false;
                        break;

                    default:
                        targetDirectories.Add(arg);
                        break;
                }
            }

            this.TargetFilePaths = targetDirectories;
        }

        #endregion

        #region -- Public Static Methods --

        /// <summary>
        /// 引数に関するヘルプを表示します。
        /// </summary>
        public static void ShowHelp()
        {
            var args = new []
            {
                new {Command = "--", Text = "これ以降のオプションをすべてファイルパスとして認識します."},
                new {Command = "    --code-pointing", Text = "コードポインティングを有効化します."},
                new {Command = "    --color", Text = "カラー表示を有効化します."},
                new {Command = "    --disable-code-pointing", Text = "コードポインティングを無効化します."},
                new {Command = "    --disable-color", Text = "カラー表示を無効化します."},
                new {Command = "-e, --suppress-error", Text = "エラー表示を抑制します."},
                new {Command = "-i, --suppress-info", Text = "情報表示を抑制します."},
                new {Command = "    --nonstop", Text = "エラー時が発生しても処理を中断しません."},
                new {Command = "-R, --recursive", Text = "子ディレクトリも再帰的に探索します."},
                new {Command = "-s, --silent", Text = "コンパイル出力以外の表示を抑制します."},
                new {Command = "-w, --suppress-warning", Text = "警告表示を抑制します."},

                new {Command = "-h, --help", Text = "このヘルプを表示します."},
            };

            Console.WriteLine("Options:");
            var commandLength = Math.Min(25, args.Max(a => a.Command.Length));
            var format = string.Format("  {{0,{0}}} {{1}}", -commandLength);

            foreach (var arg in args)
                Console.WriteLine(format, arg.Command, arg.Text);
        }

        #endregion

        #region -- Private Static Methods --

        /// <summary>
        /// 文字列をコマンドとパラメータに分割します。
        /// </summary>
        /// <param name="arg">文字列。</param>
        /// <param name="command">コマンドを表す文字列。</param>
        /// <param name="parameter">パラメータを表す文字列。</param>
        private static void SeparateArg(string arg, out string command, out string parameter)
        {
            int commandIndex = -1;
            command = arg.StartsWith("-", StringComparison.Ordinal) ?
                        ((commandIndex = arg.IndexOf(":", StringComparison.Ordinal)) < 0 ?
                            arg : arg.Substring(0, commandIndex)) : string.Empty;
            parameter = commandIndex < 0 ? 　string.Empty : arg.Substring(commandIndex);
        }

        #endregion
    }
}


//
// Program.cs
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
using Lury;
using Lury.Compiling;
using Lury.Compiling.Logger;
using Lury.Compiling.Utils;
using Lury.SampleRunner.Resources;

namespace Lury.SampleRunner
{
    /// <summary>
    /// プログラムのエントリポイントとなるクラスです。
    /// </summary>
    class Program
    {
        #region -- Private Static Fields --

        private static ProgramOptions options;
        private static SearchOption searchOption;

        #endregion

        #region -- Public Static Methods --

        /// <summary>
        /// プログラムのエントリポイントです。
        /// </summary>
        /// <param name="args">コマンドラインで指定された引数を表す文字列の配列。</param>
        public static void Main(string[] args)
        {
            // コンパイル失敗フラグ
            bool failed = false;

            if (args.Length == 0)
            {
                ShowUsage();
                Environment.Exit(ExitCode.ParameterNotEnough);
            }

            options = new ProgramOptions(args);
            searchOption = options.Recursive ? SearchOption.AllDirectories :
                                               SearchOption.TopDirectoryOnly;

            if (options.ShowHelpMode)
            {
                ShowHelp();
                Environment.Exit(ExitCode.Success);
            }

            foreach (var fi in EnumerateInputFiles(options.TargetFilePaths))
            {
                if (!options.SilentMode)
                    Console.WriteLine(fi.Name);

                string input = ReadFromFile(fi);
                    
                var compiler = new Compiler();
                var success = compiler.CompileAndRun(fi.Name, input);

                if (!options.SilentMode)
                    ShowLogs(compiler.OutputLogger);

                if (!success)
                {
                    failed = true;

                    Console.WriteLine();
                    Console.WriteLine("{0}: {1}", Language.Program_Compilation_Failed, fi.Name);

                    if (!options.NonStopMode)
                        Environment.Exit(ExitCode.CompileError);
                }
            }

            Environment.Exit(failed ? ExitCode.CompileError : ExitCode.Success);
        }

        #endregion

        #region -- Private Static Methods --

        /// <summary>
        /// 指定されたロガーに蓄積されたログを表示します。
        /// </summary>
        /// <param name="logger">ロガーオブジェクト。</param>
        private static void ShowLogs(OutputLogger logger)
        {
            foreach (var output in logger.Outputs)
            {
                if (output.Category == OutputCategory.Error && options.SuppressError ||
                    output.Category == OutputCategory.Warn && options.SuppressWarning ||
                    output.Category == OutputCategory.Info && options.SuppressInfo)
                    continue;

                if (output.Category == OutputCategory.Error && options.EnableColor)
                    Console.ForegroundColor = ConsoleColor.Red;

                if (output.Category == OutputCategory.Warn && options.EnableColor)
                    Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("{0}{1}: {2}", GetCategoryName(output.Category), output.Position, output.Message);

                Console.ResetColor();

                if (output.SourceCode != null && options.EnableCodePointing)
                {
                    int length = output.Code == null ? 0 : output.Code.Length;
                    var strs = output.SourceCode.GeneratePointingStrings(output.Position, length);

                    if (output.Position.Line > 1)
                        Console.WriteLine("| " + output.SourceCode.GetLine(output.Position.Line - 1));

                    foreach (var s in strs)
                        Console.WriteLine("| " + s);
                }
            }
        }

        /// <summary>
        /// 指定されたファイルのテキストを読み込みます。
        /// </summary>
        /// <returns>指定したファイルのテキスト。</returns>
        /// <param name="fi"><see cref="System.IO.FileInfo"/> オブジェクト。</param>
        private static string ReadFromFile(FileInfo fi)
        {
            try
            {
                using (var textReader = fi.OpenText())
                    return textReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(Language.Program_Cant_Open_File, fi.Name);
                Console.WriteLine("{0}: {1}", errorMessage, ex.Message);
                Environment.Exit(ExitCode.FileCannotOpened);
            }

            // ダミー
            return null;
        }

        /// <summary>
        /// カテゴリ名を表す文字列を表示します。
        /// </summary>
        /// <returns>カテゴリ名。</returns>
        /// <param name="category"><see cref="Lury.Compiling.Logger.OutputCategory"/> 列挙体。</param>
        private static string GetCategoryName(OutputCategory category)
        {
            switch (category)
            {
                case OutputCategory.Error:
                    return Language.Program_Output_Error;
                case OutputCategory.Warn:
                    return Language.Program_Output_Warn;
                default:
                    return Language.Program_Output_Info;
            }
        }

        /// <summary>
        /// このプログラムの使用法を表示します。
        /// </summary>
        private static void ShowUsage()
        {
            var executeFilePath = GetExecuteFilePath();
            Console.WriteLine("{0}: {1} [OPTION]... DIRECTORY...", Language.Program_Usage, executeFilePath);
            Console.WriteLine("{0}: {1} [OPTION]... FILE...", Language.Program_Usage_Or, executeFilePath);
        }

        /// <summary>
        /// 指定されたファイルパスを解析し、ディレクトリの場合はそのディレクトリ直下にあるファイルを返します。
        /// </summary>
        /// <returns><see cref="System.IO.FileInfo"/> オブジェクトの列挙子。</returns>
        /// <param name="filepaths">ファイルパスを表す文字列の列挙子。</param>
        private static IEnumerable<FileInfo> EnumerateInputFiles(IEnumerable<string> filepaths)
        {
            foreach (var filepath in filepaths)
            {
                if (File.Exists(filepath))
                    yield return new FileInfo(filepath);
                else if (Directory.Exists(filepath))
                {
                    foreach (var fi in Directory.GetFiles(filepath, "*.lr", searchOption))
                        yield return new FileInfo(fi);
                }
                else
                {
                    if (!options.SilentMode)
                        Console.WriteLine(Language.Program_Directory_Not_Found, filepath);
                }
            }
        }

        /// <summary>
        /// このプログラムの実行ファイル名を取得します。
        /// </summary>
        /// <returns>ファイル名を表す文字列。</returns>
        private static string GetExecuteFilePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
        }

        /// <summary>
        /// プログラムのヘルプを表示します。
        /// </summary>
        private static void ShowHelp()
        {
            ShowUsage();
            Console.WriteLine();
            ProgramOptions.ShowHelp();
            Console.WriteLine();
            Console.WriteLine("プログラムのリポジトリ: <https://github.com/lury-lang/lury>");
        }

        #endregion
    }
}

//
// CompileError.cs
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

namespace Lury.Compiling
{
    // Compiler's Number Range: 0x20_0000 - 0x2f_ffff

    /// <summary>
    /// エラーを識別するための列挙体です。
    /// </summary>
    public enum CompileError
    {
        // Number Range: 0x20_0000 - 0x20_ffff

        /// <summary>
        /// 不明なエラーです。
        /// </summary>
        Unknown = 0x200000,

        /// <summary>
        /// 文法エラーです。
        /// </summary>
        SyntaxError,
    }

    /// <summary>
    /// 警告を識別するための列挙体です。
    /// </summary>
    public enum CompileWarning
    {
        // Number Range: 0x21_0000 - 0x21_ffff

        /// <summary>
        /// 不明な警告です。
        /// </summary>
        Unknown = 0x210000,
    }

    /// <summary>
    /// 情報を識別するための列挙体です。
    /// </summary>
    public enum CompileInfo
    {
        // Number Range: 0x22_0000 - 0x22_ffff

        /// <summary>
        /// 不明な情報です。
        /// </summary>
        Unknown = 0x220000,
    }
}

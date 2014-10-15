//
// Compiler.cs
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
using System.IO;

namespace Lury.Compiling
{
    public class Compiler : IDisposable
    {

        #region -- Private Fields --

        private Stream errorOutputStream;
        private StreamWriter errorOutputWriter;
        private bool isDisposed;

        #endregion

        #region -- Public Properties --

        public Stream ErrorOutputStream
        { 
            get
            {
                return this.errorOutputStream;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (!value.CanWrite)
                    throw new ArgumentException("Can not write to stream");

                if (this.errorOutputStream == value)
                    return;

                if (this.errorOutputStream != null)
                {
                    this.errorOutputWriter.Flush();
                    this.errorOutputWriter.Dispose();
                    this.errorOutputStream.Dispose();
                }

                this.errorOutputStream = value;
                this.errorOutputWriter = new StreamWriter(value);
                this.errorOutputWriter.AutoFlush = true;
            }
        }

        #endregion

        #region -- Constructors --

        public Compiler()
        {
            this.ErrorOutputStream = Console.OpenStandardOutput();
        }

        #endregion

        #region -- Public Methods --

        public bool Compile(string code)
        {
            try
            {
                Parser parser = new Parser();
                Lexer lexer = new Lexer(this.errorOutputWriter, code);
                parser.yyparse(lexer);
            }
            catch
            {
                return false;
            }
            finally
            {
                this.errorOutputWriter.Flush();
            }

            this.errorOutputWriter.Flush();
            return true;
        }

        /// <summary>
        /// このオブジェクトで使用されているリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region -- Protected Methods --

        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">
        /// マネージリソースとアンマネージリソースの両方を解放する場合は true。アンマネージリソースだけを解放する場合は false。
        /// </param>
        protected void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.errorOutputWriter != null)
                    {
                        this.errorOutputWriter.Flush();
                        this.errorOutputWriter.Dispose();
                    }

                    if (this.errorOutputStream != null)
                        this.errorOutputStream.Dispose();
                }

                this.errorOutputWriter = null;
                this.errorOutputStream = null;
                this.isDisposed = true;
            }
        }

        #endregion

        #region -- Destructors --

        ~Compiler()
        {
            this.Dispose(false);
        }

        #endregion

    }
}


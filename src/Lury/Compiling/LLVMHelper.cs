//
// LLVMHelper.cs
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

using System;
using LLVMSharp;
using System.Runtime.InteropServices;

namespace Lury.Compiling
{
    public class LLVMHelper : IDisposable
    {
        #region -- Private Fields --

        private LLVMContextRef context;

        private LLVMModuleRef module;

        private LLVMBuilderRef builder;

        private LLVMExecutionEngineRef executionEngine;

        #endregion

        #region -- Public Delegates --

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Executable();

        #endregion

        #region -- Public Properties --

        public LLVMContextRef Context
        { 
            get
            {
                this.CheckDisposed();
                return this.context;
            }
        }

        public LLVMModuleRef Module
        { 
            get
            {
                this.CheckDisposed();
                return this.module;
            }
        }

        public LLVMBuilderRef Builder
        { 
            get
            {
                this.CheckDisposed();
                return this.builder;
            }
        }

        public LLVMExecutionEngineRef ExecutionEngine
        { 
            get
            {
                this.CheckDisposed();
                return this.executionEngine;
            }
        }

        public bool IsDisposed { get; private set; }

        #endregion

        #region -- Constructors --

        public LLVMHelper(string moduleName)
        {
            this.context = LLVM.ContextCreate();
            this.module = LLVM.ModuleCreateWithNameInContext(moduleName, this.context);
            this.builder = LLVM.CreateBuilderInContext(this.context);
            this.InitializeJIT();
        }

        #endregion

        #region -- Public Methods --

        public Executable GetExecutableFunction(LLVMValueRef entryFunction)
        {
            var functionPointer = LLVM.GetPointerToGlobal(this.executionEngine, entryFunction);
            return (Executable)Marshal.GetDelegateForFunctionPointer(functionPointer, typeof(Executable));
        }

        public bool Verify()
        {
            IntPtr error;
            var failed = LLVM.VerifyModule(this.module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out error);

            return (failed.Value == 0);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region -- Protected Methods --

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.executionEngine.Pointer != IntPtr.Zero)
                {
                    LLVM.DisposeExecutionEngine(this.executionEngine);
                    this.executionEngine.Pointer = IntPtr.Zero;
                }

                if (this.builder.Pointer != IntPtr.Zero)
                {
                    LLVM.DisposeBuilder(this.builder);
                    this.builder.Pointer = IntPtr.Zero;
                }

                if (this.context.Pointer != IntPtr.Zero)
                {
                    LLVM.ContextDispose(this.context);
                    this.context.Pointer = IntPtr.Zero;
                }
            }

            this.IsDisposed = true;
        }

        #endregion

        #region -- Private Methods --

        private void CheckDisposed()
        {
            if (this.IsDisposed)
                throw new ObjectDisposedException("this");
        }

        private void InitializeJIT()
        {
            IntPtr error;

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                LLVM.SetTarget(module, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");

            var options = new LLVMMCJITCompilerOptions();
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size;

            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);
            LLVM.CreateMCJITCompilerForModule(out this.executionEngine, this.module, out options, optionsSize, out error);
            ReportError(error);
        }

        private static void ReportError(IntPtr error)
        {
            if (error != IntPtr.Zero)
                Console.Error.WriteLine(Marshal.PtrToStringAuto(error));
        }

        #endregion

        #region -- Destructor --

        ~LLVMHelper ()
        {
            this.Dispose(false);
        }

        #endregion

    }
}


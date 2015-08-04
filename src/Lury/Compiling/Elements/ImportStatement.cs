//
// ImportStatement.cs
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

namespace Lury.Compiling.Elements
{
    class ImportStatement : SimpleStatement
    {
        public readonly ImportName ImportName;

        public ImportStatement(object importName)
        {
            this.ImportName = (ImportName)importName;
        }

        public override string ToString()
        {
            return string.Format("import {0}", this.ImportName);
        }
    }

    class PublicImportStatement : ImportStatement
    {
        public PublicImportStatement(object importName)
            : base (importName)
        {
        }

        public override string ToString()
        {
            return string.Format("public import {0}", this.ImportName);
        }
    }

    class ImportName : Nonterminal, IElementList
    {
        public readonly ModuleName ModuleName;
        public readonly ImportName ImportNameList;

        public bool HasNextElement { get { return this.ImportNameList != null; } }

        public ImportName(object moduleName, object importName)
        {
            this.ModuleName = (ModuleName)moduleName;
            this.ImportNameList = (ImportName)importName;
        }

        public ImportName(object moduleName)
            : this(moduleName, null)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}, {1}", this.ModuleName, this.ImportNameList);
            else
                return this.ModuleName.ToString();
        }
    }

    class ModuleName : Nonterminal, IElementList
    {
        public readonly Terminal Identifier;
        public readonly ModuleName ModuleNameList;

        public bool HasNextElement { get { return this.ModuleNameList != null; } }

        public ModuleName(object identifier, object moduleName)
        {
            this.Identifier = (Terminal)identifier;
            this.ModuleNameList = (ModuleName)moduleName;
        }

        public ModuleName(object identifier)
            : this(identifier, null)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}.{1}", this.ModuleNameList, this.Identifier);
            else
                return this.Identifier.ToString();
        }
    }
}

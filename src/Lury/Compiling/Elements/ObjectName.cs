//
// ObjectName.cs
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
    class ObjectName : Nonterminal, IElementList
    {
        public readonly ObjectName ObjectNameList;
        public readonly Terminal Identifier;

        public bool HasNextElement { get { return this.ObjectNameList != null; }}

        public ObjectName(object objectName, object identifier)
        {
            this.ObjectNameList = (ObjectName)objectName;
            this.Identifier = (Terminal)identifier;
        }

        public ObjectName(object identifier)
            : this(null, identifier)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}.{1}", this.ObjectNameList, this.Identifier);
            else
                return this.Identifier.ToString();
        }
    }
}

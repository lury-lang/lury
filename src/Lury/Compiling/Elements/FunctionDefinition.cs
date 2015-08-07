//
// FunctionDefinition.cs
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
    class FunctionDefinition : CompundStatement
    {
        public readonly Lexer.Token Def;
        public readonly FunctionName FunctionName;
        public readonly FunctionParameterList FunctionParameterList;
        public readonly Suite Suite;

        public FunctionDefinition(object def, object functionName, object functionParameterList, object suite)
        {
            this.Def = (Lexer.Token)def;
            this.FunctionName = (FunctionName)functionName;
            this.FunctionParameterList = (FunctionParameterList)functionParameterList;
            this.Suite = (Suite)suite;
        }

        public FunctionDefinition(object def, object functionName, object suite)
            : this(def, functionName, null, suite)
        {
        }

        public override string ToString()
        {
            if (this.FunctionParameterList != null)
                return string.Format("{0} {1}({2}):{3}", this.Def, this.FunctionName, this.FunctionParameterList, this.Suite);
            else
                return string.Format("{0} {1}:{2}", this.Def, this.FunctionName, this.Suite);
        }
    }

    class FunctionName : Nonterminal, IElementList
    {
        public readonly Terminal Identifier;
        public readonly FunctionName FunctionNameList;

        public bool HasNextElement { get { return this.FunctionNameList != null; }}

        public FunctionName(object functionNameList, object identifier)
        {
            this.FunctionNameList = (FunctionName)functionNameList;
            this.Identifier = (Terminal)identifier;
        }

        public FunctionName(object identifier)
            : this(null, identifier)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}.{1}", this.FunctionNameList, this.Identifier);
            else
                return this.Identifier.ToString();
        }
    }

    class FunctionParameterList : Nonterminal, IElementList
    {
        public readonly Terminal Identifier;
        public readonly FunctionParameterList ParameterList;

        public bool HasNextElement { get { return this.ParameterList != null; }}

        public FunctionParameterList(object parameterList, object identifier)
        {
            this.ParameterList = (FunctionParameterList)parameterList;
            this.Identifier = (Terminal)identifier;
        }

        public FunctionParameterList(object identifier)
            : this(null, identifier)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}, {1}", this.ParameterList, this.Identifier);
            else
                return this.Identifier.ToString();
        }
    }
}

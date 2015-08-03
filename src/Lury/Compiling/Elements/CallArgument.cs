//
// CallArgument.cs
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
    class CallArgumentList : Nonterminal, IElementList
    {
        public readonly CallArgument Argument;
        public readonly CallArgumentList ArgumentList;

        public bool HasNextElement { get { return this.ArgumentList != null; }}

        public CallArgumentList(object argument, object argumentList)
        {
            this.Argument = (CallArgument)argument;
            this.ArgumentList = (CallArgumentList)argumentList;
        }

        public CallArgumentList(object argument)
            : this(argument, null)
        {
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("{0}, {1}", this.Argument, this.ArgumentList);
            else
                return this.Argument.ToString();
        }
    }

    class CallArgument : Nonterminal
    {
        public readonly Expression Expression;

        public CallArgument(object expression)
        {
            this.Expression = (Expression)expression;
        }

        public override string ToString()
        {
            return this.Expression.ToString();
        }
    }

    class NamedCallArgument : CallArgument
    {
        public readonly Terminal Identifier;

        public NamedCallArgument (object identifier, object expression)
            : base(expression)
        {
            this.Identifier = (Terminal)identifier;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Identifier, this.Expression);
        }
    }
}

//
// PostfixExpression.cs
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
    abstract class PostfixExpression : Nonterminal
    { 
    }

    class PostfixExpressionPrimary : PostfixExpression
    {
        public readonly object Primary;

        public PostfixExpressionPrimary(object primary)
        {
            this.Primary = primary;
        }

        public override string ToString()
        {
            return this.Primary.ToString();
        }
    }

    #region Recursive Rules

    abstract class PostfixExpressionRecursive : PostfixExpression
    {
        public readonly PostfixExpression Postfix;

        public PostfixExpressionRecursive(object postfix)
        {
            this.Postfix = (PostfixExpression)postfix;
        }
    }

    class PostfixExpressionDot : PostfixExpressionRecursive
    {
        public readonly Terminal Identifier;

        public PostfixExpressionDot(object postfix, object identifier)
            : base(postfix)
        {
            this.Identifier = (Terminal)identifier;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", this.Postfix, this.Identifier);
        }
    }

    class PostfixExpressionIncrement : PostfixExpressionRecursive
    {
        public PostfixExpressionIncrement(object postfix)
            : base(postfix)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}++", this.Postfix);
        }
    }

    class PostfixExpressionDecrement : PostfixExpressionRecursive
    {
        public PostfixExpressionDecrement(object postfix)
            : base(postfix)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}--", this.Postfix);
        }
    }

    class PostfixExpressionCallNoArg : PostfixExpressionRecursive
    {
        public PostfixExpressionCallNoArg(object postfix)
            : base(postfix)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}()", this.Postfix);
        }
    }

    class PostfixExpressionCall : PostfixExpressionRecursive
    {
        // TODO: Create class ArgumentList
        public readonly object ArgumentList;

        public PostfixExpressionCall(object postfix, object argumentList)
            : base(postfix)
        {
            this.ArgumentList = argumentList;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", this.Postfix, this.ArgumentList);
        }
    }

    #endregion
}


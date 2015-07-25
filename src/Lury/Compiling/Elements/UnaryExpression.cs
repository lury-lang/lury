//
// UnaryExpression.cs
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
    abstract class UnaryExpression : Nonterminal
    { 
    }

    class UnaryExpressionPower : UnaryExpression
    {
        public readonly PowerExpression Power;

        public UnaryExpressionPower(object power)
        {
            this.Power = (PowerExpression)power;
        }

        public override string ToString()
        {
            return this.Power.ToString();
        }
    }

    #region Recursive Rules

    abstract class UnaryExpressionRecursive : UnaryExpression
    {
        public readonly UnaryExpression Unary;

        public UnaryExpressionRecursive(object unary)
        {
            this.Unary = (UnaryExpression)unary;
        }
    }

    class UnaryExpressionIncrement : UnaryExpressionRecursive
    {
        public UnaryExpressionIncrement(object unary)
            : base(unary)
        {
        }

        public override string ToString()
        {
            return string.Format("++{0}", this.Unary);
        }
    }

    class UnaryExpressionDecrement : UnaryExpressionRecursive
    {
        public UnaryExpressionDecrement(object unary)
            : base(unary)
        {
        }

        public override string ToString()
        {
            return string.Format("--{0}", this.Unary);
        }
    }

    class UnaryExpressionMinus : UnaryExpressionRecursive
    {
        public UnaryExpressionMinus(object unary)
            : base(unary)
        {
        }

        public override string ToString()
        {
            return string.Format("-{0}", this.Unary);
        }
    }

    class UnaryExpressionPlus : UnaryExpressionRecursive
    {
        public UnaryExpressionPlus(object unary)
            : base(unary)
        {
        }

        public override string ToString()
        {
            return string.Format("+{0}", this.Unary);
        }
    }

    class UnaryExpressionInvert : UnaryExpressionRecursive
    {
        public UnaryExpressionInvert(object unary)
            : base(unary)
        {
        }

        public override string ToString()
        {
            return string.Format("~{0}", this.Unary);
        }
    }

    #endregion
}


//
// MultiplicationExpression.cs
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
    abstract class MultiplicationExpression : Nonterminal
    { 
    }

    class MultiplicationExpressionUnary : MultiplicationExpression
    {
        public readonly UnaryExpression Unary;

        public MultiplicationExpressionUnary(object unary)
        {
            this.Unary = (UnaryExpression)unary;
        }

        public override string ToString()
        {
            return this.Unary.ToString();
        }
    }

    #region Recursive Rules

    abstract class MultiplicationExpressionRecursive : MultiplicationExpression
    {
        public readonly MultiplicationExpression Multiplication;
        public readonly UnaryExpression Unary;

        public MultiplicationExpressionRecursive(object multiplication, object unary)
        {
            this.Multiplication = (MultiplicationExpression)multiplication;
            this.Unary = (UnaryExpression)unary;
        }
    }

    class MultiplicationExpressionMultiplication : MultiplicationExpressionRecursive
    {
        public MultiplicationExpressionMultiplication(object mulitiplication, object unary)
            : base(mulitiplication, unary)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} * {1}", this.Multiplication, this.Unary);
        }
    }

    class MultiplicationExpressionIntDivision : MultiplicationExpressionRecursive
    {
        public MultiplicationExpressionIntDivision(object mulitiplication, object unary)
            : base(mulitiplication, unary)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} // {1}", this.Multiplication, this.Unary);
        }
    }

    class MultiplicationExpressionDivision : MultiplicationExpressionRecursive
    {
        public MultiplicationExpressionDivision(object mulitiplication, object unary)
            : base(mulitiplication, unary)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}", this.Multiplication, this.Unary);
        }
    }

    class MultiplicationExpressionModulo : MultiplicationExpressionRecursive
    {
        public MultiplicationExpressionModulo(object mulitiplication, object unary)
            : base(mulitiplication, unary)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} % {1}", this.Multiplication, this.Unary);
        }
    }

    #endregion
}


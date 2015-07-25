//
// AdditionExpression.cs
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
    abstract class AdditionExpression : Nonterminal
    { 
    }

    class AdditionExpressionMultiplication : AdditionExpression
    {
        public readonly MultiplicationExpression Multiplication;

        public AdditionExpressionMultiplication(object multiplication)
        {
            this.Multiplication = (MultiplicationExpression)multiplication;
        }

        public override string ToString()
        {
            return this.Multiplication.ToString();
        }
    }

    #region Recursive Rules

    abstract class AdditionExpressionRecursive : AdditionExpression
    {
        public readonly AdditionExpression Addition;
        public readonly MultiplicationExpression Multiplication;

        public AdditionExpressionRecursive(object addition, object multiplication)
        {
            this.Addition = (AdditionExpression)addition;
            this.Multiplication = (MultiplicationExpression)multiplication;
        }
    }

    class AdditionExpressionRecursiveAddition : AdditionExpressionRecursive
    {
        public AdditionExpressionRecursiveAddition(object addition, object multiplication)
            : base(addition, multiplication)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} + {1}", this.Addition, this.Multiplication);
        }
    }

    class AdditionExpressionRecursiveSubtraction : AdditionExpressionRecursive
    {
        public AdditionExpressionRecursiveSubtraction(object addition, object multiplication)
            : base(addition, multiplication)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Addition, this.Multiplication);
        }
    }

    class AdditionExpressionRecursiveConcatenation : AdditionExpressionRecursive
    {
        public AdditionExpressionRecursiveConcatenation(object addition, object multiplication)
            : base(addition, multiplication)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} ~ {1}", this.Addition, this.Multiplication);
        }
    }

    #endregion
}


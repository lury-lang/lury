//
// ShiftExpression.cs
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
    abstract class ShiftExpression : Nonterminal
    { 
    }

    class ShiftExpressionAddition : ShiftExpression
    {
        public readonly AdditionExpression Addition;

        public ShiftExpressionAddition(object addition)
        {
            this.Addition = (AdditionExpression)addition;
        }

        public override string ToString()
        {
            return this.Addition.ToString();
        }
    }

    #region Recursive Rules

    abstract class ShiftExpressionRecursive : ShiftExpression
    {
        public readonly ShiftExpression Shift;
        public readonly AdditionExpression Addition;

        public ShiftExpressionRecursive(object shift, object addition)
        {
            this.Shift = (ShiftExpression)shift;
            this.Addition = (AdditionExpression)addition;
        }
    }

    class ShiftExpressionLeft : ShiftExpressionRecursive
    {
        public ShiftExpressionLeft(object shift, object addition)
            : base(shift, addition)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} << {1}", this.Shift, this.Addition);
        }
    }

    class ShiftExpressionRight : ShiftExpressionRecursive
    {
        public ShiftExpressionRight(object shift, object addition)
            : base(shift, addition)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} >> {1}", this.Shift, this.Addition);
        }
    }

    #endregion
}


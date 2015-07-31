//
// ConditionalExpression.cs
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
    class AssignmentExpression : Nonterminal
    {
        public readonly ConditionalExpression Conditional;

        public AssignmentExpression (object conditional)
        {
            this.Conditional = (ConditionalExpression)conditional;
        }

        public override string ToString()
        {
            return this.Conditional.ToString();
        }
    }

    #region Recursive

    abstract class AssignmentExpressionRecursive : AssignmentExpression
    {
        public readonly AssignmentExpression Assignment;

        public AssignmentExpressionRecursive (object conditional, object assignment)
            : base(conditional)
        {
            this.Assignment = (AssignmentExpression)assignment;
        }
    }

    class AssignmentExpressionAssignment　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionAssignment(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionAdd　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionAdd(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} += {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionSub　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionSub(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} -= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionMultiply　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionMultiply(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} *= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionDivide　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionDivide(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} /= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionIntDivide　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionIntDivide(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} //= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionPower　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionPower(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} **= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionModulo : AssignmentExpressionRecursive
    {
        public AssignmentExpressionModulo(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} %= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionAnd　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionAnd(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} &= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionOr　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionOr(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} |= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionXor　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionXor(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} ^= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionConcat　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionConcat(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} ~= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionLeftShift　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionLeftShift(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} <<= {1}", this.Conditional, this.Assignment);
        }
    }

    class AssignmentExpressionRightShift　: AssignmentExpressionRecursive
    {
        public AssignmentExpressionRightShift(object conditional, object assignment)
            : base(conditional, assignment)
        {
        }

        public override string ToString()
        {
            return string.Format("{0} >>= {1}", this.Conditional, this.Assignment);
        }
    }

    #endregion
}

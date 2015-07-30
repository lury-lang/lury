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
    class ConditionalExpression : Nonterminal
    {
        public readonly BoolOrExpression BoolOrValue;

        public ConditionalExpression (object value)
        {
            this.BoolOrValue = (BoolOrExpression)value;
        }

        public override string ToString()
        {
            return this.BoolOrValue.ToString();
        }
    }

    class ConditionalExpressionConditional : ConditionalExpression
    { 
        public readonly BoolOrExpression BoolOrCondition;
        public readonly BoolOrExpression BoolOrElse;

        public ConditionalExpressionConditional(object value, object condition, object @else)
            : base(value)
        {
            this.BoolOrCondition = (BoolOrExpression)condition;
            this.BoolOrElse = (BoolOrExpression)@else;
        }

        public override string ToString()
        {
            return string.Format("{0} ? {1} : {2}", this.BoolOrCondition, this.BoolOrValue, this.BoolOrElse);
        }
    }

    class ConditionalExpressionIf : ConditionalExpression
    { 
        public readonly BoolOrExpression BoolOrCondition;
        public readonly BoolOrExpression BoolOrElse;

        public ConditionalExpressionIf(object value, object condition, object @else)
            : base(value)
        {
            this.BoolOrCondition = (BoolOrExpression)condition;
            this.BoolOrElse = (BoolOrExpression)@else;
        }

        public override string ToString()
        {
            return string.Format("{0} if {1} else {2}", this.BoolOrValue, this.BoolOrCondition, this.BoolOrElse);
        }
    }
}

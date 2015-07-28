//
// ComparisonExpression.cs
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
    abstract class ComparisonExpression : Nonterminal
    { 
    }

    class ComparisonExpressionOr : ComparisonExpression
    {
        public readonly OrExpression Or;

        public ComparisonExpressionOr(object or)
        {
            this.Or = (OrExpression)or;
        }

        public override string ToString()
        {
            return this.Or.ToString();
        }
    }

    class ComparisonExpressionComparison : ComparisonExpression
    {
        public readonly OrExpression Or0;
        public readonly OrExpression Or1;
        public readonly ComparisonOperator Op;

        public ComparisonExpressionComparison(object or0, object op, object or1)
        {
            this.Or0 = (OrExpression)or0;
            this.Op = (ComparisonOperator)op;
            this.Or1 = (OrExpression)or1;
        }

        public override string ToString()
        {
            return string.Format("{0} {2} {1}", this.Or0, this.Or1, this.Op);
        }
    }

    class ComparisonOperator : Nonterminal
    {
        public readonly Lexer.Token Token;
        public readonly ComparisonType Type;

        public ComparisonOperator(object token, ComparisonType type)
        {
            this.Token = (Lexer.Token)token;
            this.Type = type;
        }

        public override string ToString()
        {
            return this.Token.Text;
        }
    }

    class TwoComparisonOperator : ComparisonOperator
    {
        public readonly Lexer.Token TokenSecond;

        public TwoComparisonOperator(object token, object token2, ComparisonType type)
            : base(token, type)
        {
            this.TokenSecond = (Lexer.Token)token2;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Token.Text, this.Token.Text);
        }
    }

    enum ComparisonType
    {
        LessThan,
        MoreThan,
        Equal,
        LessThanEqual,
        MoreThanEqual,
        NotEqual,
        Is,
        IsNot
    }
}


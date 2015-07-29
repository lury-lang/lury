//
// BoolOrExpression.cs
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
    abstract class BoolOrExpression : Nonterminal
    { 
    }

    class BoolOrExpressionAnd : BoolOrExpression
    {
        public readonly BoolAndExpression BoolAnd;

        public BoolOrExpressionAnd(object boolAnd)
        {
            this.BoolAnd = (BoolAndExpression)boolAnd;
        }

        public override string ToString()
        {
            return this.BoolAnd.ToString();
        }
    }

    class BoolOrExpressionOr : BoolOrExpression
    {
        public readonly Lexer.Token Token;
        public readonly BoolOrExpression BoolOr;
        public readonly BoolAndExpression BoolAnd;

        public BoolOrExpressionOr(object boolOr, object token, object boolAnd)
        {
            this.BoolOr = (BoolOrExpression)boolOr;
            this.Token = (Lexer.Token)token;
            this.BoolAnd = (BoolAndExpression)boolAnd;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.BoolOr, this.Token.Text, this.BoolAnd);
        }
    }
}

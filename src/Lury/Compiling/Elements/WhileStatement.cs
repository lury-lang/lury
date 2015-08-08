//
// ElseStatement.cs
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
using Lury.Type;
using Lury.Runtime;

namespace Lury.Compiling.Elements
{
    class WhileStatement : CompundStatement
    {
        public readonly Lexer.Token While;
        public readonly Expression Expression;
        public readonly Suite Suite;
        public readonly ElseStatement Else;

        public WhileStatement(object @while, object expression, object suite, object @else)
        {
            this.While = (Lexer.Token)@while;
            this.Expression = (Expression)expression;
            this.Suite = (Suite)suite;
            this.Else = (ElseStatement)@else;
        }

        public WhileStatement(object @while, object expression, object suite)
            : this(@while, expression, suite, null)
        {
        }

        public override string ToString()
        {
            if (this.Else != null)
                return string.Format("{0} {1}:{2}{3}", this.While, this.Expression, this.Suite, this.Else);
            else
                return string.Format("{0} {1}:{2}", this.While, this.Expression, this.Suite);
        }

        public override LuryObject Evaluate(LuryHost host)
        {
            throw new NotImplementedException();
        }
    }
}

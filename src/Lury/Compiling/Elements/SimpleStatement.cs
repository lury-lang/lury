//
// SimpleStatement.cs
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
    abstract class SimpleStatement : Nonterminal
    {
    }

    abstract class SimpleStatementTerm : SimpleStatement
    {
        public readonly Terminal Term;

        public SimpleStatementTerm(object term)
        {
            this.Term = (Terminal)term;
        }

        public override string ToString()
        {
            return this.Term.ToString();
        }
    }

    class StatementList : Nonterminal
    {
        public readonly SimpleStatement SimpleStatement;
        public readonly SimpleStatements SimpleStatements;

        public StatementList(object simpleStatement, object simpleStatements)
        {
            this.SimpleStatement = (SimpleStatement)simpleStatement;
            this.SimpleStatements = (SimpleStatements)simpleStatements;
        }

        public StatementList(object simpleStatement)
            : this(simpleStatement, null)
        {
        }

        public override string ToString()
        {
            if (this.SimpleStatements != null)
                return string.Format("{0}{1}", this.SimpleStatement, this.SimpleStatements);
            else
                return this.SimpleStatement.ToString();
        }
    }

    class SimpleStatements : Nonterminal, IElementList
    {
        public readonly SimpleStatement SimpleStatement;
        public readonly SimpleStatements NextSimpleStatements;

        public bool HasNextElement { get { return this.NextSimpleStatements != null; } }

        public SimpleStatements(object simpleStatement, object nextSimpleStatements)
        {
            this.SimpleStatement = (SimpleStatement)simpleStatement;
            this.NextSimpleStatements = (SimpleStatements)nextSimpleStatements;
        }

        public SimpleStatements()
        {
            // There is no simple statement.
        }

        public override string ToString()
        {
            if (this.HasNextElement)
                return string.Format("; {0}{1}", this.SimpleStatement, this.NextSimpleStatements);
            else
                return ";";
        }
    }
}

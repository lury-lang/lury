//
// CharPosition.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2014 13y-yamamoto
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

namespace Lury
{
    public struct CharPosition
    {
        #region -- Private Static Fields --

        private static readonly CharPosition empty = new CharPosition();

        private static readonly CharPosition basePosition = new CharPosition(1, 1);

        #endregion

        #region -- Private Fields --

        private int line;

        private int column;

        #endregion

        #region -- Public Properties --

        public int Line
        { 
            get { return this.line; }
            set
            { 
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                this.line = value;
            }
        }

        public int Column
        { 
            get { return this.column; }
            set
            { 
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                this.column = value;
            }
        }

        public bool IsEmpty
        {
            get { return this == empty; }
        }

        #endregion

        #region -- Public Static Properties --

        public static CharPosition Empty { get { return empty; } }

        public static CharPosition BasePosition { get { return basePosition; } }

        #endregion

        #region -- Constructor --

        public CharPosition(int line, int column)
        {
            if (line < 1)
                throw new ArgumentOutOfRangeException("line");

            if (column < 1)
                throw new ArgumentOutOfRangeException("column");

            this.line = line;
            this.column = column;
        }

        #endregion

        #region -- Public Methods --

        public override string ToString()
        {
            return string.Format("({0}, {1})", this.line, this.column);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CharPosition))
                return false;

            CharPosition x = (CharPosition)obj;

            return (x.line == this.line) && (x.column == this.column);
        }

        public override int GetHashCode()
        {
            return this.line ^ this.column;
        }

        #endregion

        #region -- Public Static Methods --

        public static bool operator ==(CharPosition cp1, CharPosition　cp2)
        {
            return cp1.line == cp2.line && cp1.column == cp2.column;
        }

        public static bool operator !=(CharPosition cp1, CharPosition　cp2)
        {
            return cp1.line != cp2.line && cp1.column != cp2.column;
        }

        #endregion
    }
}


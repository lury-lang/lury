//
// Literal.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2014-2015 Tomona Nanase
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
using System.Text;

namespace Lury.Compiling.Element
{
    abstract class Literal<T>
    {
        public T Value { get; private set; } 

        public Literal(T value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }

    class StringLiteral : Literal<string>
    {
        public StringLiteral(string value, char marker)
            : base(ConvertToInternalString(value, marker))
        {
        }

        private static string ConvertToInternalString(string value, char marker)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            if (value.Length < 2)
                throw new ArgumentException("value");

            var sb = new StringBuilder(value);
            TrimMarker(sb, marker);
            ReplaceEscapeChar(sb);

            return sb.ToString();
        }
       
        private static void TrimMarker(StringBuilder value, char marker)
        {
            if (value[0] != marker || value[value.Length - 1] != marker)
                throw new ArgumentException("value");

            value.Remove(0, 1);
            value.Remove(value.Length - 1, 1);
        }

        private static void ReplaceEscapeChar(StringBuilder value)
        {
            value.Replace(@"\\", "\\");
            value.Replace(@"\'", "'");
            value.Replace(@"\""", "\"");
            value.Replace(@"\a", "\a");
            value.Replace(@"\b", "\b");
            value.Replace(@"\f", "\f");
            value.Replace(@"\n", "\n");
            value.Replace(@"\r", "\r");
            value.Replace(@"\t", "\t");
            value.Replace(@"\v", "\v");

            // TODO: Replace Unicode EscapeSequence
        }
    }

    class ImaginaryNumberLiteral : Literal<double>
    {
        public ImaginaryNumberLiteral(string value)
            : base(ConvertToDouble(value))
        {
        }

        public override string ToString()
        {
            return this.Value.ToString() + "i";
        }

        private static double ConvertToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("value");

            if (value[value.Length - 1] != 'i')
                throw new ArgumentException("value");

            return double.Parse(value.Substring(0, value.Length - 1).Replace("_", ""));
        }
    }

    class FloatingNumberLiteral : Literal<double>
    {
        public FloatingNumberLiteral(string value)
            : base(ConvertToDouble(value))
        {
        }

        private static double ConvertToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("value");

            return double.Parse(value.Replace("_", ""));
        }
    }

    class IntegerLiteral : Literal<long>
    {
        public IntegerLiteral(string value)
            : base(ConvertToInt64(value))
        {
        }

        private static long ConvertToInt64(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("value");

            value = value.Replace("_", "");

            if (value.StartsWith("0x", true, null))
                return Convert.ToInt64(value.Substring(2), 16);
               
            if (value.StartsWith("0o", true, null))
                return Convert.ToInt64(value.Substring(2), 8);

            if (value.StartsWith("0b", true, null))
                return Convert.ToInt64(value.Substring(2), 2);

            return long.Parse(value);
        }
    }
}


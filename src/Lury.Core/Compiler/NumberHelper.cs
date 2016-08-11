/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Tomona Nanase
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 */

using System;
using System.Numerics;

namespace Lury.Core.Compiler
{
    internal static class NumberHelper
    {
        #region -- Public Static Methods --

        public static BigInteger ForLuryInteger(this string input)
        {
            input = input.Replace("_", "");

			if (input.StartsWith("0x", StringComparison.Ordinal))
                return Convert.ToInt64(input.Substring(2), 16);

            if (input.StartsWith("0o", StringComparison.Ordinal))
                return Convert.ToInt64(input.Substring(2), 8);

            if (input.StartsWith("0b", StringComparison.Ordinal))
                return Convert.ToInt64(input.Substring(2), 2);

            return BigInteger.Parse(input);
        }

        public static double ForLuryReal(this string input)
        {
            input = input.Replace("_", "");
            return double.Parse(input);
        }

        #endregion
    }
}

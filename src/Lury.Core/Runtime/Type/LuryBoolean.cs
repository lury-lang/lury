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
using static Lury.Core.Runtime.Type.IntrinsicConstants;

namespace Lury.Core.Runtime.Type
{
    [IntrinsicClass(FullName, TypeName)]
    public class LuryBoolean : LuryObject
    {
        #region -- Public Fields --

        public const string FullName = "lury.core.Boolean";
        public const string TypeName = "Boolean";

        public static readonly LuryBoolean True = new LuryBoolean(true);
        public static readonly LuryBoolean False = new LuryBoolean(false);

        #endregion

        #region -- Constructors --

        private LuryBoolean(bool value)
            : base(FullName, value, true)
        {
        }

        #endregion

        #region -- Public Static Methods --

        [Intrinsic(OperatorEq)]
        public static LuryObject Equals(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return self.Value == other.Value ? True : False;
        }

        [Intrinsic(OperatorNe)]
        public static LuryObject NotEqual(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return self.Value != other.Value ? True : False;
        }

        [Intrinsic(OperatorAnd)]
        public static LuryObject And(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return (bool)self.Value & (bool)other.Value ? True : False;
        }

        [Intrinsic(OperatorXor)]
        public static LuryObject Xor(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return (bool)self.Value ^ (bool)other.Value ? True : False;
        }

        [Intrinsic(OperatorOr)]
        public static LuryObject Or(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return (bool)self.Value | (bool)other.Value ? True : False;
        }

        [Intrinsic(OperatorNot)]
        public static LuryObject Not(LuryObject self)
        {
            return (bool)self.Value ? False : True;
        }

        #endregion
    }
}

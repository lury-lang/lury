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
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using static Lury.Core.Runtime.Type.IntrinsicConstants;
using static Lury.Core.Runtime.Type.LuryBoolean;

namespace Lury.Core.Runtime.Type
{
    [IntrinsicClass(FullName, TypeName)]
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class LuryInteger : LuryObject
    {
        #region -- Public Fields --

        public const string FullName = "lury.core.Integer";
        public const string TypeName = "Integer";

        #endregion

        #region -- Constructors --

        private LuryInteger(BigInteger value)
            : base(FullName, value, true)
        {
        }

        #endregion

        #region -- Public Static Methods --

        public static LuryInteger GetObject(BigInteger value) => new LuryInteger(value);

        [Intrinsic(OperatorPow)]
        public static LuryObject Pow(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
            {
                var exponent = (BigInteger)other.Value;

                if (exponent > int.MaxValue || exponent < int.MinValue)
                    return LuryReal.GetObject(Math.Pow((double)(BigInteger)self.Value, (double)exponent));

                return GetObject(BigInteger.Pow((BigInteger)self.Value, (int)exponent));
            }

            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject(Math.Pow((double)(BigInteger)self.Value, (double)other.Value));

            throw new ArgumentException();
        }

        [Intrinsic(OperatorInc)]
        public static LuryObject Inc(LuryObject self)
        {
            return GetObject((BigInteger)self.Value + 1);
        }

        [Intrinsic(OperatorDec)]
        public static LuryObject Dec(LuryObject self)
        {
            return GetObject((BigInteger)self.Value - 1);
        }

        [Intrinsic(OperatorPos)]
        public static LuryObject Pos(LuryObject self)
        {
            return self;
        }

        [Intrinsic(OperatorNeg)]
        public static LuryObject Neg(LuryObject self)
        {
            return GetObject(-(BigInteger)self.Value);
        }

        [Intrinsic(OperatorInv)]
        public static LuryObject Inv(LuryObject self)
        {
            return GetObject(~(BigInteger)self.Value);
        }

        [Intrinsic(OperatorMul)]
        public static LuryObject Mul(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return GetObject((BigInteger)self.Value * (BigInteger)other.Value);

            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value * (double)other.Value);

            throw new ArgumentException();
        }

        [Intrinsic(OperatorDiv)]
        public static LuryObject Div(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            if (other.LuryTypeName == FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value / (double)(BigInteger)other.Value);
            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value / (double)other.Value);

            throw new ArgumentException();
        }

        [Intrinsic(OperatorIntDiv)]
        public static LuryObject IntDiv(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            if (other.LuryTypeName == FullName)
                return GetObject((BigInteger)self.Value / (BigInteger)other.Value);

            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value / (double)other.Value);

            throw new ArgumentException();
        }

        [Intrinsic(OperatorMod)]
        public static LuryObject Mod(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return GetObject((BigInteger)self.Value % (BigInteger)other.Value);

            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value % (double)other.Value);

            throw new ArgumentException();
        }

        [Intrinsic(OperatorAdd)]
        public static LuryObject Add(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return GetObject((BigInteger)self.Value + (BigInteger)other.Value);

            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value + (double)other.Value);

            throw new ArgumentException();
        }

        [Intrinsic(OperatorSub)]
        public static LuryObject Sub(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return GetObject((BigInteger)self.Value - (BigInteger)other.Value);

            if (other.LuryTypeName == LuryReal.FullName)
                return LuryReal.GetObject((double)(BigInteger)self.Value - (double)other.Value);

            throw new ArgumentException();
        }

        [Intrinsic(OperatorLShift)]
        public static LuryObject LShift(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            return GetObject((BigInteger)self.Value << (int)((BigInteger)other.Value));
        }

        [Intrinsic(OperatorRShift)]
        public static LuryObject RShift(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            return GetObject((BigInteger)self.Value >> (int)((BigInteger)other.Value));
        }

        [Intrinsic(OperatorAnd)]
        public static LuryObject And(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            return GetObject((BigInteger)self.Value & (BigInteger)other.Value);
        }

        [Intrinsic(OperatorXor)]
        public static LuryObject Xor(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            return GetObject((BigInteger)self.Value ^ (BigInteger)other.Value);
        }

        [Intrinsic(OperatorOr)]
        public static LuryObject Or(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != FullName)
                throw new ArgumentException();

            return GetObject((BigInteger)self.Value | (BigInteger)other.Value);
        }


        [Intrinsic(OperatorEq)]
        public static LuryObject Equals(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return (BigInteger)self.Value == (BigInteger)other.Value ? True : False;

            if (other.LuryTypeName == LuryReal.FullName)
                return (double)(BigInteger)self.Value == (double)(BigInteger)other.Value ? True : False;

            throw new ArgumentException();
        }

        [Intrinsic(OperatorNe)]
        public static LuryObject NotEqual(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return (BigInteger)self.Value != (BigInteger)other.Value ? True : False;

            if (other.LuryTypeName == LuryReal.FullName)
                return (double)(BigInteger)self.Value != (double)(BigInteger)other.Value ? True : False;

            throw new ArgumentException();
        }

        [Intrinsic(OperatorLt)]
        public static LuryObject Lt(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return (BigInteger)self.Value < (BigInteger)other.Value ? True : False;

            if (other.LuryTypeName == LuryReal.FullName)
                return (double)(BigInteger)self.Value < (double)(BigInteger)other.Value ? True : False;

            throw new ArgumentException();
        }

        [Intrinsic(OperatorLtq)]
        public static LuryObject Ltq(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return (BigInteger)self.Value <= (BigInteger)other.Value ? True : False;

            if (other.LuryTypeName == LuryReal.FullName)
                return (double)(BigInteger)self.Value <= (double)(BigInteger)other.Value ? True : False;

            throw new ArgumentException();
        }

        [Intrinsic(OperatorGt)]
        public static LuryObject Gt(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return (BigInteger)self.Value > (BigInteger)other.Value ? True : False;

            if (other.LuryTypeName == LuryReal.FullName)
                return (double)(BigInteger)self.Value > (double)(BigInteger)other.Value ? True : False;

            throw new ArgumentException();
        }

        [Intrinsic(OperatorGtq)]
        public static LuryObject Gtq(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName == FullName)
                return (BigInteger)self.Value >= (BigInteger)other.Value ? True : False;

            if (other.LuryTypeName == LuryReal.FullName)
                return (double)(BigInteger)self.Value >= (double)(BigInteger)other.Value ? True : False;

            throw new ArgumentException();
        }

        #endregion
    }
}

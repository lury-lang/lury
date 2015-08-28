//
// LuryReal.cs
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

namespace Lury.Objects
{
    public class LuryReal : LuryNumber
    {
        private readonly double value;

        public double Value　{ get { return this.value; } }

        public LuryReal(double value)
        {
            this.value = value;  
        }

        public override LuryObject Inc()
        {
            return new LuryReal(this.value + 1.0);
        }

        public override LuryObject Dec()
        {
            return new LuryReal(this.value - 1.0);
        }

        public override LuryObject Pow(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(Math.Pow(this.value, ((LuryInteger)other).Value));

            if (other is LuryReal)
                return new LuryReal(Math.Pow(this.value, ((LuryReal)other).value));

            if (other is LuryComplex)
            {
                var or = ((LuryComplex)other).Real;
                var oi = ((LuryComplex)other).Imag;

                var log_zr = Math.Log(this.value);
                var log_zi = Math.Atan2(0.0, this.value);

                var a_log_zr = or * log_zr - log_zi * oi;
                var a_log_zi = oi * log_zr + or * log_zi;

                return new LuryComplex(Math.Exp(a_log_zr) * Math.Cos(a_log_zi), Math.Exp(a_log_zr) * Math.Sin(a_log_zi));
            }

            throw new NotSupportedException();
        }

        public override LuryObject Pos()
        {
            return this;
        }

        public override LuryObject Neg()
        {
            return new LuryReal(-this.value);
        }

        public override LuryObject Mul(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(this.value * ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryReal(this.value　* ((LuryReal)other).value);

            if (other is LuryComplex)
                return new LuryComplex(this.value　* ((LuryComplex)other).Real, this.value * ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryObject Div(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(this.value / ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryReal(this.value　/ ((LuryReal)other).value);

            if (other is LuryComplex)
            {
                var o = (LuryComplex)other;
                var icd2 = 1.0 / (o.Real * o.Real + o.Imag * o.Imag);
                return new LuryComplex((this.value * o.Real) * icd2, (-this.value * o.Imag) * icd2);
            }

            throw new NotSupportedException();
        }

        public override LuryObject IDiv(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(this.value / ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryReal((double)(long)((double)this.value　/ ((LuryReal)other).Value));

            if (other is LuryComplex)
            {
                var o = (LuryComplex)other;
                var icd2 = 1.0 / (o.Real * o.Real + o.Imag * o.Imag);
                return new LuryComplex((double)(long)((this.value * o.Real) * icd2), (double)(long)((-this.value * o.Imag) * icd2));
            }

            throw new NotSupportedException();
        }

        public override LuryObject Mod(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(this.value % ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryReal(this.value　% ((LuryReal)other).value);

            throw new NotSupportedException();
        }

        public override LuryObject Add(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(this.value + ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryReal(this.value　+ ((LuryReal)other).value);

            if (other is LuryComplex)
                return new LuryComplex(this.value　+ ((LuryComplex)other).Real, ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryObject Sub(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryReal(this.value - ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryReal(this.value　- ((LuryReal)other).value);

            if (other is LuryComplex)
                return new LuryComplex(this.value　- ((LuryComplex)other).Real, ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryBoolean CLt(LuryObject other)
        {
            if (other is LuryInteger)
                return this.value < (double)((LuryInteger)other).Value ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return this.value < ((LuryReal)other).Value ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override LuryBoolean CGt(LuryObject other)
        {
            if (other is LuryInteger)
                return this.value > (double)((LuryInteger)other).Value ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return this.value > ((LuryReal)other).Value ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override LuryBoolean CEq(LuryObject other)
        {
            if (other is LuryInteger)
                return this.value == (double)((LuryInteger)other).Value ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return this.value == ((LuryReal)other).Value ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}


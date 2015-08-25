//
// LuryInteger.cs
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
    public class LuryInteger : LuryNumber
    {
        private readonly long value;

        public long Value　{ get { return this.value; } }

        public LuryInteger(long value)
        {
            this.value = value;  
        }

        public override LuryObject Inc()
        {
            return new LuryInteger(this.value + 1);
        }

        public override LuryObject Dec()
        {
            return new LuryInteger(this.value - 1);
        }

        public override LuryObject Pow(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger((long)Math.Pow(this.value, ((LuryInteger)other).value));

            if (other is LuryReal)
                return new LuryReal(Math.Pow(this.value, ((LuryReal)other).Value));

            throw new NotSupportedException();
        }

        public override LuryObject Pos()
        {
            return this;
        }

        public override LuryObject Neg()
        {
            return new LuryInteger(-this.value);
        }

        public override LuryObject BNot()
        {
            return new LuryInteger(~this.value);
        }

        public override LuryObject Mul(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value * ((LuryInteger)other).value);

            if (other is LuryReal)
                return new LuryReal(this.value　* ((LuryReal)other).Value);

            if (other is LuryComplex)
                return new LuryComplex(this.value　* ((LuryComplex)other).Real, this.value * ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryObject Div(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value / ((LuryInteger)other).value);

            if (other is LuryReal)
                return new LuryReal(this.value　/ ((LuryReal)other).Value);

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
                return new LuryInteger(this.value / ((LuryInteger)other).value);

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
                return new LuryInteger(this.value % ((LuryInteger)other).value);

            if (other is LuryReal)
                return new LuryReal(this.value　% ((LuryReal)other).Value);

            throw new NotSupportedException();
        }

        public override LuryObject Add(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value + ((LuryInteger)other).value);

            if (other is LuryReal)
                return new LuryReal(this.value　+ ((LuryReal)other).Value);

            if (other is LuryComplex)
                return new LuryComplex(this.value　+ ((LuryComplex)other).Real, ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryObject Sub(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value - ((LuryInteger)other).value);

            if (other is LuryReal)
                return new LuryReal(this.value　- ((LuryReal)other).Value);

            if (other is LuryComplex)
                return new LuryComplex(this.value　- ((LuryComplex)other).Real, ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryObject Shl(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value << (int)((LuryInteger)other).value);

            throw new NotSupportedException();
        }

        public override LuryObject Shr(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value >> (int)((LuryInteger)other).value);

            throw new NotSupportedException();
        }

        public override LuryObject BAnd(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value & ((LuryInteger)other).value);

            throw new NotSupportedException();
        }

        public override LuryObject BXor(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value ^ ((LuryInteger)other).value);
            
            throw new NotSupportedException();
        }

        public override LuryObject BOr(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryInteger(this.value | ((LuryInteger)other).value);
            
            throw new NotSupportedException();
        }

        public override LuryBoolean CLt(LuryObject other)
        {
            if (other is LuryInteger)
                return this.value < ((LuryInteger)other).value ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return (double)this.value < ((LuryReal)other).Value ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override LuryBoolean CGt(LuryObject other)
        {
            if (other is LuryInteger)
                return this.value > ((LuryInteger)other).value ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return (double)this.value > ((LuryReal)other).Value ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override LuryBoolean CEq(LuryObject other)
        {
            if (other is LuryInteger)
                return this.value == ((LuryInteger)other).value ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return (double)this.value == ((LuryReal)other).Value ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}


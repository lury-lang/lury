//
// LuryComplex.cs
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
    public class LuryComplex : LuryNumber
    {
        private readonly double real, imag;

        public double Real　{ get { return this.real; } }

        public double Imag　{ get { return this.imag; } }

        public LuryComplex(double real, double imag)
        {
            this.real = real;
            this.imag = imag;
        }

        public override LuryObject Pow(LuryObject other)
        {
            throw new NotSupportedException();
        }

        public override LuryObject Pos()
        {
            return this;
        }

        public override LuryObject Neg()
        {
            return new LuryComplex(-this.real, -this.imag);
        }

        public override LuryObject Mul(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryComplex(this.real　* ((LuryInteger)other).Value, this.imag * ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryComplex(this.real　* ((LuryReal)other).Value, this.imag * ((LuryReal)other).Value);

            if (other is LuryComplex)
            {
                var o = (LuryComplex)other;
                return new LuryComplex(this.real * o.Real - this.imag * o.imag, this.imag * o.real + this.real * o.imag);
            }

            throw new NotSupportedException();
        }

        public override LuryObject Div(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryComplex(this.real　/ ((LuryInteger)other).Value, this.imag / ((LuryInteger)other).Value);

            if (other is LuryReal)
                return new LuryComplex(this.real　/ ((LuryReal)other).Value, this.imag / ((LuryReal)other).Value);

            if (other is LuryComplex)
            {
                var o = (LuryComplex)other;
                var icd2 = 1.0 / (o.real * o.real + o.imag * o.imag);
                return new LuryComplex((this.real * o.Real + this.imag * o.imag) * icd2, (this.imag * o.real - this.real * o.imag) * icd2);
            }

            throw new NotSupportedException();
        }

        public override LuryObject IDiv(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryComplex((double)(long)(this.real　/ ((LuryInteger)other).Value), (double)(long)(this.imag / ((LuryInteger)other).Value));

            if (other is LuryReal)
                return new LuryComplex((double)(long)(this.real　/ ((LuryReal)other).Value), (double)(long)(this.imag / ((LuryReal)other).Value));

            if (other is LuryComplex)
            {
                var o = (LuryComplex)other;
                var icd2 = 1.0 / (o.real * o.real + o.imag * o.imag);
                return new LuryComplex((double)(long)((this.real * o.Real + this.imag * o.imag) * icd2), (double)(long)((this.imag * o.real - this.real * o.imag) * icd2));
            }

            throw new NotSupportedException();
        }

        public override LuryObject Add(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryComplex(this.real　+ ((LuryInteger)other).Value, this.imag);

            if (other is LuryReal)
                return new LuryComplex(this.real　+ ((LuryReal)other).Value, this.imag);

            if (other is LuryComplex)
                return new LuryComplex(this.real　+ ((LuryComplex)other).Real, this.imag　+ ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryObject Sub(LuryObject other)
        {
            if (other is LuryInteger)
                return new LuryComplex(this.real　- ((LuryInteger)other).Value, this.imag);

            if (other is LuryReal)
                return new LuryComplex(this.real　- ((LuryReal)other).Value, this.imag);

            if (other is LuryComplex)
                return new LuryComplex(this.real　- ((LuryComplex)other).Real, this.imag　- ((LuryComplex)other).Imag);

            throw new NotSupportedException();
        }

        public override LuryBoolean CEq(LuryObject other)
        {
            if (other is LuryInteger)
                return this.real == (double)((LuryInteger)other).Value && this.imag == 0.0 ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryReal)
                return this.real == ((LuryReal)other).Value && this.imag == 0.0 ? LuryBoolean.True : LuryBoolean.False;

            if (other is LuryComplex)
                return this.real == ((LuryComplex)other).real && this.imag == ((LuryComplex)other).imag ? LuryBoolean.True : LuryBoolean.False;

            throw new NotSupportedException();
        }

        public override string ToString()
        {
            if (double.IsNaN(this.real) || double.IsNaN(this.imag))
                return "NaN";

            var imag = this.imag >= 0.0 ? '+' + this.imag.ToString() : this.imag.ToString();
            return '(' + this.real.ToString() + imag + "i)";
        }
    }
}


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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Lury.Core.Runtime.Type.IntrinsicConstants;
using static Lury.Core.Runtime.Type.LuryBoolean;
using LList = System.Collections.Generic.List<Lury.Core.Runtime.LuryObject>;

namespace Lury.Core.Runtime.Type
{
    [IntrinsicClass(FullName, TypeName)]
    public class LuryList : LuryObject
    {
        #region -- Public Fields --

        public const string FullName = "lury.core.List";
        public const string TypeName = "List";

        #endregion

        #region -- Constructors --

        private LuryList(LList value)
            : base(FullName, value, true)
        {
        }

        #endregion

        #region -- Public Methods --

        public override string ToString()
        {
            return $"[{string.Join(", ", (LList)Value)}]";
        }

        #endregion

        #region -- Public Static Methods --

        public static LuryList GetObject(params LuryObject[] objects) => GetObject((IEnumerable<LuryObject>)objects);

        public static LuryList GetObject(LList objects) => new LuryList(objects);

        public static LuryList GetObject(IEnumerable<LuryObject> objects) => new LuryList(new LList(objects));

        [Intrinsic(OperatorEq)]
        public static LuryObject Equals(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return ((LList)self.Value).SequenceEqual((LList)other.Value) ? True : False;
        }

        [Intrinsic(OperatorNe)]
        public static LuryObject NotEqual(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return !((LList)self.Value).SequenceEqual((LList)other.Value) ? True : False;
        }

        [Intrinsic(OperatorCon)]
        public static LuryObject Con(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return GetObject(((LList)self.Value).Concat((LList)other.Value));
        }

        [Intrinsic(OperatorSub)]
        public static LuryObject Sub(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return GetObject(((LList)self.Value).Except((LList)other.Value));
        }

        [Intrinsic(OperatorAnd)]
        public static LuryObject And(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return GetObject(((LList)self.Value).Intersect((LList)other.Value));
        }

        [Intrinsic(OperatorXor)]
        public static LuryObject Xor(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            var or = ((LList)self.Value).Union((LList)other.Value);
            var and = ((LList)self.Value).Intersect((LList)other.Value);

            return GetObject(or.Except(and));
        }

        [Intrinsic(OperatorOr)]
        public static LuryObject Or(LuryObject self, LuryObject other)
        {
            if (other.LuryTypeName != TypeName)
                throw new ArgumentException();

            return GetObject(((LList)self.Value).Union((LList)other.Value));
        }

        [Intrinsic(OperatorIn)]
        public static LuryObject In(LuryObject self, LuryObject other)
        {
            return ((LList)self.Value).Contains(other) ? True : False;
        }

        [Intrinsic(OperatorGetIdx)]
        public static LuryObject GetIndex(LuryObject self, LuryObject other)
        {
            var indexObject = other as LuryInteger;

            if (indexObject == null)
                throw new InvalidOperationException();

            var index = (BigInteger)indexObject.Value;
            var llist = (LList)self.Value;

            if (index < 0 || index >= llist.Count)
                throw new InvalidOperationException();

            return llist[(int)index];
        }

        [Intrinsic(OperatorSetIdx)]
        public static LuryObject SetIndex(LuryObject self, LuryObject other, LuryObject element)
        {
            var indexObject = other as LuryInteger;

            if (indexObject == null)
                throw new InvalidOperationException();

            var index = (BigInteger)indexObject.Value;
            var llist = (LList)self.Value;

            if (index < 0 || index > int.MaxValue)
                throw new InvalidOperationException();

            return llist[(int)index] = element;
        }

        #endregion
    }
}

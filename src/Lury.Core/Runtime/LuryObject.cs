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

namespace Lury.Core.Runtime
{
    public class LuryObject
    {
        #region -- Private Fields --

        private readonly Dictionary<string, LuryObject> members = new Dictionary<string, LuryObject>(0);

        #endregion

        #region -- Public Properties --

        public string LuryTypeName { get; }

        public object Value { get; }

        public bool IsFrozen { get; private set; }

        #endregion

        #region -- Constructors --

        public LuryObject(string luryTypeName, object value, bool freeze = false)
        {
            LuryTypeName = luryTypeName;
            Value = value;

            if (freeze)
                Freeze();
        }

        #endregion

        #region -- Public Methods --

        public void SetMember(string name, LuryObject obj)
        {
            if (IsFrozen)
                throw new InvalidOperationException();

            if (members.ContainsKey(name))
                members[name] = obj;
            else
                members.Add(name, obj);
        }

        public LuryObject GetMember(string name, LuryContext context)
        {
            if (members.ContainsKey(name))
                return members[name];

            if (LuryTypeName != null && context.HasMember(LuryTypeName))
                return context[LuryTypeName].GetMemberNoRecursion(name);

            //throw new LuryException(LuryExceptionType.NameIsNotFound);
            throw new InvalidOperationException();
        }

        public bool HasMember(string member)
        {
            return members.ContainsKey(member);
        }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public override string ToString() => Value.ToString();

        public override bool Equals(object obj)
        {
            var other = obj as LuryObject;

            if (other == null)
                return false;

            return Value.Equals(other.Value) && LuryTypeName.Equals(other.LuryTypeName);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ LuryTypeName.GetHashCode();
        }

        #endregion

        #region -- Private Methods --

        private LuryObject GetMemberNoRecursion(string name)
        {
            if (members.ContainsKey(name))
                return members[name];

            //throw new LuryException(LuryExceptionType.NameIsNotFound);
            throw new InvalidOperationException();
        }

        #endregion
    }
}

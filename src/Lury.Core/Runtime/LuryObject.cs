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

using System.Collections.Generic;
using Antlr4.Runtime;
using Lury.Core.Compiler;
using Lury.Core.Error;

namespace Lury.Core.Runtime
{
    public class LuryObject : ILuryAccessible
    {
        #region -- Private Fields --

        private readonly Dictionary<string, LuryObject> attributes = new Dictionary<string, LuryObject>(0);

        #endregion

        #region -- Public Properties --

        public LuryObject BaseObject { get; }

        public LuryObject Class { get; }

        public object Value { get; }

        public bool IsFrozen { get; private set; }

        #endregion

        #region -- Constructors --

        public LuryObject(LuryObject baseObject, LuryObject @class, object value = null, bool freeze = false)
        {
            BaseObject = baseObject;
            Class = @class;
            Value = value;

            if (freeze)
                Freeze();
        }

        #endregion

        #region -- Public Methods --

        public void Assign(string target, LuryObject data)
        {
            Assign(new CommonToken(LuryLexer.NAME, target), data);
        }

        public bool Has(string target)
        {
            var targetObject = this;

            while (true)
            {
                if (targetObject.attributes.ContainsKey(target))
                    return true;

                if (targetObject.BaseObject != null)
                    targetObject = targetObject.BaseObject;
                else
                    return false;
            }
        }

        public LuryObject Fetch(string target)
        {
            return Fetch(new CommonToken(LuryLexer.NAME, target));
        }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public override string ToString() => Value.ToString();

        public virtual string ToInspectString() => ToString();

        // TODO: 新オブジェクトモデルに適合するよう書き換え
        public override bool Equals(object obj)
        {
            var other = obj as LuryObject;

            if (other == null)
                return false;

            return Value.Equals(other.Value);
        }

        // TODO: 新オブジェクトモデルに適合するよう書き換え
        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Class.GetHashCode();
        }

        #endregion

        #region -- Internal Methods --

        internal void Assign(IToken target, LuryObject data)
        {
            if (attributes.ContainsKey(target.Text) ||
                BaseObject == null ||
                !CheckAndAssign(target, data))
                AssignAttribute(target, data);
        }

        internal bool Has(IToken target) => Has(target.Text);

        internal LuryObject Fetch(IToken target, string ownerName = null)
        {
            var name = target.Text;
            var targetObject = this;

            while (true)
            {
                if (targetObject.attributes.ContainsKey(name))
                    return targetObject.attributes[name];

                if (targetObject.BaseObject != null)
                    targetObject = targetObject.BaseObject;
                else
                    throw new AttributeNotDefinedException(target, ownerName);
            }
        }

        #endregion

        #region -- Private Methods --

        private bool CheckAndAssign(IToken target, LuryObject data)
        {
            var targetObject = this;

            while (true)
            {
                if (targetObject.attributes.ContainsKey(target.Text))
                {
                    targetObject.AssignAttribute(target, data);
                    return true;
                }

                if (targetObject.BaseObject != null)
                    targetObject = targetObject.BaseObject;
                else
                    return false;
            }
        }

        private void AssignAttribute(IToken target, LuryObject data)
        {
            if (IsFrozen)
                throw new CantModifyException(target);

            attributes[target.Text] = data;
        }

        #endregion
    }
}

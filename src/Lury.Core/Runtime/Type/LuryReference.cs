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
    internal class LuryReference : LuryObject
    {
        #region -- Public Fields --

        public const string FullName = "lury.core.`_reference";
        public const string TypeName = "`_reference";

        #endregion

        #region -- Constructors --

        private LuryReference(Reference reference)
            : base(TypeName, reference, true)
        {
        }

        #endregion

        #region -- Public Static Methods --

        public static LuryReference Create(string @object, LuryObject key = null)
            => new LuryReference(new Reference(@object, key));

        public static LuryReference Create(LuryObject subject, string @object, LuryObject key = null)
            => new LuryReference(new Reference(subject, @object, key));

        #endregion

        #region -- Public Methods --

        public LuryObject Dereference(LuryContext context)
        {
            var reference = (Reference)Value;

            if (reference.Key != null)
                throw new NotImplementedException();

            if (reference.Subject != null)
                return reference.Subject.GetMember(reference.Object, context);

            return context[reference.Object];
        }

        public LuryObject Assign(LuryContext context, LuryObject @object)
        {
            var reference = (Reference)Value;

            if (reference.Key != null)
                throw new NotImplementedException();

            if (reference.Subject != null)
                reference.Subject.SetMember(reference.Object, @object);
            else
                context[reference.Object] = @object;

            return @object;
        }

        #endregion

        private class Reference
        {
            #region -- Public Properties --

            public LuryObject Subject { get; }

            public string Object { get; }

            public LuryObject Key { get; }

            #endregion

            #region -- Constructors --

            public Reference(string @object, LuryObject key = null)
                : this(null, @object, key)
            {
            }

            public Reference(LuryObject subject, string @object, LuryObject key = null)
            {
                Subject = subject;
                Object = @object;
                Key = key;
            }

            #endregion
        }
    }
}

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
    public class LuryContext
    {
        #region -- Private Fields --

        private readonly Dictionary<string, LuryObject> objects = new Dictionary<string, LuryObject>(0);

        #endregion

        #region -- Public Indexers --

        public LuryObject this[string member]
        {
            get
            {
                var context = this;

                while (context != null)
                {
                    if (context.objects.ContainsKey(member))
                        return context.objects[member];

                    context = context.Parent;
                }

                throw new NotDefinedException(new CommonToken(LuryLexer.NAME, member));
            }
            set
            {
                var context = this;

                while (context != null)
                {
                    if (context.objects.ContainsKey(member))
                    {
                        context.objects[member] = value;
                        return;
                    }

                    context = context.Parent;
                }

                objects.Add(member, value);
            }
        }

        #endregion

        #region -- Public Properties --

        public LuryContext Parent { get; }

        #endregion

        #region -- Constructors --

        public LuryContext(LuryContext parent)
        {
            Parent = parent;
        }

        public LuryContext()
            : this(null)
        {
        }

        #endregion

        #region -- Public Methods --

        public bool HasMember(string name)
        {
            var context = this;

            while (context != null)
            {
                if (context.objects.ContainsKey(name))
                    return true;

                context = context.Parent;
            }

            return objects.ContainsKey(name);
        }

        public void SetMemberNoRecursion(string name, LuryObject value)
        {
            objects.Add(name, value);
        }

        #endregion

        #region -- Public Static Methods --



        #endregion
    }
}

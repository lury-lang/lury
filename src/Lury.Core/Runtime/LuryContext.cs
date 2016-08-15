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
using System.Reflection;
using Lury.Core.Runtime.Type;

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

                //throw new LuryException(LuryExceptionType.NameIsNotFound);
                throw new InvalidOperationException();
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

        public static LuryContext CreateGlobalContext()
        {
            var context = new LuryContext();

            #region Intrinsic Classes

            Action<string, string, IEnumerable<Tuple<string, MethodInfo>>> setFunctionMember = (t, n, f) =>
            {
                var type = new LuryObject(t, null);

                foreach (var item in f)
                    type.SetMember(item.Item1, new LuryObject(LuryFunction.FullName, item.Item2, true));

                type.Freeze();
                context[n] = type;
                context[type.LuryTypeName] = type;
            };

            var intrinsicTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    Attribute.GetCustomAttribute(t, typeof(IntrinsicClassAttribute)) != null);

            foreach (var type in intrinsicTypes)
            {
                var attr = type.GetCustomAttribute<IntrinsicClassAttribute>();
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var funcnames = methods
                    .Select(
                        method =>
                            new
                            {
                                method,
                                attributes = method.GetCustomAttributes<IntrinsicAttribute>().Select(a => a.TargetFunction)
                            })
                    .SelectMany(_ => _.attributes, (_, funcName) => Tuple.Create(funcName, _.method)).ToList();

                if (funcnames.Count > 0)
                    setFunctionMember(attr.FullName, attr.TypeName, funcnames);
            }

            #endregion

            #region BuiltIn Functions

            var builtInFunctions = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    Attribute.GetCustomAttribute(t, typeof(BuiltInClass)) != null);

            foreach (var type in builtInFunctions)
            {
                var attr = type.GetCustomAttribute<BuiltInClass>();
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var funcnames = methods
                    .Select(
                        method =>
                            new
                            {
                                method,
                                attributes = method.GetCustomAttributes<BuiltInAttribute>().Select(a => a.FunctionName)
                            })
                    .SelectMany(_ => _.attributes, (_, funcName) => Tuple.Create(funcName, _.method)).ToList();

                foreach (var func in funcnames)
                    context[func.Item1] = LuryFunction.GetObject(func.Item2);
            }

            #endregion

            return context;
        }

        #endregion
    }
}

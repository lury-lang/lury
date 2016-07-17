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
using Lury.Core.Compiler;
using static Lury.Core.Runtime.Type.IntrinsicConstants;

namespace Lury.Core.Runtime.Type
{
    public class LuryFunction : LuryObject
    {
        #region -- Public Fields --

        public const string FullName = "lury.core.Function";
        public const string TypeName = "Function";

        #endregion

        #region -- Constructors --

        private LuryFunction(MethodInfo methodInfo)
            : base(FullName, methodInfo, true)
        {
        }

        private LuryFunction(UserFunctionInfo functionInfo)
            : base(FullName, functionInfo, true)
        {
        }

        #endregion

        #region -- Public Static Methods --

        public static LuryFunction GetObject(MethodInfo methodInfo) => new LuryFunction(methodInfo);

        public static LuryFunction GetObject(Action action) => new LuryFunction(action.Method);

        internal static LuryFunction GetObject(UserFunctionInfo functionInfo) => new LuryFunction(functionInfo);

        [Intrinsic(OperatorCall)]
        public static LuryObject Call(LuryObject self, params object[] others)
        {
            var methodInfo = self.Value as MethodInfo;

            if (methodInfo != null)
                return (LuryObject)methodInfo.Invoke(null, BindingFlags.Default, null, others, null);

            var functionInfo = self.Value as UserFunctionInfo;

            if (functionInfo != null)
            {
                var param = functionInfo.ParameterNames.ToArray();

                if (param.Length != others.Length)
                    throw new InvalidOperationException();

                var context = new LuryContext(functionInfo.ParentContext);

                for (var i = 0; i < param.Length; i++)
                    context[param[i]] = (LuryObject)others[i];

                return new LuryVisitor(context).VisitSuite(functionInfo.BodySuite);
            }

            throw new NotImplementedException();
        }

        #endregion
    }

    internal class UserFunctionInfo
    {
        #region -- Public Properties --

        public LuryParser.SuiteContext BodySuite { get; }

        public LuryContext ParentContext { get; }

        public IEnumerable<string> ParameterNames { get; }

        #endregion

        #region -- Constructors --

        public UserFunctionInfo(LuryParser.SuiteContext bodySuite, LuryContext parentContext, IEnumerable<string> parameterNames)
        {
            BodySuite = bodySuite;
            ParentContext = parentContext;
            ParameterNames = parameterNames;
        }

        #endregion
    }
}

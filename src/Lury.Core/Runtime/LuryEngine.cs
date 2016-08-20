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

using System.Runtime.CompilerServices;
using Antlr4.Runtime;
using Lury.Core.Compiler;

namespace Lury.Core.Runtime
{
    public class LuryEngine
    {
        #region -- Public Properties --

        public LuryContext Context { get; }

        #endregion

        #region -- Constructors --

        public LuryEngine()
        {
        }

        #endregion

        #region -- Public Methods --

        public LuryObject Execute(string source)
        {
            var inputStream = new AntlrInputStream(source);
            var luryLexer = new LuryLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(luryLexer);
            var luryParser = new LuryParser(commonTokenStream);
            var programContext = luryParser.program();
            
            var luryVisitor = new LuryVisitor(Context);

            return luryVisitor.VisitProgram(programContext);
        }

        #endregion

        #region -- Public Static Methods --

        public static LuryObject Run(string source)
        {
            var engine = new LuryEngine();

            return engine.Execute(source);
        }

        #endregion
    }
}

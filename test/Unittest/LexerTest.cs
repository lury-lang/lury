using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Lury.Core.Compiler;
using Lury.Core.Error;
using Lury.Core.Runtime;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using static Lury.Core.Compiler.LuryLexer;

namespace Unittest
{
    [Parallelizable]
    [TestFixture]
    public class LexerTest
    {
        [Test]
        public void IntegerTest()
        {
            Assert.That("0", IsTokenized.Under(DECIMAL_INTEGER).And.Append(IsSeparated.Into("0")));
            Assert.That("1", IsTokenized.Under(DECIMAL_INTEGER).And.Append(IsSeparated.Into("1")));
        }
    }
}

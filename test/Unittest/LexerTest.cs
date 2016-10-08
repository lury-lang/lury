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
            Assert.That(Tokens.From("0").Type, Are.EqualTo(DECIMAL_INTEGER));
            Assert.That(Tokens.From("0").Text, Are.EqualTo("0"));

            Assert.That(Tokens.From("1").Type, Are.EqualTo(DECIMAL_INTEGER));
            Assert.That(Tokens.From("1").Text, Are.EqualTo("1"));
        }
    }
}

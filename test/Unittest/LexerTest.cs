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
            Assert.That("10", IsTokenized.Under(DECIMAL_INTEGER).And.Append(IsSeparated.Into("10")));

            Assert.That("0b0", IsTokenized.Under(BIN_INTEGER).And.Append(IsSeparated.Into("0b0")));
            Assert.That("0b1", IsTokenized.Under(BIN_INTEGER).And.Append(IsSeparated.Into("0b1")));
            Assert.That("0B0", IsTokenized.Under(BIN_INTEGER).And.Append(IsSeparated.Into("0B0")));
            Assert.That("0B1", IsTokenized.Under(BIN_INTEGER).And.Append(IsSeparated.Into("0B1")));

            Assert.That("0o0", IsTokenized.Under(OCT_INTEGER).And.Append(IsSeparated.Into("0o0")));
            Assert.That("0o7", IsTokenized.Under(OCT_INTEGER).And.Append(IsSeparated.Into("0o7")));
            Assert.That("0O0", IsTokenized.Under(OCT_INTEGER).And.Append(IsSeparated.Into("0O0")));
            Assert.That("0O7", IsTokenized.Under(OCT_INTEGER).And.Append(IsSeparated.Into("0O7")));

            Assert.That("0x0", IsTokenized.Under(HEX_INTEGER).And.Append(IsSeparated.Into("0x0")));
            Assert.That("0xf", IsTokenized.Under(HEX_INTEGER).And.Append(IsSeparated.Into("0xf")));
            Assert.That("0X0", IsTokenized.Under(HEX_INTEGER).And.Append(IsSeparated.Into("0X0")));
            Assert.That("0Xf", IsTokenized.Under(HEX_INTEGER).And.Append(IsSeparated.Into("0Xf")));
            Assert.That("0XF", IsTokenized.Under(HEX_INTEGER).And.Append(IsSeparated.Into("0XF")));
        }

        [Test]
        public void FloatNumberTest()
        {
            Assert.That("0.0", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.0")));
            Assert.That("1.0", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("1.0")));
            Assert.That("10.0", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("10.0")));

            Assert.That("0.1", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1")));
            Assert.That("0.01", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.01")));
            Assert.That(".1", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into(".1")));

            Assert.That("0.1e10", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1e10")));
            Assert.That("0.1E10", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1E10")));
            Assert.That("0.1e+10", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1e+10")));
            Assert.That("0.1E+10", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1E+10")));
            Assert.That("0.1e-10", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1e-10")));
            Assert.That("0.1E-10", IsTokenized.Under(FLOAT_NUMBER).And.Append(IsSeparated.Into("0.1E-10")));
        }

        [Test]
        public void StringEmptyTest()
        {
            Assert.That("''", IsTokenized.Under(STRING_LITERAL).And.Append(IsSeparated.Into("''")));
            Assert.That("\"\"", IsTokenized.Under(STRING_LITERAL).And.Append(IsSeparated.Into("\"\"")));
            Assert.That("``", IsTokenized.Under(STRING_LITERAL).And.Append(IsSeparated.Into("``")));
        }

        [Test]
        public void EmptyContainerTest()
        {
            Assert.That("[]", IsTokenized.Under(OPEN_BRACK, CLOSE_BRACK).And.Append(IsSeparated.Into("[", "]")));
            Assert.That("()", IsTokenized.Under(OPEN_PAREN, CLOSE_PAREN).And.Append(IsSeparated.Into("(", ")")));
            Assert.That("{}", IsTokenized.Under(OPEN_BRACE, CLOSE_BRACE).And.Append(IsSeparated.Into("{", "}")));
        }
    }
}

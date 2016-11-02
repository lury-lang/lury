using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace Unittest
{
    [Parallelizable]
    [TestFixture]
    public class LexerTest
    {
        private static readonly LexerTestCase TestCase;

        static LexerTest()
        {
            var appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "");
            using (var input = File.OpenText(Path.Combine(appDirectory, @"TestCase/Lexer.yml")))
            {
                var db = new DeserializerBuilder();
                db.WithNodeDeserializer(new TokenValueDeserializer());
                TestCase = db.Build().Deserialize<LexerTestCase>(input);
            }
        }

        private static IEnumerable<TokenItem> SingleTokenItems => TestCase.Single.SelectMany(s => s.Expand());

        private static IEnumerable<TokenItem> CompoundTokenItems => TestCase.Compound.SelectMany(s => s.Expand());

        [Test]
        [TestCaseSource(typeof(LexerTest), nameof(SingleTokenItems))]
        public void SingleTokenTest(TokenItem testCase)
        {
            Assert.That(testCase.Source, IsTokenized.Under(testCase.Tokens[0].Value).And.Append(IsSeparated.Into(testCase.SeparatedTexts[0])));
        }

        [Test]
        [TestCaseSource(typeof(LexerTest), nameof(CompoundTokenItems))]
        public void CompoundTokenTest(TokenItem testCase)
        {
            Assert.That(testCase.Source, IsTokenized.Under(testCase.Tokens.Select(t => t.Value).ToArray()).And.Append(IsSeparated.Into(testCase.SeparatedTexts)));
        }
    }
}

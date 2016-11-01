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

        private static IEnumerable<SingleTokenItem> SingleTokenItems => TestCase.Single;

        private static IEnumerable<CompoundTokenItem> CompoundTokenItems => TestCase.Compound;

        [Test]
        [TestCaseSource(typeof(LexerTest), nameof(SingleTokenItems))]
        public void SingleTokenTest(SingleTokenItem testCase)
        {
            foreach (var source in testCase.Sources)
            {
                Assert.That(source, IsTokenized.Under(testCase.Token.Value).And.Append(IsSeparated.Into(source)));
            }
        }

        [Test]
        [TestCaseSource(typeof(LexerTest), nameof(CompoundTokenItems))]
        public void CompoundTokenTest(CompoundTokenItem testCase)
        {
            foreach (var source in testCase.Sources)
            {
                Assert.That(source, IsTokenized.Under(testCase.Tokens.Select(t => t.Value).ToArray()).And.Append(IsSeparated.Into(testCase.SeparatedTexts)));
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Lury.Core.Compiler;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Unittest
{
    internal static class Tokens
    {
        public static IEnumerable<IToken> From(string source)
        {
            var inputStream = new AntlrInputStream(source);
            var luryLexer = new LuryLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(luryLexer);
            commonTokenStream.Fill();
            return commonTokenStream.GetTokens();
        }
    }

    internal static class TokensEx
    {
        public static IEnumerable<int> Type(this IEnumerable<IToken> tokens) => tokens.Select(t => t.Type);

        public static IEnumerable<string> Text(this IEnumerable<IToken> tokens) => tokens.Select(t => t.Text);
    }

    internal static class Are
    {
        public static EqualConstraint EqualTo(params int[] types) => Is.EqualTo(types.Concat(new[] { LuryLexer.Eof }));

        public static EqualConstraint EqualTo(params string[] texts) => Is.EqualTo(texts.Concat(new[] { "<EOF>" }));
    }
}

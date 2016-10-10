using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

    internal static class IsTokenized
    {
        public static LexerTypeConstraint Under(params int[] types) => new LexerTypeConstraint(types);
    }

    internal static class IsSeparated
    {
        public static LexerTextConstraint Into(params string[] texts) => new LexerTextConstraint(texts);
    }

    internal class LexerTypeConstraint : Constraint
    {
        private readonly IEnumerable<int> types;

        public LexerTypeConstraint(IEnumerable<int> types)
        {
            this.types = types;
        }

        public override string Description => $"Types: <{string.Join(", ", types)}>";

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            var actualString = actual as string;

            if (actualString == null)
                throw new ArgumentException(nameof(actual));

            var actualTokens = Tokens.From(actualString);

            return new ConstraintResult(this, actual, actualTokens.Type().SequenceEqual(types.Concat(new[] { LuryLexer.Eof })));
        }
    }

    internal class LexerTextConstraint : Constraint
    {
        private readonly IEnumerable<string> texts;

        public LexerTextConstraint(IEnumerable<string> texts)
        {
            this.texts = texts;
        }

        public override string Description => $"Texts: <{string.Join(", ", texts)}>";

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            var actualString = actual as string;
            
            if (actualString == null)
                throw new ArgumentException(nameof(actual));

            var actualTokens = Tokens.From(actualString);

            return new ConstraintResult(this, actual, actualTokens.Text().SequenceEqual(texts.Concat(new[] { "<EOF>" })));
        }
    }
}

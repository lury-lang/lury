using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lury.Core.Compiler;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Unittest
{
    internal class TokenValueDeserializer : INodeDeserializer
    {
        public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            value = null;

            if (expectedType != typeof(TokenValue))
                return false;

            var name = (string)nestedObjectDeserializer(reader, typeof(string));

            TokenValue tokenValue;

            if (!TokenValue.TryCreate(name, out tokenValue))
                return false;

            value = tokenValue;
            return true;
        }
    }

    public struct TokenValue
    {
        public int Value { get; private set; }

        public string Name { get; private set; }

        private TokenValue(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public static bool TryCreate(string name, out TokenValue tokenValue)
        {
            tokenValue = default(TokenValue);
            int value;

            if (!TokenMap.TryGetValue(name, out value))
                return false;

            tokenValue = new TokenValue(value, name);
            return true;
        }

        public static string GetTokenName(int value) => TokenMap.FirstOrDefault(p => p.Value == value).Key;

        private static readonly IDictionary<string, int> TokenMap;

        static TokenValue()
        {
            TokenMap = typeof(LuryLexer)
                  .GetRuntimeFields()
                  .Where(f => f.FieldType == typeof(int) && f.IsStatic && f.IsLiteral)
                  .ToDictionary(f => f.Name, f => (int)f.GetRawConstantValue());
        }
    }

    internal class LexerTestCase
    {
        [YamlMember(Alias = "single")]
        public SingleTokenItem[] Single { get; set; }

        [YamlMember(Alias = "compound")]
        public CompoundTokenItem[] Compound { get; set; }
    }

    public class SingleTokenItem
    {
        [YamlMember(Alias = "source")]
        public string[] Sources { get; set; }

        [YamlMember(Alias = "token")]
        public TokenValue Token { get; set; }

        public IEnumerable<TokenItem> Expand() => Sources.Select(s => new TokenItem(s, new[] { Token }, new[] { s }));
    }

    public class CompoundTokenItem
    {
        [YamlMember(Alias = "source")]
        public string[] Sources { get; set; }

        [YamlMember(Alias = "token")]
        public TokenValue[] Tokens { get; set; }

        [YamlMember(Alias = "separate")]
        public string[] SeparatedTexts { get; set; }

        public IEnumerable<TokenItem> Expand() => Sources.Select(s => new TokenItem(s, Tokens, SeparatedTexts));
    }

    public class TokenItem
    {
        public string Source { get; set; }

        public TokenValue[] Tokens { get; set; }

        public string[] SeparatedTexts { get; set; }

        public TokenItem(string source, TokenValue[] tokens, string[] separatedTexts)
        {
            Source = source;
            Tokens = tokens;
            SeparatedTexts = separatedTexts;
        }
    }
}

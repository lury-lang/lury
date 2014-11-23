//
// TokenPairs.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Tomona Nanase
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lury.Compiling
{
    class TokenPairs
    {
        #region -- Public Static Fields --

        public static readonly IEnumerable<TokenPair> Operands = new []
        {
            new TokenPair(@"[\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Pc}\p{Lm}$]\w*", Token.Identifier),
            new TokenPair(@"\d+", Token.Number),
            new TokenPair(@"'(?:\'|.)*?'", Token.SingleString),
            new TokenPair(@"""(?:\""|.)*?""", Token.DoubleString),
        };

        public static readonly IEnumerable<TokenPair> Keywords = new []
        {
            new TokenPair("def", Token.Def),
            new TokenPair("var", Token.Var),

            new TokenPair("class", Token.Class),
            new TokenPair("abstract", Token.Abstract),
            new TokenPair("extends", Token.Extends),

            new TokenPair("prop", Token.Property),
            new TokenPair("get", Token.Getter),
            new TokenPair("set", Token.Setter),

            new TokenPair("public", Token.Public),
            new TokenPair("protected", Token.Protected),
            new TokenPair("private", Token.Private),

            new TokenPair("if", Token.yyErrorCode),
            new TokenPair("but", Token.yyErrorCode),
            new TokenPair("then", Token.yyErrorCode),

            new TokenPair("new", Token.yyErrorCode),
            new TokenPair("me", Token.Me),

            new TokenPair("println", Token.PrintLine),
            new TokenPair("print", Token.Print),
        };

        public static readonly IEnumerable<TokenPair> Operators = new []
        {
            new TokenPair(@"=>", Token.Lambda),

            new TokenPair(@"\+", '+'),
            new TokenPair(@"\-", '-'),
            new TokenPair(@"\*", '*'),
            new TokenPair(@"\/", '/'),
            new TokenPair(@"%", '%'),
            new TokenPair(@"\(", '('),
            new TokenPair(@"\)", ')'),
            new TokenPair(@"\[", '['),
            new TokenPair(@"\]", ']'),
            new TokenPair(@"~", '~'),
            new TokenPair(@",", ','),
            new TokenPair(@"\.", '.'),
            new TokenPair(@"=", '='),
            new TokenPair(@">", '>'),
            new TokenPair(@"<", '<'),
        };

        public static readonly IEnumerable<TokenPair> Delimiters = new []
        {
            new TokenPair(@":", ':'),
            new TokenPair(@"#", '#'),
            new TokenPair(RegexPattern.NewLine + "+", Token.NewLine) { IgnoreAfterSpaces = true },
        };

        public static readonly IEnumerable<TokenPair> Comments = new []
        {
            new TokenPair(@"\/\/[^\r\n]*"),
            new TokenPair(@"\/\*.*?\*\/", RegexOptions.Singleline),
        };

        #endregion
    }

    class TokenPair
    {
        #region -- Public Properties --

        public string Pattern { get; private set; }

        public int TokenNumber { get; private set; }

        public RegexOptions Options { get; private set; }

        public bool IgnoreAfterSpaces { get; set; }

        #endregion

        #region -- Constructor --

        public TokenPair(string pattern)
            : this(pattern, Token.yyErrorCode)
        {
        }

        public TokenPair(string pattern, RegexOptions options)
            : this(pattern, Token.yyErrorCode)
        {
            this.Options = options;
        }

        public TokenPair(string pattern, int tokenNumber)
        {
            this.Pattern = pattern;
            this.TokenNumber = tokenNumber;
        }

        #endregion

        #region -- Public Methods --

        public Match MatchBeforeSpace(string input, int startIndex)
        {
            var pattern = "(" + this.Pattern + ")";

            if (!this.IgnoreAfterSpaces)
                pattern += RegexPattern.Space;

            return new Regex(pattern, this.Options).Match(input, startIndex);
        }

        #endregion
    }
}


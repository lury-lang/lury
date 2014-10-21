//
// StringUtils.cs
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
using System.Text.RegularExpressions;

namespace Lury
{
    static class StringUtils
    {
        private static readonly Regex NewLine = new Regex(@"(\n|(:?\r\n)|\r)", RegexOptions.Compiled | RegexOptions.Singleline);

        public static int GetNumberOfLine(this string text)
        {
            return (text == null) ? 0 : NewLine.Matches(text).Count + 1;
        }

        public static void GetIndexPosition(this string text, int index, out int line, out int column)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (index < 0 || index >= text.Length)
                throw new ArgumentOutOfRangeException("index");

            line = 1;
            column = 1;
            Match prevMatch = null;

            foreach (Match match in NewLine.Matches(text))
            {
                if (match.Index + match.Length - 1 >= index)
                    break;

                prevMatch = match; 
                line++;
            }

            column = (prevMatch == null) ? index + 1 :
                     index - prevMatch.Index - prevMatch.Length + 1;
        }

        public static string[] GeneratePointingStrings(this string text, int index, out int line, out int column)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (index < 0 || index >= text.Length)
                throw new ArgumentOutOfRangeException("index");

            text.GetIndexPosition(index, out line, out column);

            Match nextNewLine = NewLine.Match(text, index);
            int cursorLineIndex = index - (column - 1);
            int cursorLineLength = (nextNewLine.Success ? nextNewLine.Index : text.Length) - cursorLineIndex;

            string cursorLine = text.Substring(cursorLineIndex, cursorLineLength)
                .Replace('\t', ' ')
                .Replace('\r', ' ')
                .Replace('\n', ' ');

            return new string[] {
                cursorLine,
                new string(' ', column - 1) + "^"
            };
        }

        public static string GetLine(this string text, int line)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var matches = NewLine.Matches(text);
            line--;

            if (line < 0 || line >= matches.Count)
                throw new ArgumentOutOfRangeException("line");

            int lineIndex = (line == 0) ? 0 : matches[line - 1].Index + matches[line - 1].Length;
            int lineLength = ((line == matches.Count) ? text.Length : matches[line].Index) - lineIndex;

            return text.Substring(lineIndex, lineLength);
        }
    }
}


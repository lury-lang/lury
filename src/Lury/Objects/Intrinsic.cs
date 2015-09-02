//
// Intrinsic.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2015 Tomona Nanase
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
using System.Linq;
using Lury.Objects;

namespace Lury
{
    static class Intrinsic
    {
        public static void SetBuiltInFunctions(LuryObject obj)
        {
            obj["println"] = new LuryFunction(PrintLine);
            obj["print"] = new LuryFunction(Print);
        }

        private static LuryObject PrintLine(params LuryObject[] param)
        {
            string str = string.Join(" ", param.Select(p => p == null ? "(nil)" : p.ToString()));
            Console.WriteLine(str);
            return new LuryString(str + "\n");
        }

        private static LuryObject Print(params LuryObject[] param)
        {
            string str = string.Join(" ", param.Select(p => p == null ? "(nil)" : p.ToString()));
            Console.Write(str);
            return new LuryString(str);
        }
    }
}


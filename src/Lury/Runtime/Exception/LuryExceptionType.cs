//
// LuryExceptionType.cs
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

namespace Lury.Runtime
{
    public enum LuryExceptionType
    {
        Unknown = -1,

        NilReference,
        DivideByZero,
        NotSupportedOperationUnary,
        NotSupportedOperationBinary,
        UnableToCall,
        NotEnoughFunctionArgumentNumber,
        WrongBreak,
        ConditionValueIsNotBoolean,
        AttributeIsNotFound,
        NameIsNotFound,
        WrongLValue,
        WrongRefReference,
    }

    public static class LuryExceptionStrings
    {
        public static string GetMessage(this LuryExceptionType type)
        {
            return GetRawMessage(type);
        }

        private static string GetRawMessage(LuryExceptionType type)
        {
            switch (type)
            {
                case LuryExceptionType.NilReference:
                    return "nil オブジェクトにはアクセスできません.";

                case LuryExceptionType.DivideByZero:
                    return "ゼロで除算できません.";

                case LuryExceptionType.NotSupportedOperationUnary:
                    return "定義されていない演算が試行されました.";

                case LuryExceptionType.NotSupportedOperationBinary:
                    return "定義されていない演算が試行されました.";

                case LuryExceptionType.UnableToCall:
                    return "Function 型でないオブジェクトを関数として呼び出すことはできません.";

                case LuryExceptionType.NotEnoughFunctionArgumentNumber:
                    return "関数の引数の数が一致しません.";

                case LuryExceptionType.WrongBreak:
                    return "不正な break が存在します.";

                case LuryExceptionType.ConditionValueIsNotBoolean:
                    return "条件の式は Boolean 型である必要があります.";

                case LuryExceptionType.AttributeIsNotFound:
                    return "存在しないオブジェクトを参照しました.";

                case LuryExceptionType.NameIsNotFound:
                    return "存在しないオブジェクトを参照しました.";

                case LuryExceptionType.WrongLValue:
                    return "非左辺値に代入できません.";

                case LuryExceptionType.WrongRefReference:
                    return "非左辺値は ref による参照はできません.";

                default:
                    return "不明なエラーです.";
            }
        }
    }
}


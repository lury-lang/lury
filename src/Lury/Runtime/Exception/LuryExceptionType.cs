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
        public static string GetMessage(this LuryExceptionType type, params object[] messageParams)
        {
            return String.Format(GetRawMessage(type), messageParams);
        }

        private static string GetRawMessage(LuryExceptionType type)
        {
            switch (type)
            {
                case LuryExceptionType.NilReference:
                    return "nil オブジェクトにはアクセスできません.";

                case LuryExceptionType.DivideByZero:
                    return "整数値はゼロで除算できません.";

                case LuryExceptionType.NotSupportedOperation:
                    return "'{0}' 操作を '{1}' と '{2}' 型の値に適用できません.";

                case LuryExceptionType.NotEnoughFunctionArgumentNumber:
                    return "関数の引数の数が一致しません. '{0}' 関数は {1} 個の引数を受けますが、{2} 個の引数で呼び出されています.";

                case LuryExceptionType.WrongBreak:
                    return "不正な break が存在します.";

                case LuryExceptionType.ConditionValueIsNotBoolean:
                    return "条件の式は Boolean 型である必要があります. {0} 型が指定されています.";

                case LuryExceptionType.AttributeIsNotFound:
                    return "存在しないオブジェクトを参照しました. '{0}' に属性 '{1}' は存在しません.";

                case LuryExceptionType.WrongLValue:
                    return "'{0}' に代入できません.";

                default:
                    return "不明なエラーです.";
            }
        }
    }
}


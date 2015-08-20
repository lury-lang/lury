//
// ParserInterpreter.cs
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
using System.Collections.Generic;
using Lury.Resources;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using System.Linq;
using Lury.Objects;

namespace Lury.Compiling
{
    partial　class Parser
    {
        private LuryObject globalObject;

        public Parser(LuryObject globalObject)
        {
            this.globalObject = globalObject;
        }

        private LuryObject CreateBoolean(bool value)
        {
            return value ? LuryBoolean.True : LuryBoolean.False;
        }

        private LuryObject CreateInteger(object token)
        {
            var value = ((Lexer.Token)token).Text;
            value = value.Replace("_", "");

            if (value.StartsWith("0x", true, null))
                return new LuryInteger(Convert.ToInt64(value.Substring(2), 16));

            if (value.StartsWith("0o", true, null))
                return new LuryInteger(Convert.ToInt64(value.Substring(2), 8));

            if (value.StartsWith("0b", true, null))
                return new LuryInteger(Convert.ToInt64(value.Substring(2), 2));

            return new LuryInteger(long.Parse(value));
        }

        private LuryObject CreateReal(object token)
        {
            throw new NotImplementedException();
        }

        private LuryObject CreateComplex(object token)
        {
            throw new NotImplementedException();
        }

        private LuryObject CreateString(object token, char marker)
        {
            return new LuryString(StringHelper.ConvertToInternalString(((Lexer.Token)token).Text, marker));
        }
    
        private LuryObject BuildUnary(object token, UnaryOperator operate)
        {
            var value = this.Dereference(token);

            switch (operate)
            {
                case UnaryOperator.SignNegative:
                    return value.Neg();

                case UnaryOperator.SignPositive:
                    return value.Pos();

                case UnaryOperator.BitwiseNot:
                    return value.BNot();

                case UnaryOperator.LogicalNot:
                    return value.LNot();

                default:
                    throw new InvalidOperationException();
            }
        }

        private LuryObject BuildUnaryAssign(object token, UnaryAssignOperator operate)
        {
            if (!(token is LValue))
                throw new InvalidOperationException();

            var lvalue = (LValue)token;
            var dr_value = lvalue.Dereference(this.globalObject);

            switch (operate)
            {
                case UnaryAssignOperator.IncrementPostfix:
                    lvalue.Assign(dr_value.Inc(), this.globalObject);
                    return dr_value;

                case UnaryAssignOperator.DecrementPostfix:
                    lvalue.Assign(dr_value.Dec(), this.globalObject);
                    return dr_value;

                case UnaryAssignOperator.IncrementPrefix:
                    dr_value = dr_value.Inc();
                    lvalue.Assign(dr_value, this.globalObject);
                    return dr_value;

                case UnaryAssignOperator.DecrementPrefix:
                    dr_value = dr_value.Dec();
                    lvalue.Assign(dr_value, this.globalObject);
                    return dr_value;

                default:
                    throw new InvalidOperationException();
            }
        }

        private LuryObject BuildBinary(object tokenX, object tokenY, BinaryOperator operate)
        {
            var x = this.Dereference(tokenX);
            var y = this.Dereference(tokenY);

            switch (operate)
            {
                case BinaryOperator.Power:
                    return x.Pow(y);

                case BinaryOperator.Multiplication:
                    return x.Mul(y);

                case BinaryOperator.Division:
                    return x.Div(y);

                case BinaryOperator.Modulo:
                    return x.Mod(y);

                case BinaryOperator.Addition:
                    return x.Add(y);

                case BinaryOperator.Subtraction:
                    return x.Sub(y);

                case BinaryOperator.Concatenation:
                    return x.Con(y);

                case BinaryOperator.LeftShift:
                    return x.Shl(y);

                case BinaryOperator.RightShift:
                    return x.Shl(y);

                case BinaryOperator.ArithmeticAnd:
                    return x.BAnd(y);

                case BinaryOperator.ArithmeticXor:
                    return x.BXor(y);

                case BinaryOperator.ArithmeticOr:
                    return x.BOr(y);

                case BinaryOperator.LogicalAnd:
                    return x.LAnd(y);

                case BinaryOperator.LogicalOr:
                    return x.LOr(y);

                default:
                    throw new InvalidOperationException();
            }
        }

        private LuryObject BuildAssign(object lvalue, object rvalue, AssignOperator operate)
        {
            if (!(lvalue is LValue))
                throw new InvalidOperationException();
            
            var dst = (LValue)lvalue;
            var value = this.Dereference(rvalue);

            switch (operate)
            {
                case AssignOperator.Assign:
                    dst.Assign(value, this.globalObject);
                    return value;
                    
                default:
                    throw new InvalidOperationException();
            }
        }

        private LuryObject Call(object token)
        {
            return this.Dereference(token).Call();
        }

        private LuryObject Call(object token, object paramList)
        {
            return this.Dereference(token).Call(((IEnumerable<LuryObject>)paramList).ToArray());
        }

        private LuryObject Dereference(object token)
        {
            return (token is LValue) ? ((LValue)token).Dereference(this.globalObject) : (LuryObject)token;
        }
    }

    enum UnaryOperator 
    {
        Unknown,
        SignNegative,
        SignPositive,
        BitwiseNot,
        LogicalNot,
    }

    enum UnaryAssignOperator
    {
        Unknown,
        IncrementPostfix,
        DecrementPostfix,
        IncrementPrefix,
        DecrementPrefix,
    }

    enum BinaryOperator 
    {
        Unknown,
        Power,
        Multiplication,
        Division,
        Modulo,
        Addition,
        Subtraction,
        Concatenation,
        LeftShift,
        RightShift,
        ArithmeticAnd,
        ArithmeticXor,
        ArithmeticOr,
        LogicalAnd,
        LogicalOr,
    }

    enum AssignOperator 
    {
        Unknown,
        Assign,
    }

    class LValue
    {
        private string reference;

        public LValue(object reference)
        {
            this.reference = ((Lexer.Token)reference).Text;
        }

        public LValue(string reference)
        {
            this.reference = reference;
        }

        public LuryObject Dereference(LuryObject context)
        {
            return context[this.reference];
        }

        public void Assign(LuryObject value, LuryObject context)
        {
            context[this.reference] = value;
        }
    }

    static class StringHelper
    {
        #region -- Private Static Fields --

        private static readonly Regex unicode_hex = new Regex(@"\\x[0-9A-Fa-f]{1,4}", RegexOptions.Compiled);
        private static readonly Regex unicode_hex4 = new Regex(@"\\u[0-9A-Fa-f]{4}", RegexOptions.Compiled);
        private static readonly Regex unicode_hex8 = new Regex(@"\\U[0-9A-Fa-f]{8}", RegexOptions.Compiled);

        #endregion

        public static string ConvertToInternalString(string value, char marker)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            if (value.Length < 2)
                throw new ArgumentException("value");

            ReplaceUnicodeChar(ref value);

            var sb = new StringBuilder(value);
            TrimMarker(sb, marker);
            ReplaceEscapeChar(sb);

            return sb.ToString();
        }

        private static void TrimMarker(StringBuilder value, char marker)
        {
            if (value[0] != marker || value[value.Length - 1] != marker)
                throw new ArgumentException("value");

            value.Remove(0, 1);
            value.Remove(value.Length - 1, 1);
        }

        private static void ReplaceEscapeChar(StringBuilder value)
        {
            value.Replace(@"\\", "\\");
            value.Replace(@"\'", "'");
            value.Replace(@"\""", "\"");
            value.Replace(@"\a", "\a");
            value.Replace(@"\b", "\b");
            value.Replace(@"\f", "\f");
            value.Replace(@"\n", "\n");
            value.Replace(@"\r", "\r");
            value.Replace(@"\t", "\t");
            value.Replace(@"\v", "\v");
        }

        private static void ReplaceUnicodeChar(ref string value)
        {
            // Refer to:
            // http://stackoverflow.com/questions/183907

            // type \xX - \xXXXX
            value = unicode_hex.Replace(value, m => ((char)Int16.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // type: \uXXXX
            value = unicode_hex4.Replace(value, m => ((char)Int32.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // type: \UXXXXXXXX
            value = unicode_hex8.Replace(value, m => ToUTF16(m.Value.Substring(2)));
        }

        private static string ToUTF16(string hex)
        {
            int value = int.Parse(hex, NumberStyles.HexNumber);

            if (value < 0 || value > 0x10ffff)
                throw new ArgumentException("hex");

            if (value <= 0x00ff)
                return ((char)value).ToString();
            else
            {
                int w = value - 0x10000;
                char high = (char)(0xd800 | (w >> 10) & 0x03ff);
                char low = (char)(0xdc00 | (w >> 0) & 0x03ff);
                return new string(new char[2] { high, low });
            }
        }
    }
}


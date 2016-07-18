/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Tomona Nanase
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Lury.Core.Runtime;
using Lury.Core.Runtime.Type;

namespace Lury.Core.Compiler
{
    internal class LuryVisitor : LuryBaseVisitor<LuryObject>
    {
        #region -- Private Fields --

        private readonly LuryContext globalContext;
        private LuryContext currentContext;

        #endregion

        #region -- Constructors --

        public LuryVisitor(LuryContext globalContext)
        {
            this.globalContext = globalContext;
            currentContext = globalContext;
        }

        #endregion

        #region -- Public Methods --

        public override LuryObject VisitProgram(LuryParser.ProgramContext context)
        {
            LuryObject result = null;

            for (var i = 0; i < context.ChildCount; i++)
            {
                var statementContext = context.children[i] as LuryParser.StatementContext;
                LuryObject returnValue = null;

                if (statementContext != null)
                    returnValue = VisitStatement(statementContext);

                if (returnValue != null)
                    result = returnValue;
            }

            return result;
        }

        public override LuryObject VisitStatement(LuryParser.StatementContext context)
        {
            if (context.Simple != null)
                return VisitSimple_statement(context.Simple);

            return VisitCompound_statement(context.Compound);
        }

        public override LuryObject VisitSimple_statement(LuryParser.Simple_statementContext context)
        {
            var result = VisitSmall_statement(context.First);

            for (var i = 1; i < context.ChildCount; i++)
            {
                var statementContext = context.children[i] as LuryParser.Small_statementContext;
                LuryObject returnValue = null;

                if (statementContext != null)
                    returnValue = VisitSmall_statement(statementContext);

                if (returnValue != null)
                    result = returnValue;
            }

            return result;
        }

        public override LuryObject VisitIf_statement(LuryParser.If_statementContext context)
        {
            // if block
            currentContext = new LuryContext(currentContext);
            try
            {
                var condition = VisitExpression(context.Condition) as LuryBoolean;

                if (condition == null)
                    throw new InvalidOperationException();

                if ((bool)condition.Value)
                    return VisitSuite(context.IfSuite);
            }
            finally { currentContext = currentContext.Parent; }

            // else block
            if (context.ElseSuite == null)
                return null;

            LuryObject elseReturn;
            currentContext = new LuryContext(currentContext);
            try
            {
                elseReturn = VisitSuite(context.ElseSuite);
            }
            finally { currentContext = currentContext.Parent; }

            return elseReturn;
        }

        public override LuryObject VisitFor_statement(LuryParser.For_statementContext context)
        {
            // for block
            var forCount = 0;
            currentContext = new LuryContext(currentContext);
            try
            {
                var @object = VisitExpression(context.Object);
                var iterator = LuryFunction.Call(@object.GetMember(IntrinsicConstants.FunctionIterate, currentContext), @object);
                var moveNext = LuryFunction.Call(iterator.GetMember(IntrinsicConstants.FunctionMoveNext, currentContext), iterator);
                var variable = context.Variable.Text;
                LuryObject forReturn = null;

                while (moveNext.Equals(LuryBoolean.True))
                {
                    forCount++;
                    var fetchedValue = LuryFunction.Call(iterator.GetMember(IntrinsicConstants.FunctionFetch, currentContext), iterator);
                    currentContext[variable] = fetchedValue;
                    forReturn = VisitSuite(context.ForSuite);
                    moveNext = LuryFunction.Call(iterator.GetMember(IntrinsicConstants.FunctionMoveNext, currentContext), iterator);
                }

                if (forCount > 0)
                    return forReturn;

            }
            finally { currentContext = currentContext.Parent; }

            // else block
            if (forCount <= 0 || context.ElseSuite == null)
                return null;

            LuryObject elseReturn;
            currentContext = new LuryContext(currentContext);
            try
            {
                elseReturn = VisitSuite(context.ElseSuite);
            }
            finally { currentContext = currentContext.Parent; }

            return elseReturn;
        }

        public override LuryObject VisitFunction_definition(LuryParser.Function_definitionContext context)
        {
            var name = context.Name.Text;
            var parameter = context.Parameter == null ?
                Enumerable.Empty<string>() :
                ((List<LuryObject>) VisitParameter(context.Parameter).Value).Select(_ => (string) _.Value);

            var functionInfo = new UserFunctionInfo(context.FunctionSuite, currentContext, parameter);
            var function = LuryFunction.GetObject(functionInfo);

            return currentContext[name] = function;
        }

        public override LuryObject VisitParameter(LuryParser.ParameterContext context)
        {
            return LuryList.GetObject(
                context.children
                .Where(_ => _.GetText() != ",")
                .Select(_ => LuryString.GetObject(_.GetText())));
        }

        public override LuryObject VisitSuite(LuryParser.SuiteContext context)
        {
            LuryObject result = null;

            if (context.Statements == null)
                result = VisitChildren(context);
            else
            {
                for (var i = 0; i < context.ChildCount; i++)
                {
                    var statementContext = context.children[i] as LuryParser.StatementContext;
                    LuryObject returnValue = null;

                    if (statementContext != null)
                        returnValue = VisitStatement(statementContext);

                    if (returnValue != null)
                        result = returnValue;
                }
            }

            return result;
        }

        public override LuryObject VisitExpression(LuryParser.ExpressionContext context)
        {
            return Dereference(VisitChildren(context));
        }

        public override LuryObject VisitAssignment_expression(LuryParser.Assignment_expressionContext context)
        {
            if (context.Left == null)
                return VisitChildren(context);

            var left = VisitPostfix_expression(context.Left) as LuryReference;

            if (left == null)
                throw new InvalidOperationException();

            var right = VisitAssignment_expression(context.Right);
            var result = Dereference(right);

            if (context.Op.Text != "=")
            {
                var leftDereferenced = left.Dereference(currentContext);

                switch (context.Op.Text)
                {
                    case "**=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorPow, leftDereferenced, right);
                        break;

                    case "+=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorAdd, leftDereferenced, right);
                        break;

                    case "-=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorSub, leftDereferenced, right);
                        break;

                    case "~=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorCon, leftDereferenced, right);
                        break;

                    case "*=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorMul, leftDereferenced, right);
                        break;

                    case "/=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorDiv, leftDereferenced, right);
                        break;

                    case "//=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorIntDiv, leftDereferenced, right);
                        break;

                    case "%=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorMod, leftDereferenced, right);
                        break;

                    case "&=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorAnd, leftDereferenced, right);
                        break;

                    case "|=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorOr, leftDereferenced, right);
                        break;

                    case "^=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorXor, leftDereferenced, right);
                        break;

                    case "<<=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorLShift, leftDereferenced, right);
                        break;

                    case ">>=":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorRShift, leftDereferenced, right);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            return left.Assign(currentContext, result);
        }

        public override LuryObject VisitComma_expression(LuryParser.Comma_expressionContext context)
        {
            var left = VisitBool_not_expression(context.Left);

            if (context.Right == null)
                return left;

            throw new NotImplementedException();
        }

        public override LuryObject VisitBool_not_expression(LuryParser.Bool_not_expressionContext context)
        {
            return context.Right == null ?
                VisitChildren(context) :
                CallUnaryOperator(IntrinsicConstants.OperatorNot, VisitBool_not_expression(context.Right));
        }

        public override LuryObject VisitComparison_expression(LuryParser.Comparison_expressionContext context)
        {
            var left = VisitRange_expression(context.Left);

            if (context.Op == null)
                return left;

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.children[i];
                var right = VisitRange_expression((LuryParser.Range_expressionContext)context.children[i + 1]);
                LuryObject judge;

                switch (op.GetText())
                {
                    case ">":
                        judge = CallBinaryOperator(IntrinsicConstants.OperatorGt, left, right);
                        break;

                    case "<":
                        judge = CallBinaryOperator(IntrinsicConstants.OperatorLt, left, right);
                        break;

                    case "==":
                        judge = CallBinaryOperator(IntrinsicConstants.OperatorEq, left, right);
                        break;

                    case "!=":
                        judge = CallBinaryOperator(IntrinsicConstants.OperatorNe, left, right);
                        break;

                    case ">=":
                        judge = CallBinaryOperator(IntrinsicConstants.OperatorGtq, left, right);
                        break;

                    case "<=":
                        judge = CallBinaryOperator(IntrinsicConstants.OperatorLtq, left, right);
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                if (!((LuryBoolean)judge).ToBoolean())
                    return LuryBoolean.False;

                left = right;
            }

            return LuryBoolean.True;
        }

        public override LuryObject VisitRange_expression(LuryParser.Range_expressionContext context)
        {
            var left = VisitIn_expression(context.Left);

            if (context.Op == null)
                return left;

            throw new NotImplementedException();
        }

        public override LuryObject VisitIn_expression(LuryParser.In_expressionContext context)
        {
            var left = VisitOr_expression(context.Left);

            if (context.Op == null)
                return left;

            var right = VisitOr_expression(context.Right);

            if (context.Op.Text != "in" && context.Op.Text != "!in")
                throw new InvalidOperationException();

            // subject for in operator is right term
            var result = CallBinaryOperator(IntrinsicConstants.OperatorIn, right, left);

            return context.Op.Text == "!in" ? ((LuryBoolean)result).Toggle() : result;
        }

        public override LuryObject VisitOr_expression(LuryParser.Or_expressionContext context)
        {
            var result = VisitXor_expression(context.Left);

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var right = VisitXor_expression((LuryParser.Xor_expressionContext)context.children[i + 1]);
                result = CallBinaryOperator(IntrinsicConstants.OperatorOr, result, right);
            }

            return result;
        }

        public override LuryObject VisitXor_expression(LuryParser.Xor_expressionContext context)
        {
            var result = VisitAnd_expression(context.Left);

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var right = VisitAnd_expression((LuryParser.And_expressionContext)context.children[i + 1]);
                result = CallBinaryOperator(IntrinsicConstants.OperatorXor, result, right);
            }

            return result;
        }

        public override LuryObject VisitAnd_expression(LuryParser.And_expressionContext context)
        {
            var result = VisitShift_expression(context.Left);

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var right = VisitShift_expression((LuryParser.Shift_expressionContext)context.children[i + 1]);
                result = CallBinaryOperator(IntrinsicConstants.OperatorAnd, result, right);
            }

            return result;
        }

        public override LuryObject VisitShift_expression(LuryParser.Shift_expressionContext context)
        {
            var result = VisitAddition_expression(context.Left);

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.children[i];
                var right = VisitAddition_expression((LuryParser.Addition_expressionContext)context.children[i + 1]);

                switch (op.GetText())
                {
                    case "<<":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorLShift, result, right);
                        break;

                    case ">>":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorRShift, result, right);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            return result;
        }

        public override LuryObject VisitAddition_expression(LuryParser.Addition_expressionContext context)
        {
            var result = VisitMultiplication_expression(context.Left);

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.children[i];
                var right = VisitMultiplication_expression((LuryParser.Multiplication_expressionContext)context.children[i + 1]);

                switch (op.GetText())
                {
                    case "+":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorAdd, result, right);
                        break;

                    case "-":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorSub, result, right);
                        break;

                    case "~":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorCon, result, right);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            return result;
        }

        public override LuryObject VisitMultiplication_expression(LuryParser.Multiplication_expressionContext context)
        {
            var result = VisitPower_expression(context.Left);

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.children[i];
                var right = VisitPower_expression((LuryParser.Power_expressionContext)context.children[i + 1]);

                switch (op.GetText())
                {
                    case "*":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorMul, result, right);
                        break;

                    case "/":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorDiv, result, right);
                        break;

                    case "//":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorIntDiv, result, right);
                        break;

                    case "%":
                        result = CallBinaryOperator(IntrinsicConstants.OperatorMod, result, right);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            return result;
        }

        public override LuryObject VisitPower_expression(LuryParser.Power_expressionContext context)
        {
            var left = VisitUnary_expression(context.Left);

            if (context.Right == null)
                return left;

            var right = VisitPower_expression(context.Right);

            return CallBinaryOperator(IntrinsicConstants.OperatorPow, left, right);
        }

        public override LuryObject VisitUnary_expression(LuryParser.Unary_expressionContext context)
        {
            if (context.Op == null)
            {
                var obj = VisitPostfix_expression(context.RefRight);

                if (context.Ref != null)
                    return obj;

                var testObj = Dereference(obj) as LuryFunction;

                return testObj != null
                    ? LuryFunction.Call(testObj)
                    : obj;
            }

            var result = VisitUnary_expression(context.Right);
            var reference = result as LuryReference;

            var op = context.Op;

            switch (op.Text)
            {
                case "++":
                    if (reference == null)
                        throw new InvalidOperationException();

                    return reference.Assign(currentContext, CallUnaryOperator(IntrinsicConstants.OperatorInc, result));

                case "--":
                    if (reference == null)
                        throw new InvalidOperationException();

                    return reference.Assign(currentContext, CallUnaryOperator(IntrinsicConstants.OperatorDec, result));

                case "+":
                    return CallUnaryOperator(IntrinsicConstants.OperatorPos, result);

                case "-":
                    return CallUnaryOperator(IntrinsicConstants.OperatorNeg, result);

                case "!":
                    return CallUnaryOperator(IntrinsicConstants.OperatorInv, result);

                default:
                    throw new InvalidOperationException();
            }
        }

        public override LuryObject VisitPostfix_expression(LuryParser.Postfix_expressionContext context)
        {
            if (context.Left == null)
                return VisitChildren(context);

            var left = VisitPostfix_expression(context.Left);

            if (context.Dot != null)
                return LuryReference.Create(Dereference(left), context.Dot.Text);

            if (context.Op != null)
            {
                var result = Dereference(left);
                var reference = left as LuryReference;

                if (reference == null)
                    throw new InvalidOperationException();

                reference.Assign(currentContext,
                    context.Op.Text == "++"
                        ? CallUnaryOperator(IntrinsicConstants.OperatorInc, result)
                        : CallUnaryOperator(IntrinsicConstants.OperatorDec, result));

                return result;
            }

            if (context.Key != null)
                return LuryReference.Create(Dereference(left), VisitKey(context.Key));

            if (context.Call != null)
            {
                if (context.Arguments == null)
                    return LuryFunction.Call(Dereference(left));

                var arguments = VisitArgument(context.Arguments);
                return LuryFunction.Call(Dereference(left), ((List<LuryObject>)arguments.Value).ToArray());
            }

            throw new InvalidOperationException();
        }

        public override LuryObject VisitArgument(LuryParser.ArgumentContext context)
        {
            return LuryList.GetObject(
                context.children
                .OfType<LuryParser.Bool_not_expressionContext>()
                .Select(_ => Dereference(VisitBool_not_expression(_))));
        }

        public override LuryObject VisitPrimary(LuryParser.PrimaryContext context)
        {
            if (context.True != null)
                return LuryBoolean.True;

            if (context.False != null)
                return LuryBoolean.False;

            if (context.Nil != null)
                return LuryNil.Nil;

            if (context.Expression != null)
                return VisitExpression(context.Expression);

            if (context.Name != null)
                return LuryReference.Create(context.Name.Text);

            return VisitLiteral(context.Literal);
        }

        public override LuryObject VisitLiteral(LuryParser.LiteralContext context)
        {
            if (context.String != null)
            {
                var str = context.children[0].GetText();
                return LuryString.GetObject(str.ConvertToInternalString(str[0]));
            }

            if (context.Real != null)
                return LuryReal.GetObject(context.children[0].GetText().ForLuryReal());

            if (context.Integer != null)
                return LuryInteger.GetObject(context.children[0].GetText().ForLuryInteger());

            if (context.List != null)
                return VisitList_literal(context.List);

            throw new InvalidOperationException();
        }

        public override LuryObject VisitList_literal(LuryParser.List_literalContext context)
        {
            if (context.First == null)
                return LuryList.GetObject(Enumerable.Empty<LuryObject>());

            return LuryList.GetObject(
                context.children
                .OfType<LuryParser.Bool_not_expressionContext>()
                .Select(VisitBool_not_expression));
        }

        #endregion

        #region -- Private Methods --

        private LuryObject Dereference(LuryObject @object)
        {
            var reference = @object as LuryReference;

            if (reference != null)
                return reference.Dereference(currentContext);

            return @object;
        }

        private LuryObject CallUnaryOperator(string op, LuryObject subject)
        {
            subject = Dereference(subject);

            var @operator = subject.GetMember(op, currentContext);
            return LuryFunction.Call(@operator, subject);
        }

        private LuryObject CallBinaryOperator(string op, LuryObject subject, LuryObject @object)
        {
            subject = Dereference(subject);
            @object = Dereference(@object);

            var @operator = subject.GetMember(op, currentContext);
            return LuryFunction.Call(@operator, subject, @object);
        }

        #endregion
    }
}

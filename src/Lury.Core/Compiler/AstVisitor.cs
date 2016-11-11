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
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Lury.Core.Error;
using static Lury.Core.Compiler.LuryParser;
using static Lury.Core.Compiler.AstNodeType;
using NodeChildren = System.Collections.Generic.Dictionary<object, Lury.Core.Compiler.AstNode>;

namespace Lury.Core.Compiler
{
    internal class AstVisitor : LuryBaseVisitor<AstNode>
    {
        #region -- Public Methods --

        public override AstNode VisitProgram(ProgramContext context)
        {
            return new AstNode(Program, null,
                context
                   .children
                   .OfType<StatementContext>()
                   .SelectMany(c => VisitStatement(c).Children.Values)
                   .Select((c, i) => new { c, i = (object)i })
                   .ToDictionary(t => t.i, t => t.c));
        }

        public override AstNode VisitStatement(StatementContext context)
        {
            if (context.Simple != null)
                return VisitSimple_statement(context.Simple);

            return new AstNode(Statement, null, new NodeChildren { { 0, VisitCompound_statement(context.Compound) } });
        }

        public override AstNode VisitSimple_statement(Simple_statementContext context)
        {
            return new AstNode(SimpleStatement, null,
                context
                   .children
                   .OfType<Small_statementContext>()
                   .Select((c, i) => new { c = VisitSmall_statement(c), i = (object)i })
                   .ToDictionary(t => t.i, t => t.c));
        }

        public override AstNode VisitPass_statement(Pass_statementContext context)
        {
            return new AstNode(Pass, context.Mark);
        }

        public override AstNode VisitBreak_statement(Break_statementContext context)
        {
            return new AstNode(Break, context.Mark);
        }

        public override AstNode VisitReturn_statement(Return_statementContext context)
        {
            if (context.Value == null)
                return new AstNode(Return, context.Mark);

            return new AstNode(Return, context.Mark, new NodeChildren
            {
                { "Value", VisitExpression(context.Value) }
            });
        }

        public override AstNode VisitIf_statement(If_statementContext context)
        {
            return new AstNode(IfStatement, context.Mark, new NodeChildren
            {
                { "Condition0", VisitExpression(context.Condition) },
                { "Suite0", VisitSuite(context.IfSuite) },
                { "Else", context.ElseSuite == null ? null : VisitSuite(context.ElseSuite) },
            });
        }

        public override AstNode VisitFor_statement(For_statementContext context)
        {
            return new AstNode(ForStatement, context.Mark, new NodeChildren
            {
                { "Variable", new AstNode(Identifier, context.Variable) },
                { "Object", VisitExpression(context.Object) },
                { "Suite", VisitSuite(context.ForSuite) },
                { "Else", context.ElseSuite == null ? null : VisitSuite(context.ElseSuite) },
            });
        }

        public override AstNode VisitFunction_definition(Function_definitionContext context)
        {
            return new AstNode(FunctionDefinition, context.Mark, new NodeChildren
            {
                { "Name", new AstNode(Identifier, context.Name) },
                { "Parameter", context.Parameter == null ? null : VisitParameter(context.Parameter) },
                { "Suite", VisitSuite(context.FunctionSuite) }
            });
        }

        public override AstNode VisitParameter(ParameterContext context)
        {
            return new AstNode(Parameter, null, new NodeChildren(
                context
                   .children
                   .OfType<ITerminalNode>()
                   .Select((c, i) => new { c = new AstNode(Identifier, c.Symbol), i = (object)i })
                   .ToDictionary(t => t.i, t => t.c)));
        }

        public override AstNode VisitSuite(SuiteContext context)
        {
            if (context.Statements == null)
                return VisitChildren(context);

            return new AstNode(Suite, null,
                context
                   .children
                   .OfType<StatementContext>()
                   .Select((c, i) => new { c = VisitStatement(c), i = (object)i })
                   .ToDictionary(t => t.i, t => t.c));
        }

        public override AstNode VisitAssignment_expression(Assignment_expressionContext context)
        {
            if (context.Right == null)
                return VisitChildren(context);

            return new AstNode(GetOperatorType(context.Op), context.Op, new NodeChildren
            {
                { "Left", VisitPostfix_expression(context.Left) },
                { "Right", VisitAssignment_expression(context.Right) }
            });
        }

        public override AstNode VisitComma_expression(Comma_expressionContext context)
        {
            if (context.Right == null)
                return VisitBool_not_expression(context.Left);

            return new AstNode(Comma, null,
                context
                   .children
                   .OfType<Bool_not_expressionContext>()
                   .Select((c, i) => new { c = VisitBool_not_expression(c), i = (object)i })
                   .ToDictionary(t => t.i, t => t.c));
        }

        public override AstNode VisitBool_not_expression(Bool_not_expressionContext context)
        {
            if (context.Op != null)
                return new AstNode(Not, context.Op, new NodeChildren
                {
                    { "Right", VisitBool_not_expression(context.Right) }
                });

            return VisitChildren(context);
        }

        public override AstNode VisitComparison_expression(Comparison_expressionContext context)
        {
            if (context.Right == null)
                return VisitRange_expression(context.Left);

            return CreateLeftAssociatedNode<Range_expressionContext, Comparison_expressionContext>(VisitRange_expression, context);
        }

        public override AstNode VisitRange_expression(Range_expressionContext context)
        {
            if (context.Right == null)
                return VisitIn_expression(context.Left);

            var children = new NodeChildren
            {
                { "Left", VisitIn_expression(context.Left) },
                { "Right", VisitIn_expression(context.Right) }
            };

            switch (context.Op?.Text)
            {
                case "..":
                    return new AstNode(RangeOpen, context.Op, children);
                case "...":
                    return new AstNode(RangeClose, context.Op, children);

                default:
                    throw new CodeBugException(context.Start);
            }
        }

        public override AstNode VisitIn_expression(In_expressionContext context)
        {
            if (context.Right == null)
                return VisitOr_expression(context.Left);

            var rightNode = new NodeChildren { { "Right", VisitOr_expression(context.Right) } };

            switch (context.Op?.Text)
            {
                case "in":
                    return new AstNode(In, context.Op, rightNode);
                case "!in":
                    return new AstNode(NotIn, context.Op, rightNode);

                default:
                    throw new CodeBugException(context.Start);
            }
        }

        public override AstNode VisitOr_expression(Or_expressionContext context)
        {
            if (context.Right == null)
                return VisitXor_expression(context.Left);

            return CreateLeftAssociatedNode<Xor_expressionContext, Or_expressionContext>(VisitXor_expression, context);
        }

        public override AstNode VisitXor_expression(Xor_expressionContext context)
        {
            if (context.Right == null)
                return VisitAnd_expression(context.Left);

            return CreateLeftAssociatedNode<And_expressionContext, Xor_expressionContext>(VisitAnd_expression, context);
        }

        public override AstNode VisitAnd_expression(And_expressionContext context)
        {
            if (context.Right == null)
                return VisitShift_expression(context.Left);

            return CreateLeftAssociatedNode<Shift_expressionContext, And_expressionContext>(VisitShift_expression, context);
        }

        public override AstNode VisitShift_expression(Shift_expressionContext context)
        {
            if (context.Right == null)
                return VisitAddition_expression(context.Left);

            return CreateLeftAssociatedNode<Addition_expressionContext, Shift_expressionContext>(VisitAddition_expression, context);
        }

        public override AstNode VisitAddition_expression(Addition_expressionContext context)
        {
            if (context.Right == null)
                return VisitMultiplication_expression(context.Left);

            return CreateLeftAssociatedNode<Multiplication_expressionContext, Addition_expressionContext>(VisitMultiplication_expression, context);
        }

        public override AstNode VisitMultiplication_expression(Multiplication_expressionContext context)
        {
            if (context.Right == null)
                return VisitPower_expression(context.Left);

            return CreateLeftAssociatedNode<Power_expressionContext, Multiplication_expressionContext>(VisitPower_expression, context);
        }

        public override AstNode VisitPower_expression(Power_expressionContext context)
        {
            var left = VisitUnary_expression(context.Left);

            if (context.Right != null)
                return new AstNode(Power, context.Op, new NodeChildren
                {
                    { "Left", left },
                    { "Right", VisitPower_expression(context.Right) }
                });

            return left;
        }

        public override AstNode VisitUnary_expression(Unary_expressionContext context)
        {
            if (context.Ref != null)
                return new AstNode(UnaryRef, context.Ref, new NodeChildren
                {
                    { "Right", VisitPostfix_expression(context.RefRight) }
                });

            if (context.RefRight != null)
                return VisitPostfix_expression(context.RefRight);

            var rightNode = new NodeChildren { { "Right", VisitUnary_expression(context.Right) } };

            switch (context.Op?.Text)
            {
                case "++":
                    return new AstNode(UnaryIncrement, context.Op, rightNode);
                case "--":
                    return new AstNode(UnaryDecrement, context.Op, rightNode);
                case "+":
                    return new AstNode(UnaryPlus, context.Op, rightNode);
                case "-":
                    return new AstNode(UnaryNegate, context.Op, rightNode);
                case "~":
                    return new AstNode(UnaryInvert, context.Op, rightNode);

                default:
                    throw new CodeBugException(context.Start);
            }
        }

        public override AstNode VisitPostfix_expression(Postfix_expressionContext context)
        {
            if (context.Left == null)
                return VisitChildren(context);

            var left = VisitPostfix_expression(context.Left);

            if (context.Attribute != null)
                return new AstNode(PostfixAttribute, null, new NodeChildren
                {
                    { "Left", left },
                    { "Attribute", new AstNode(AstNodeType.Attribute, context.Attribute) }
                });

            if (context.Op?.Text == "++")
                return new AstNode(PostfixIncrement, context.Op, new NodeChildren { { "Left", left } });

            if (context.Op?.Text == "--")
                return new AstNode(PostfixDecrement, context.Op, new NodeChildren { { "Left", left } });

            if (context.Call != null)
                return new AstNode(PostfixCall, context.Call, new NodeChildren
                {
                    { "Left", left },
                    { "Argument", context.Arguments == null ? null : VisitArgument(context.Arguments) }
                });

            if (context.Index != null)
                return new AstNode(PostfixIndex, context.Index, new NodeChildren
                {
                    { "Left", left },
                    { "Key", VisitKey_index(context.Key) }
                });

            throw new CodeBugException(context.Start);
        }

        public override AstNode VisitArgument(ArgumentContext context)
        {
            return new AstNode(Argument, null,
                 context
                    .children
                    .OfType<Bool_not_expressionContext>()
                    .Select((c, i) => new { c, i })
                    .ToDictionary(t => (object)t.i, t => VisitBool_not_expression(t.c)));
        }

        public override AstNode VisitKey_index(Key_indexContext context)
        {
            return VisitChildren(context);
        }

        public override AstNode VisitPrimary(PrimaryContext context)
        {
            if (context.Name != null)
                return new AstNode(Identifier, context.Name);

            if (context.Literal != null)
                return VisitLiteral(context.Literal);

            if (context.True != null)
                return new AstNode(PrimaryTrue, context.True);

            if (context.False != null)
                return new AstNode(PrimaryFalse, context.False);

            if (context.Nil != null)
                return new AstNode(PrimaryNil, context.Nil);

            if (context.Expression != null)
                return VisitExpression(context.Expression);

            throw new CodeBugException(context.Start);
        }

        public override AstNode VisitLiteral(LiteralContext context)
        {
            if (context.String != null)
                return new AstNode(LiteralString, context.String);

            if (context.Real != null)
                return new AstNode(LiteralReal, context.Real);

            if (context.Integer != null)
                return new AstNode(LiteralInteger, context.Integer);

            if (context.List != null)
                return VisitList_literal(context.List);

            if (context.Tuple != null)
                return VisitTuple_literal(context.Tuple);

            if (context.Hash != null)
                return VisitHash_literal(context.Hash);

            throw new CodeBugException(context.Start);
        }

        public override AstNode VisitList_literal(List_literalContext context)
        {
            return new AstNode(LiteralList, null,
                 context
                    .children
                    .OfType<Bool_not_expressionContext>()
                    .Select((c, i) => new { c, i })
                    .ToDictionary(t => (object)t.i, t => VisitBool_not_expression(t.c)));
        }

        public override AstNode VisitTuple_literal(Tuple_literalContext context)
        {
            return new AstNode(LiteralTuple, null,
                 context
                    .children
                    .OfType<Bool_not_expressionContext>()
                    .Select((c, i) => new { c, i })
                    .ToDictionary(t => (object)t.i, t => VisitBool_not_expression(t.c)));
        }

        public override AstNode VisitHash_literal(Hash_literalContext context)
        {
            return new AstNode(LiteralHash, null,
                 context
                    .children
                    .OfType<Hash_elementContext>()
                    .Select((c, i) => new { c, i })
                    .ToDictionary(t => (object)t.i, t => VisitHash_element(t.c)));
        }

        public override AstNode VisitHash_element(Hash_elementContext context)
        {
            return new AstNode(HashElement, null, new NodeChildren
            {
                { "Key", VisitPrimary(context.Key) },
                { "Value", VisitBool_not_expression(context.Value) }
            });
        }

        #endregion

        #region -- Private Methods --

        private static AstNodeType GetOperatorType(IToken token)
        {
            switch (token.Text)
            {
                case ",":
                    return Comma;

                case "!":
                    return Not;

                case ">":
                    return GreaterThan;
                case "<":
                    return LessThan;
                case ">=":
                    return GreaterThanOrEqual;
                case "<=":
                    return LessThanOrEqual;
                case "==":
                    return Equal;
                case "!=":
                    return NotEqual;

                case "..":
                    return RangeOpen;
                case "...":
                    return RangeClose;

                case "in":
                    return In;
                case "!in":
                    return NotIn;

                case "|":
                    return Or;
                case "^":
                    return Xor;
                case "&":
                    return And;

                case "<<":
                    return ShiftLeft;
                case ">>":
                    return ShiftRight;

                case "+":
                    return Add;
                case "-":
                    return Subtract;
                case "~":
                    return Concat;

                case "*":
                    return Multiply;
                case "/":
                    return Devide;
                case "//":
                    return IntDevide;
                case "%":
                    return Modulo;

                default:
                    throw new CodeBugException(token);
            }
        }

        private static AstNode CreateLeftAssociatedNode<TVisitor, TContext>(Func<TVisitor, AstNode> visitor, TContext context)
            where TContext : ParserRuleContext
        {
            var nodes = context.children
                .OfType<TVisitor>()
                .Select(visitor)
                .ToArray();

            var ops = context.children
                .OfType<ITerminalNode>()
                .Select(c => c.Symbol)
                .ToArray();

            AstNode lastNode = null;

            for (var i = 0; i < ops.Length; i++)
                lastNode = lastNode == null ?
                    new AstNode(GetOperatorType(ops[i]), ops[i], new NodeChildren { { "Left", nodes[i] }, { "Right", nodes[i + 1] } }) :
                    new AstNode(GetOperatorType(ops[i]), ops[i], new NodeChildren { { "Left", lastNode }, { "Right", nodes[i + 1] } });

            return lastNode;
        }
        #endregion

    }
}

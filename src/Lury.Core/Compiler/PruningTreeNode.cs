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

using System.Collections.Generic;
using Antlr4.Runtime;

namespace Lury.Core.Compiler
{
    internal class PruningTreeNode
    {
        #region -- Public Properties --

        public bool IsTerm => TermToken != null && Children == null;

        public IToken TermToken { get; }

        public IReadOnlyDictionary<object, PruningTreeNode> Children { get; }

        public PruningTreeNodeType NodeType { get; }

        #endregion

        #region -- Constructors --

        public PruningTreeNode(PruningTreeNodeType nodeType, IToken termToken)
        {
            TermToken = termToken;
            NodeType = nodeType;
            Children = null;
        }

        public PruningTreeNode(PruningTreeNodeType nodeType, IToken termToken, IDictionary<object, PruningTreeNode> children)
        {
            TermToken = termToken;
            NodeType = nodeType;
            Children = (IReadOnlyDictionary<object, PruningTreeNode>) children;
        }

        #endregion

        #region -- Public Methods --

        public override string ToString()
        {
            var term = TermToken == null ? "" : $" {TermToken.Text}";
            return $"{NodeType}{term}";
        }

        #endregion
    }

    internal enum PruningTreeNodeType
    {
        Term = 0,
        
        Program,

        Statement,

        SimpleStatement,

        Pass,
        Break,
        Return,

        IfStatement,
        ForStatement,

        FunctionDefinition,
        Parameter,

        Suite,

        Assign,
        AssignAdd,
        AssignSubtract,
        AssignMultiply,
        AssignDivide,
        AssignIntDivide,
        AssignModulo,
        AssignAnd,
        AssignOr,
        AssignXor,
        AssignShiftLeft,
        AssignShiftRight,
        AssignPower,
        AssignConcat,

        Comma,

        Not,

        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Equal,
        NotEqual,

        RangeOpen,
        RangeClose,

        In,
        NotIn,

        Or,
        Xor,
        And,

        ShiftLeft,
        ShiftRight,

        Add,
        Subtract,
        Concat,

        Multiply,
        Devide,
        IntDevide,
        Modulo,

        Power,

        UnaryRef,
        UnaryIncrement,
        UnaryDecrement,
        UnaryPlus,
        UnaryNegate,
        UnaryInvert,

        PostfixAttribute,
        PostfixIncrement,
        PostfixDecrement,
        PostfixCall,
        PostfixIndex,

        Attribute,
        Argument,

        Identifier,
        PrimaryTrue,
        PrimaryFalse,
        PrimaryNil,

        LiteralString,
        LiteralReal,
        LiteralInteger,
        LiteralList,
        LiteralTuple,
        LiteralHash,

        HashElement
    }
}

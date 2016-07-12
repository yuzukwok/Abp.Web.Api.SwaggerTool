#region License
//The MIT License (MIT)

//Copyright (c) 2014 Doug Swisher

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using Difftaculous.ZModel;


namespace Difftaculous.Paths.Expressions
{
    internal class BooleanQueryExpression : QueryExpression
    {
        public List<PathFilter> Path { get; set; }
        public ZValue Value { get; set; }

        public override bool IsMatch(ZToken t)
        {
            IEnumerable<ZToken> pathResult = DiffPath.Evaluate(Path, t, false);
            foreach (ZToken r in pathResult)
            {
                ZValue v = r as ZValue;
                switch (Operator)
                {
                    case QueryOperator.Equals:
                        if (v != null && v.Equals(Value))
                            return true;
                        break;

                    case QueryOperator.NotEquals:
                        if (v != null && !v.Equals(Value))
                            return true;
                        break;

                    case QueryOperator.GreaterThan:
                        if (v != null && v.CompareTo(Value) > 0)
                            return true;
                        break;

                    case QueryOperator.GreaterThanOrEquals:
                        if (v != null && v.CompareTo(Value) >= 0)
                            return true;
                        break;

                    case QueryOperator.LessThan:
                        if (v != null && v.CompareTo(Value) < 0)
                            return true;
                        break;

                    case QueryOperator.LessThanOrEquals:
                        if (v != null && v.CompareTo(Value) <= 0)
                            return true;
                        break;

                    case QueryOperator.Exists:
                        return true;

                    default:
                        throw new NotImplementedException(Operator + " query operator is not implemented");
                }
            }

            return false;
        }
    }
}

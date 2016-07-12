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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Difftaculous.ZModel;

namespace Difftaculous.Paths
{
    internal class FieldFilter : PathFilter
    {
        public string Name { get; set; }


        public override IEnumerable<ZToken> ExecuteFilter(IEnumerable<ZToken> current, bool errorWhenNoMatch)
        {
            foreach (ZToken t in current)
            {
                ZObject o = t as ZObject;
                if (o != null)
                {
                    if (Name != null)
                    {
                        ZProperty p = o.Property(Name, false);
                        ZToken v = (p == null) ? null : p.Value;

                        if (v != null)
                        {
                            yield return v;
                        }
                        else if (errorWhenNoMatch)
                        {
                            throw new PathException(string.Format("Property '{0}' does not exist on ZObject.", Name));
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, ZToken> p in o)
                        {
                            yield return p.Value;
                        }
                    }
                }
                else
                {
                    if (errorWhenNoMatch)
                    {
                        throw new PathException(string.Format("Property '{0}' not valid on {1}.", Name ?? "*", t.GetType().Name));
                    }
                }
            }
        }



        public override void AddJsonPath(StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Append(".");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Name.Any(c => !char.IsLetterOrDigit(c) && (c != '$')))
                {
                    sb.AppendFormat("['{0}']", Name);
                }
                else
                {
                    sb.Append(Name);
                }
            }
            else
            {
                sb.Append("*");
            }
        }
    }
}

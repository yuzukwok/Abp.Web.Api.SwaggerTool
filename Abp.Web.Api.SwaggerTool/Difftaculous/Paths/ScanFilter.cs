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
using System.Text;
using Difftaculous.ZModel;

namespace Difftaculous.Paths
{
    internal class ScanFilter : PathFilter
    {
        public string Name { get; set; }

        public override IEnumerable<ZToken> ExecuteFilter(IEnumerable<ZToken> current, bool errorWhenNoMatch)
        {
            foreach (ZToken root in current)
            {
                if (Name == null)
                    yield return root;

                ZToken value = root;
                ZToken container = root;

                while (true)
                {
                    if (container != null && container.HasValues)
                    {
                        value = container.First;
                    }
                    else
                    {
                        while (value != null && value != root && value == value.Parent.Last)
                        {
                            value = value.Parent;
                        }

                        if (value == null || value == root)
                            break;

                        value = value.Next;
                    }

                    ZProperty e = value as ZProperty;

                    if (e != null)
                    {
                        if (e.Name == Name)
                            yield return e.Value;
                    }
                    else
                    {
                        if (Name == null)
                            yield return value;
                    }

                    container = value as ZContainer;
                }
            }
        }



        public override void AddJsonPath(StringBuilder sb)
        {
            throw new NotImplementedException();
        }
    }
}

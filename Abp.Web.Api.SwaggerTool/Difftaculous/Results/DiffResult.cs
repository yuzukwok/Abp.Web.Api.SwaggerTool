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


namespace Difftaculous.Results
{
    internal class DiffResult : IDiffResult
    {
        private readonly static DiffResult _same = new DiffResult { AreSame = true };


        public static DiffResult Same { get { return _same; } }

        private DiffResult()
        {
            Annotations = Enumerable.Empty<AbstractDiffAnnotation>();
        }



        public DiffResult(AbstractDiffAnnotation annotation)
        {
            Annotations = new[] { annotation };
            AreSame = annotation.AreSame;
        }



        public IEnumerable<AbstractDiffAnnotation> Annotations { get; private set; }
        public bool AreSame { get; private set; }


        public IDiffResult Merge(IDiffResult other)
        {
            return new DiffResult { AreSame = AreSame && other.AreSame, Annotations = Annotations.Union(other.Annotations) };
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("DiffResult, AreSame={0}", AreSame));

            foreach (var anno in Annotations)
            {
                builder.AppendLine(string.Format("Anno, path={0}, msg={1}", anno.Path.AsJsonPath, anno.Message));
            }

            return builder.ToString();
        }
    }
}

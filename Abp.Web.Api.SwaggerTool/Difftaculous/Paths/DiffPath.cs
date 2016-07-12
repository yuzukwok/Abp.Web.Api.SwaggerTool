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
using System.Text;
using Difftaculous.ZModel;


namespace Difftaculous.Paths
{
    /// <summary>
    /// A path to one or more items within the abstract difference model.
    /// </summary>
    public class DiffPath
    {

        private DiffPath()
        {
            Filters = new List<PathFilter>();
        }


        internal List<PathFilter> Filters { get; private set; }



        /// <summary>
        /// Construct a path from a JsonPath string.
        /// </summary>
        /// <param name="path">The JsonPath string.</param>
        /// <returns>A DiffPath for the string.</returns>
        public static DiffPath FromJsonPath(string path)
        {
            return new DiffPath
            {
                Filters = JsonPathParser.Parse(path)
            };
        }


        internal static DiffPath FromToken(ZToken token)
        {
            return token.Path;
        }



        /// <summary>
        /// Return a string representation of the DiffPath using JsonPath notation.
        /// </summary>
        public string AsJsonPath
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (var filter in Filters)
                {
                    filter.AddJsonPath(builder);
                }
                return builder.ToString();
            }
        }



        /// <summary>
        /// Return a string representation of this DiffPath.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return AsJsonPath;
        }



        internal IEnumerable<ZToken> Evaluate(ZToken t, bool errorWhenNoMatch)
        {
            return Evaluate(Filters, t, errorWhenNoMatch);
        }



        internal static IEnumerable<ZToken> Evaluate(List<PathFilter> filters, ZToken t, bool errorWhenNoMatch)
        {
            IEnumerable<ZToken> current = new[] { t };
            foreach (PathFilter filter in filters)
            {
                current = filter.ExecuteFilter(current, errorWhenNoMatch);
            }

            return current;
        }
    }
}

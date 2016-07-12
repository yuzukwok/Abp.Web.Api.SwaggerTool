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
using Difftaculous.Caveats;
using Difftaculous.Hints;
using Difftaculous.Paths;


namespace Difftaculous
{
    /// <summary>
    /// Settings that alter or enhance the behavior of the diff process.
    /// </summary>
    public class DiffSettings
    {
        private readonly List<IHint> _hints = new List<IHint>();
        private readonly List<ICaveat> _caveats = new List<ICaveat>();



        /// <summary>
        /// For the current array, set the diff mode to keyed and define the key.
        /// </summary>
        /// <param name="path">The path of the array</param>
        /// <param name="name">The key</param>
        /// <returns>The settings</returns>
        public DiffSettings KeyedBy(DiffPath path, string name)
        {
            _hints.Add(new ArrayDiffHint(path, name));

            return this;
        }


        /// <summary>
        /// For the current item, define the amount by which it can vary and still be considered the same
        /// </summary>
        /// <param name="path">The path of the item</param>
        /// <param name="amount">The amount of allowed variance</param>
        /// <returns>The settings</returns>
        public DiffSettings CanVaryBy(DiffPath path, double amount)
        {
            _caveats.Add(new VarianceCaveat(path, amount));

            return this;
        }


        internal IEnumerable<IHint> Hints { get { return _hints; } }
        internal IEnumerable<ICaveat> Caveats { get { return _caveats; } }
    }
}

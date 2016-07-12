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

using Difftaculous.Paths;


namespace Difftaculous.Results
{
    /// <summary>
    /// Annotation indicating that values differ.
    /// </summary>
    public class DifferingValuesAnnotation : AbstractDiffAnnotation
    {
        internal DifferingValuesAnnotation(DiffPath path, object valueA, object valueB, bool withinTolerance = false)
            : base(path)
        {
            ValueA = valueA;
            ValueB = valueB;
            WithinTolerance = withinTolerance;
        }


        /// <summary>
        /// An indication of whether this annotation is a difference
        /// </summary>
        public override bool AreSame
        {
            get { return WithinTolerance; }
        }


        /// <summary>
        /// The first value
        /// </summary>
        public object ValueA { get; private set; }


        /// <summary>
        /// The second value
        /// </summary>
        public object ValueB { get; private set; }


        /// <summary>
        /// True if this difference is within tolerance
        /// </summary>
        public bool WithinTolerance { get; set; }


        /// <summary>
        /// A human-readable explanation of the annotation.
        /// </summary>
        public override string Message
        {
            get { return string.Format("values differ: '{0}' vs. '{1}'", ValueA, ValueB); }
        }
    }
}

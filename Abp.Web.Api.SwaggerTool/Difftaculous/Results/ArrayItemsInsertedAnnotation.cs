
using Difftaculous.ArrayDiff;
using Difftaculous.Paths;


namespace Difftaculous.Results
{
    /// <summary>
    /// Annotation indicating that items were added to an array.
    /// </summary>
    public class ArrayItemsInsertedAnnotation : AbstractDiffAnnotation
    {
        internal ArrayItemsInsertedAnnotation(DiffPath path, ElementGroup group)
            : base(path)
        {
            Start = group.StartB;
            End = group.EndB;
        }


        /// <summary>
        /// The index of the first item removed.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// The index of the last item removed.
        /// </summary>
        public int End { get; private set; }


        /// <summary>
        /// A human-readable explanation of the annotation.
        /// </summary>
        public override string Message
        {
            get
            {
                if (Start == End)
                {
                    return string.Format("Item [{0}] added.", Start);
                }

                return string.Format("Items [{0}..{1}] added.", Start, End);
            }
        }
    }
}

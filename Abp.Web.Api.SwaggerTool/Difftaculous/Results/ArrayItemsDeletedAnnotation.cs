
using Difftaculous.ArrayDiff;
using Difftaculous.Paths;


namespace Difftaculous.Results
{
    /// <summary>
    /// Annotation indicating that items were removed from an array.
    /// </summary>
    public class ArrayItemsDeletedAnnotation : AbstractDiffAnnotation
    {
        internal ArrayItemsDeletedAnnotation(DiffPath path, ElementGroup group)
            : base(path)
        {
            Start = group.StartA;
            End = group.EndA;
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
                    return string.Format("Item [{0}] removed.", Start);
                }

                return string.Format("Items [{0}..{1}] removed.", Start, End);
            }
        }
    }
}

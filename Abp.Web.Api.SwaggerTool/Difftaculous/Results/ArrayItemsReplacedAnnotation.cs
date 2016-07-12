
using Difftaculous.ArrayDiff;
using Difftaculous.Paths;


namespace Difftaculous.Results
{
    /// <summary>
    /// Annotation indicating that items in the first array have been replaced by items in the second array.
    /// </summary>
    public class ArrayItemsReplacedAnnotation : AbstractDiffAnnotation
    {

        internal ArrayItemsReplacedAnnotation(DiffPath path, ElementGroup group)
            : base(path)
        {
            StartA = group.StartA;
            EndA = group.EndA;
            StartB = group.StartB;
            EndB = group.EndB;
        }


        /// <summary>
        /// The index of the first item removed.
        /// </summary>
        public int StartA { get; private set; }

        /// <summary>
        /// The index of the last item removed.
        /// </summary>
        public int EndA { get; private set; }

        /// <summary>
        /// The index of the first item added.
        /// </summary>
        public int StartB { get; private set; }

        /// <summary>
        /// The index of the last item added.
        /// </summary>
        public int EndB { get; private set; }


        /// <summary>
        /// A human-readable explanation of the annotation.
        /// </summary>
        public override string Message
        {
            get
            {
                if (StartA == StartB)
                {
                    if (StartA == EndA)
                    {
                        return string.Format("Element [{0}] changed.", StartA);
                    }

                    return string.Format("Elements [{0}..{1}] changed.", StartA, EndA);
                }

                if (StartA == EndA)
                {
                    return string.Format("Element [{0}] replaced by element [{1}].", StartA, StartB);
                }

                return string.Format("Elements [{0}..{1}] replaced by elements [{2}..{3}].", StartA, EndA, StartB, EndB);
            }
        }
    }
}

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


namespace Difftaculous.ArrayDiff
{
    internal class DiffArraySubsequencer : IArraySubsequencer
    {
        public List<ElementGroup> ComputeSubsequences(ZArray arrayA, ZArray arrayB)
        {
            List<ElementGroup> list = new List<ElementGroup>();

            // If both arrays are empty, this is easy...
            if ((arrayA.Count == 0) && (arrayB.Count == 0))
            {
                return list;
            }

            // Compute the diff and return it.
            // This uses the algorithm from http://en.wikipedia.org/wiki/Longest_common_subsequence_problem
            var c = ComputeArray(arrayA, arrayB);

            Compute(list, c, arrayA, arrayB, arrayA.Count, arrayB.Count);

            return ElementGroupPostProcessor.PostProcess(list);
        }



        private void Compute(List<ElementGroup> list, int[,] c, ZArray arrayA, ZArray arrayB, int i, int j)
        {
#if false
    if i > 0 and j > 0 and X[i] = Y[j]
        printDiff(C, X, Y, i-1, j-1)
        print "  " + X[i]
    else if j > 0 and (i = 0 or C[i,j-1] ≥ C[i-1,j])
        printDiff(C, X, Y, i, j-1)
        print "+ " + Y[j]
    else if i > 0 and (j = 0 or C[i,j-1] < C[i-1,j])
        printDiff(C, X, Y, i-1, j)
        print "- " + X[i]
    else
        print ""
#endif

            if ((i > 0) && (j > 0) && arrayA[i - 1].DeepEquals(arrayB[j - 1]))
            {
                Compute(list, c, arrayA, arrayB, i - 1, j - 1);

                var prev = (list.Count == 0) ? null : list[list.Count - 1];

                if ((prev != null)
                    && (prev.Operation == Operation.Equal)
                    && (prev.EndA == i - 2)
                    && (prev.EndB == j - 2))
                {
                    prev.Extend(1);
                }
                else
                {
                    list.Add(ElementGroup.Equal(i - 1, i - 1, j - 1, j - 1));
                }
            }
            else if ((j > 0) && ((i == 0) || (c[i, j - 1] >= c[i - 1, j])))
            {
                Compute(list, c, arrayA, arrayB, i, j - 1);

                var prev = (list.Count == 0) ? null : list[list.Count - 1];

                if ((prev != null)
                    && (prev.Operation == Operation.Insert)
                    && (prev.EndB == j - 2))
                {
                    prev.Extend(1);
                }
                else
                {
                    list.Add(ElementGroup.Insert(j - 1, j - 1));
                }
            }
            else if ((i > 0) && ((j == 0) || (c[i, j - 1] < c[i - 1, j])))
            {
                Compute(list, c, arrayA, arrayB, i - 1, j);

                var prev = (list.Count == 0) ? null : list[list.Count - 1];

                if ((prev != null)
                    && (prev.Operation == Operation.Delete)
                    && (prev.EndA == i - 2))
                {
                    prev.Extend(1);
                }
                else
                {
                    list.Add(ElementGroup.Delete(i - 1, i - 1));
                }
            }
        }


        private int[,] ComputeArray(ZArray arrayA, ZArray arrayB)
        {
#if false
function LCSLength(X[1..m], Y[1..n])
    C = array(0..m, 0..n)
    for i := 0..m
       C[i,0] = 0
    for j := 0..n
       C[0,j] = 0
    for i := 1..m
        for j := 1..n
            if X[i] = Y[j]
                C[i,j] := C[i-1,j-1] + 1
            else
                C[i,j] := max(C[i,j-1], C[i-1,j])
    return C[m,n]
#endif

            int[,] c = new int[arrayA.Count + 1, arrayB.Count + 1];

            for (int i = 0; i <= arrayA.Count; i++)
            {
                c[i, 0] = 0;
            }

            for (int j = 0; j <= arrayB.Count; j++)
            {
                c[0, j] = 0;
            }

            for (int i = 1; i <= arrayA.Count; i++)
            {
                for (int j = 1; j <= arrayB.Count; j++)
                {
                    if (arrayA[i - 1].DeepEquals(arrayB[j - 1]))
                    {
                        c[i, j] = c[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        c[i, j] = Math.Max(c[i, j - 1], c[i - 1, j]);
                    }
                }
            }

            return c;
        }
    }
}

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
using Difftaculous.ZModel;


namespace Difftaculous.ArrayDiff
{
    internal class IndexedArraySubsequencer : IArraySubsequencer
    {
        public List<ElementGroup> ComputeSubsequences(ZArray arrayA, ZArray arrayB)
        {
            List<ElementGroup> list = new List<ElementGroup>();

            if (arrayA.Count != arrayB.Count)
            {
                // TODO - this should go through all the items and diff, then put an Insert on the end
                list.Add(ElementGroup.Delete(0, arrayA.Count - 1));
                list.Add(ElementGroup.Insert(0, arrayB.Count - 1));
            }
            else
            {
                for (int i = 0; i < arrayA.Count; i++)
                {
                    var itemA = arrayA[i];
                    var itemB = arrayB[i];

                    // TODO - this code has gotten out of hand!  Rewrite it!  For the moment, I just want to see a couple of unit tests work...

                    // Are they equal?
                    bool equal = itemA.DeepEquals(itemB);

                    // If not, are there any caveats that make it acceptable?
                    bool acceptable = false;
                    if (!equal && (itemA is ZValue) && (itemB is ZValue))
                    {
                        string a = ((ZValue) itemA).Value.ToString();
                        string b = ((ZValue) itemA).Value.ToString();

                        foreach (var c in itemA.Caveats)
                        {
                            acceptable = c.IsAcceptable(a, b);
                            if (acceptable)
                            {
                                break;
                            }
                        }
                    }

                    // Handle each case...
                    if (equal || acceptable)    // TODO - handle these separately and return a new ElementGroup.Acceptable thingy?
                    {
                        // Match - can we just extend a prior match?
                        if ((list.Count >= 1) && (list[list.Count - 1].Operation == Operation.Equal))
                        {
                            list[list.Count - 1].Extend(1);
                        }
                        else
                        {
                            list.Add(ElementGroup.Equal(i, i, i, i));
                        }
                    }
                    else
                    {
                        // Not a match - are there any caveats?

                        // Not a match - can we extend a prior delete/insert pair?
                        if ((list.Count >= 2)
                            && (list[list.Count - 2].Operation == Operation.Delete)
                            && (list[list.Count - 1].Operation == Operation.Insert))
                        {
                            list[list.Count - 2].Extend(1);
                            list[list.Count - 1].Extend(1);
                        }
                        else
                        {
                            list.Add(ElementGroup.Delete(i, i));
                            list.Add(ElementGroup.Insert(i, i));
                        }
                    }
                }
            }

            return ElementGroupPostProcessor.PostProcess(list);
        }
    }
}

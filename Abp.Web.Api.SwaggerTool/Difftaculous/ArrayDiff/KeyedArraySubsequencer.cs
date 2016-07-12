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
using System.Linq;
using Difftaculous.Misc;
using Difftaculous.ZModel;


namespace Difftaculous.ArrayDiff
{
    internal class KeyedArraySubsequencer : IArraySubsequencer
    {
        private readonly string _key;


        public KeyedArraySubsequencer(string key)
        {
            _key = key;
        }



        public List<ElementGroup> ComputeSubsequences(ZArray arrayA, ZArray arrayB)
        {
            // This only works on arrays that contain objects (they're the only things
            // that have properties we can use as keys).  If we have non-objects, we have
            // a difference (or perhaps an error, or ??).
            if (arrayA.Any(x => x.Type != TokenType.Object) || arrayB.Any(x => x.Type != TokenType.Object))
            {
                throw new NotImplementedException("Keyed-array diff for non-objects is not implemented.");
            }

            var dictA = MakeDict(arrayA);
            var dictB = MakeDict(arrayB);

            var join = dictA.FullOuterJoin(dictB, x => x.Key, x => x.Key, (a, b, k) => new { Key = k, A = a, B = b })
                .OrderBy(x => (x.A.Value != null) ? x.A.Value.Index : int.MaxValue);

            List<ElementGroup> list = new List<ElementGroup>();

            foreach (var row in join)
            {
                if (row.A.Key == null)
                {
                    int bIndex = row.B.Value.Index;
                    var prev = (list.Count >= 1) ? list[list.Count - 1] : null;

                    if ((prev != null)
                        && (prev.Operation == Operation.Insert)
                        && (prev.EndB == bIndex - 1))
                    {
                        prev.Extend(1);
                    }
                    else
                    {
                        list.Add(ElementGroup.Insert(bIndex, bIndex));
                    }
                }
                else if (row.B.Key == null)
                {
                    int aIndex = row.A.Value.Index;
                    var prev = (list.Count >= 1) ? list[list.Count - 1] : null;

                    if ((prev != null)
                        && (prev.Operation == Operation.Delete)
                        && (prev.EndA == aIndex - 1))
                    {
                        prev.Extend(1);
                    }
                    else
                    {
                        list.Add(ElementGroup.Delete(aIndex, aIndex));
                    }
                }
                else
                {
                    int aIndex = row.A.Value.Index;
                    int bIndex = row.B.Value.Index;
                    var prev = (list.Count >= 1) ? list[list.Count - 1] : null;

                    var a = row.A.Value.Token;
                    var b = row.B.Value.Token;

                    if (a.DeepEquals(b))
                    {
                        if ((prev != null)
                            && (prev.Operation == Operation.Equal)
                            && (prev.EndA == aIndex - 1)
                            && (prev.EndB == bIndex - 1))
                        {
                            prev.Extend(1);
                        }
                        else
                        {
                            list.Add(ElementGroup.Equal(aIndex, aIndex, bIndex, bIndex));
                        }
                    }
                    else
                    {
                        var prevPrev = (list.Count >= 2) ? list[list.Count - 2] : null;

                        if ((prev != null)
                            && (prevPrev != null)
                            && (prevPrev.Operation == Operation.Delete)
                            && (prev.Operation == Operation.Insert)
                            && (prevPrev.EndA == aIndex - 1)
                            && (prev.EndB == bIndex - 1))
                        {
                            prev.Extend(1);
                            prevPrev.Extend(1);
                        }
                        else
                        {
                            list.Add(ElementGroup.Delete(aIndex, aIndex));
                            list.Add(ElementGroup.Insert(bIndex, bIndex));
                        }
                    }
                }
            }

            return ElementGroupPostProcessor.PostProcess(list);
        }



        private Dictionary<string, Entry> MakeDict(ZArray array)
        {
            var foo = array
                .Select((z, i) => new Entry
                {
                    Index = i,
                    Token = z,
                    Key = (string) ((ZValue) (((ZObject) z).Property(_key, false)).Value)
                })
                .ToDictionary(x => x.Key);

            return foo;
        }



        private class Entry
        {
            public int Index { get; set; }
            public string Key { get; set; }
            public ZToken Token { get; set; }
        }
    }
}

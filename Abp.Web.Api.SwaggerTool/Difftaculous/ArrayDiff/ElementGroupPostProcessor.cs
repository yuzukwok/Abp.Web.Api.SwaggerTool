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


namespace Difftaculous.ArrayDiff
{
    internal static class ElementGroupPostProcessor
    {


        public static List<ElementGroup> PostProcess(List<ElementGroup> groups)
        {
            // TODO - only create a new list if we actually need to

            List<ElementGroup> list = new List<ElementGroup>();

            for (int i = 0; i < groups.Count; i++)
            {
                var current = groups[i];
                var next = (i < groups.Count - 1) ? groups[i + 1]: null;

                if ((next != null)
                    && (current.Operation == Operation.Delete)
                    && (next.Operation == Operation.Insert)
                    && ((current.EndA - current.StartA) == (next.EndB - next.StartB)))
                {
                    // We have a delete followed by an insert of the same size.  Turn it into a Replace...
                    list.Add(ElementGroup.Replace(current, next));
                    i += 1;
                }
                else
                {
                    list.Add(groups[i]);
                }
            }

            return list;
        }
    }
}

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


namespace Difftaculous.ArrayDiff
{
    /// <summary>
    /// An operation that can be applied on an element group to transform one into the other
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// The two element groups are equal
        /// </summary>
        Equal,

        /// <summary>
        /// Insert the elements from B into A
        /// </summary>
        Insert,

        /// <summary>
        /// Delete the elements from A
        /// </summary>
        Delete,

        /// <summary>
        /// A combination of a delete and insert of the same size
        /// </summary>
        Replace
    };



    /// <summary>
    /// A group of array elements that have the same status (match, insert, update, delete)
    /// </summary>
    internal class ElementGroup
    {
        public static ElementGroup Delete(int start, int end)
        {
            return new ElementGroup
            {
                Operation = Operation.Delete,
                StartA = start,
                EndA = end
            };
        }


        public static ElementGroup Insert(int start, int end)
        {
            return new ElementGroup
            {
                Operation = Operation.Insert,
                StartB = start,
                EndB = end
            };
        }



        public static ElementGroup Equal(int startA, int endA, int startB, int endB)
        {
            return new ElementGroup
            {
                Operation = Operation.Equal,
                StartA = startA,
                EndA = endA,
                StartB = startB,
                EndB = endB
            };
        }



        public static ElementGroup Replace(int startA, int endA, int startB, int endB)
        {
            return new ElementGroup
            {
                Operation = Operation.Replace,
                StartA = startA,
                EndA = endA,
                StartB = startB,
                EndB = endB
            };
        }



        public static ElementGroup Replace(ElementGroup delete, ElementGroup insert)
        {
            if (delete.Operation != Operation.Delete)
            {
                throw new ArgumentException("The delete element group must be a delete", "delete");
            }

            if (insert.Operation != Operation.Insert)
            {
                throw new ArgumentException("The insert element group must be an insert", "insert");
            }

            return Replace(delete.StartA, delete.EndA, insert.StartB, insert.EndB);
        }



        private ElementGroup()
        {
        }


        public Operation Operation { get; private set; }

        public int StartA { get; private set; }
        public int EndA { get; private set; }

        public int StartB { get; private set; }
        public int EndB { get; private set; }


        public void Extend(int amount)
        {
            switch (Operation)
            {
                case Operation.Equal:
                    EndA += amount;
                    EndB += amount;
                    break;

                case Operation.Delete:
                    EndA += amount;
                    break;

                case Operation.Insert:
                    EndB += amount;
                    break;

                default:
                    throw new NotImplementedException("Extend() for operation " + Operation + " is not yet implemented.");
            }
        }



        public override string ToString()
        {
            switch (Operation)
            {
                case Operation.Equal:
                    return FormatPair("E");

                case Operation.Delete:
                    return string.Format("D({0}..{1})", StartA, EndA);

                case Operation.Insert:
                    return string.Format("I({0}..{1})", StartB, EndB);

                case Operation.Replace:
                    return FormatPair("R");

                default:
                    throw new NotImplementedException("ToString() for operation " + Operation + " is not yet implemented.");
            }
        }



        private string FormatPair(string prefix)
        {
            if ((StartA == EndA) && (StartB == EndB))
            {
                return string.Format("{0}({1},{2})", prefix, StartA, StartB);
            }

            if ((StartA == StartB) && (EndA == EndB))
            {
                return string.Format("{0}({1}..{2})", prefix, StartA, EndA);
            }

            return string.Format("{0}({1}..{2},{3}..{4})", prefix, StartA, EndA, StartB, EndB);
        }



        public override bool Equals(object obj)
        {
            ElementGroup other = obj as ElementGroup;

            if (other == null)
            {
                return false;
            }

            return ((Operation == other.Operation)
                && (StartA == other.StartA)
                && (EndA == other.EndA)
                && (StartB == other.StartB)
                && (EndB == other.EndB));
        }



        public override int GetHashCode()
        {
            return Operation.GetHashCode() ^ StartA.GetHashCode() ^ EndA.GetHashCode() ^ StartB.GetHashCode() ^ EndB.GetHashCode();
        }
    }
}

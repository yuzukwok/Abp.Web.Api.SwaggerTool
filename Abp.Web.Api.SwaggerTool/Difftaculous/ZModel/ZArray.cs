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

// ReSharper disable DoNotCallOverridableMethodsInConstructor


namespace Difftaculous.ZModel
{
    internal class ZArray : ZContainer, IList<ZToken>
    {
        private readonly List<ZToken> _values = new List<ZToken>();


        /// <summary>
        /// Gets the container's children tokens.
        /// </summary>
        /// <value>The container's children tokens.</value>
        protected override IList<ZToken> ChildrenTokens
        {
            get { return _values; }
        }

        /// <summary>
        /// Gets the node type for this <see cref="ZToken"/>.
        /// </summary>
        /// <value>The type.</value>
        public override TokenType Type
        {
            get { return TokenType.Array; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ZArray"/> class.
        /// </summary>
        public ZArray()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ZArray"/> class from another <see cref="ZArray"/> object.
        /// </summary>
        /// <param name="other">A <see cref="ZArray"/> object to copy from.</param>
        public ZArray(ZArray other)
            : base(other)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ZArray"/> class with the specified content.
        /// </summary>
        /// <param name="content">The contents of the array.</param>
        public ZArray(params object[] content)
            : this((object)content)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ZArray"/> class with the specified content.
        /// </summary>
        /// <param name="content">The contents of the array.</param>
        public ZArray(object content)
        {
            Add(content);
        }


        internal override bool DeepEquals(ZToken node)
        {
            ZArray t = node as ZArray;
            return (t != null && ContentsEqual(t));
        }


#if false


        internal override ZToken CloneToken()
        {
            return new ZArray(this);
        }

        /// <summary>
        /// Loads an <see cref="ZArray"/> from a <see cref="JsonReader"/>. 
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader"/> that will be read for the content of the <see cref="ZArray"/>.</param>
        /// <returns>A <see cref="ZArray"/> that contains the JSON that was read from the specified <see cref="JsonReader"/>.</returns>
        public new static ZArray Load(JsonReader reader)
        {
            if (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    throw JsonReaderException.Create(reader, "Error reading ZArray from JsonReader.");
            }

            while (reader.TokenType == JsonToken.Comment)
            {
                reader.Read();
            }

            if (reader.TokenType != JsonToken.StartArray)
                throw JsonReaderException.Create(reader, "Error reading ZArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));

            ZArray a = new ZArray();
            a.SetLineInfo(reader as IJsonLineInfo);

            a.ReadTokenFrom(reader);

            return a;
        }

        /// <summary>
        /// Load a <see cref="ZArray"/> from a string that contains JSON.
        /// </summary>
        /// <param name="json">A <see cref="String"/> that contains JSON.</param>
        /// <returns>A <see cref="ZArray"/> populated from the string that contains JSON.</returns>
        /// <example>
        ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParseArray" title="Parsing a JSON Array from Text" />
        /// </example>
        public new static ZArray Parse(string json)
        {
            using (JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                ZArray a = Load(reader);

                if (reader.Read() && reader.TokenType != JsonToken.Comment)
                    throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");

                return a;
            }
        }

        /// <summary>
        /// Creates a <see cref="ZArray"/> from an object.
        /// </summary>
        /// <param name="o">The object that will be used to create <see cref="ZArray"/>.</param>
        /// <returns>A <see cref="ZArray"/> with the values of the specified object</returns>
        public new static ZArray FromObject(object o)
        {
            return FromObject(o, JsonSerializer.CreateDefault());
        }

        /// <summary>
        /// Creates a <see cref="ZArray"/> from an object.
        /// </summary>
        /// <param name="o">The object that will be used to create <see cref="ZArray"/>.</param>
        /// <param name="jsonSerializer">The <see cref="JsonSerializer"/> that will be used to read the object.</param>
        /// <returns>A <see cref="ZArray"/> with the values of the specified object</returns>
        public new static ZArray FromObject(object o, JsonSerializer jsonSerializer)
        {
            ZToken token = FromObjectInternal(o, jsonSerializer);

            if (token.Type != ZTokenType.Array)
                throw new ArgumentException("Object serialized to {0}. ZArray instance expected.".FormatWith(CultureInfo.InvariantCulture, token.Type));

            return (ZArray)token;
        }

        /// <summary>
        /// Writes this token to a <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter"/> into which this method will write.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter"/> which will be used when writing the token.</param>
        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WriteStartArray();

            for (int i = 0; i < _values.Count; i++)
            {
                _values[i].WriteTo(writer, converters);
            }

            writer.WriteEndArray();
        }
#endif


        /// <summary>
        /// Gets the <see cref="ZToken"/> with the specified key.
        /// </summary>
        /// <value>The <see cref="ZToken"/> with the specified key.</value>
        public override ZToken this[object key]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(key, "o");

                if (!(key is int))
                {
                    throw new NotImplementedException();
                    //throw new ArgumentException("Accessed ZArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
                }

                return GetItem((int)key);
            }
            set
            {
                ValidationUtils.ArgumentNotNull(key, "o");

                if (!(key is int))
                {
                    throw new ArgumentException(string.Format("Set ZArray values with invalid key value: {0}. Array position index expected.", MiscellaneousUtils.ToString(key)));
                }

                SetItem((int)key, value);
            }
        }


        /// <summary>
        /// Gets or sets the <see cref="ZToken"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ZToken this[int index]
        {
            get { return GetItem(index); }
            set { SetItem(index, value); }
        }



        #region IList<ZToken> Members

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(ZToken item)
        {
            return IndexOfItem(item);
        }


        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, ZToken item)
        {
            InsertItem(index, item, false);
        }


        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            RemoveItemAt(index);
        }


        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ZToken> GetEnumerator()
        {
            return Children().GetEnumerator();
        }

        #endregion


        #region ICollection<ZToken> Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(ZToken item)
        {
            Add((object)item);
        }



        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            ClearItems();
        }


        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(ZToken item)
        {
            return ContainsItem(item);
        }



        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(ZToken[] array, int arrayIndex)
        {
            CopyItemsTo(array, arrayIndex);
        }


        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(ZToken item)
        {
            return RemoveItem(item);
        }

        #endregion


#if false
        internal override int GetDeepHashCode()
        {
            return ContentsHashCode();
        }
#endif
    }
}

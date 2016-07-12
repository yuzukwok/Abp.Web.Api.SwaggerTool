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


namespace Difftaculous.ZModel
{
    internal class ZProperty : ZContainer
    {
        private readonly List<ZToken> _content = new List<ZToken>();
        private readonly string _name;


        /// <summary>
        /// Gets the container's children tokens.
        /// </summary>
        /// <value>The container's children tokens.</value>
        protected override IList<ZToken> ChildrenTokens
        {
            get { return _content; }
        }


        /// <summary>
        /// Gets the property name.
        /// </summary>
        /// <value>The property name.</value>
        public string Name
        {
            // [DebuggerStepThrough]
            get { return _name; }
        }



        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>The property value.</value>
        public ZToken Value
        {
            // [DebuggerStepThrough]
            get { return (_content.Count > 0) ? _content[0] : null; }
            set
            {
                //CheckReentrancy();

                ZToken newValue = value ?? ZValue.CreateNull();

                if (_content.Count == 0)
                {
                    InsertItem(0, newValue, false);
                }
                else
                {
                    SetItem(0, newValue);
                }
            }
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ZProperty"/> class from another <see cref="ZProperty"/> object.
        /// </summary>
        /// <param name="other">A <see cref="ZProperty"/> object to copy from.</param>
        public ZProperty(ZProperty other)
            : base(other)
        {
            _name = other.Name;
        }



        internal override ZToken GetItem(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException();

            return Value;
        }


        internal override void SetItem(int index, ZToken item)
        {
            throw new NotImplementedException();

            //if (index != 0)
            //    throw new ArgumentOutOfRangeException();

            //if (IsTokenUnchanged(Value, item))
            //    return;

            //if (Parent != null)
            //    ((JObject)Parent).InternalPropertyChanging(this);

            //base.SetItem(0, item);

            //if (Parent != null)
            //    ((JObject)Parent).InternalPropertyChanged(this);
        }


        internal override bool RemoveItem(ZToken item)
        {
            throw new ZException(string.Format("Cannot add or remove items from {0}.", typeof(ZProperty)));
        }


        internal override void RemoveItemAt(int index)
        {
            throw new ZException(string.Format("Cannot add or remove items from {0}.", typeof(ZProperty)));
        }


        internal override void InsertItem(int index, ZToken item, bool skipParentCheck)
        {
            // don't add comments to ZProperty
            if (item != null && item.Type == TokenType.Comment)
                return;

            if (Value != null)
                throw new ZException(string.Format("{0} cannot have multiple values.", typeof(ZProperty)));

            base.InsertItem(0, item, false);
        }


        internal override bool ContainsItem(ZToken item)
        {
            return (Value == item);
        }


        internal override void ClearItems()
        {
            throw new ZException(string.Format("Cannot add or remove items from {0}.", typeof(ZProperty)));
        }



        internal override bool DeepEquals(ZToken node)
        {
            ZProperty t = node as ZProperty;
            return (t != null && _name == t.Name && ContentsEqual(t));
        }


#if false
        internal override ZToken CloneToken()
        {
            return new ZProperty(this);
        }
#endif


        /// <summary>
        /// Gets the node type for this <see cref="ZToken"/>.
        /// </summary>
        /// <value>The type.</value>
        public override TokenType Type
        {
            // [DebuggerStepThrough]
            get { return TokenType.Property; }
        }


#if false
        internal ZProperty(string name)
        {
            // called from JTokenWriter
            ValidationUtils.ArgumentNotNull(name, "name");

            _name = name;
        }
#endif


        /// <summary>
        /// Initializes a new instance of the <see cref="ZProperty"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="content">The property content.</param>
        public ZProperty(string name, params object[] content)
            : this(name, (object)content)
        {
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ZProperty"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="content">The property content.</param>
        public ZProperty(string name, object content)
        {
            ValidationUtils.ArgumentNotNull(name, "name");

            _name = name;

            Value = IsMultiContent(content)
                ? new ZArray(content)
                : CreateFromContent(content);
        }



#if false
        /// <summary>
        /// Writes this token to a <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter"/> into which this method will write.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter"/> which will be used when writing the token.</param>
        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WritePropertyName(_name);

            ZToken value = Value;
            if (value != null)
                value.WriteTo(writer, converters);
            else
                writer.WriteNull();
        }

        internal override int GetDeepHashCode()
        {
            return _name.GetHashCode() ^ ((Value != null) ? Value.GetDeepHashCode() : 0);
        }

        /// <summary>
        /// Loads an <see cref="ZProperty"/> from a <see cref="JsonReader"/>. 
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader"/> that will be read for the content of the <see cref="ZProperty"/>.</param>
        /// <returns>A <see cref="ZProperty"/> that contains the JSON that was read from the specified <see cref="JsonReader"/>.</returns>
        public new static ZProperty Load(JsonReader reader)
        {
            if (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    throw JsonReaderException.Create(reader, "Error reading ZProperty from JsonReader.");
            }

            while (reader.TokenType == JsonToken.Comment)
            {
                reader.Read();
            }

            if (reader.TokenType != JsonToken.PropertyName)
                throw JsonReaderException.Create(reader, "Error reading ZProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));

            ZProperty p = new ZProperty((string)reader.Value);
            p.SetLineInfo(reader as IJsonLineInfo);

            p.ReadTokenFrom(reader);

            return p;
        }
#endif
    }
}

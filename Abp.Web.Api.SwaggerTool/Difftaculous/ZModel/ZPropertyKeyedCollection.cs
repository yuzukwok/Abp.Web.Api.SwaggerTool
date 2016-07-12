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
using System.Collections.ObjectModel;
using System.Linq;


namespace Difftaculous.ZModel
{
    internal class ZPropertyKeyedCollection : Collection<ZToken>
    {

        private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

        private Dictionary<string, ZToken> _dictionary;


        private void AddKey(string key, ZToken item)
        {
            EnsureDictionary();
            _dictionary[key] = item;
        }



#if false
        protected void ChangeItemKey(ZToken item, string newKey)
        {
            if (!ContainsItem(item))
                throw new ArgumentException("The specified item does not exist in this KeyedCollection.");

            string keyForItem = GetKeyForItem(item);
            if (!Comparer.Equals(keyForItem, newKey))
            {
                if (newKey != null)
                    AddKey(newKey, item);

                if (keyForItem != null)
                    RemoveKey(keyForItem);
            }
        }
#endif

        protected override void ClearItems()
        {
            base.ClearItems();

            if (_dictionary != null)
                _dictionary.Clear();
        }



        public bool Contains(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (_dictionary != null)
                return _dictionary.ContainsKey(key);

            return false;
        }


#if false
        private bool ContainsItem(ZToken item)
        {
            if (_dictionary == null)
                return false;

            string key = GetKeyForItem(item);
            ZToken value;
            return _dictionary.TryGetValue(key, out value);
        }
#endif


        private void EnsureDictionary()
        {
            if (_dictionary == null)
            {
                _dictionary = new Dictionary<string, ZToken>(Comparer);
            }
        }


        private string GetKeyForItem(ZToken item)
        {
            return ((ZProperty)item).Name;
        }


        protected override void InsertItem(int index, ZToken item)
        {
            AddKey(GetKeyForItem(item), item);
            base.InsertItem(index, item);
        }



#if false
        public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (_dictionary != null)
                return _dictionary.ContainsKey(key) && Remove(_dictionary[key]);

            return false;
        }

        protected override void RemoveItem(int index)
        {
            string keyForItem = GetKeyForItem(Items[index]);
            RemoveKey(keyForItem);
            base.RemoveItem(index);
        }

        private void RemoveKey(string key)
        {
            if (_dictionary != null)
                _dictionary.Remove(key);
        }

        protected override void SetItem(int index, ZToken item)
        {
            string keyForItem = GetKeyForItem(item);
            string keyAtIndex = GetKeyForItem(Items[index]);

            if (Comparer.Equals(keyAtIndex, keyForItem))
            {
                if (_dictionary != null)
                    _dictionary[keyForItem] = item;
            }
            else
            {
                AddKey(keyForItem, item);

                if (keyAtIndex != null)
                    RemoveKey(keyAtIndex);
            }
            base.SetItem(index, item);
        }

        public ZToken this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                if (_dictionary != null)
                    return _dictionary[key];

                throw new KeyNotFoundException();
            }
        }
#endif


        public bool TryGetValue(string key, out ZToken value, bool caseSensitive = true)
        {
            if (_dictionary == null)
            {
                value = null;
                return false;
            }

            if (caseSensitive)
            {
                return _dictionary.TryGetValue(key, out value);
            }

            // TODO - figure out a better way to do this!

            string realKey = _dictionary.Keys.FirstOrDefault(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (realKey == null)
            {
                value = null;
                return false;
            }

            return _dictionary.TryGetValue(realKey, out value);
        }



        public ICollection<string> Keys
        {
            get
            {
                EnsureDictionary();
                return _dictionary.Keys;
            }
        }

        public ICollection<ZToken> Values
        {
            get
            {
                EnsureDictionary();
                return _dictionary.Values;
            }
        }



        public bool Compare(ZPropertyKeyedCollection other)
        {
            if (this == other)
                return true;

            // dictionaries in JavaScript aren't ordered
            // ignore order when comparing properties
            Dictionary<string, ZToken> d1 = _dictionary;
            Dictionary<string, ZToken> d2 = other._dictionary;

            if (d1 == null && d2 == null)
                return true;

            if (d1 == null)
                return (d2.Count == 0);

            if (d2 == null)
                return (d1.Count == 0);

            if (d1.Count != d2.Count)
                return false;

            foreach (KeyValuePair<string, ZToken> keyAndProperty in d1)
            {
                ZToken secondValue;
                if (!d2.TryGetValue(keyAndProperty.Key, out secondValue))
                    return false;

                ZProperty p1 = (ZProperty)keyAndProperty.Value;
                ZProperty p2 = (ZProperty)secondValue;

                if (p1.Value == null)
                    return (p2.Value == null);

                if (!p1.Value.DeepEquals(p2.Value))
                    return false;
            }

            return true;
        }
    }
}

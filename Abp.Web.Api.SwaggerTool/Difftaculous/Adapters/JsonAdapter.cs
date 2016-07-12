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
using Difftaculous.ZModel;
using Newtonsoft.Json.Linq;


namespace Difftaculous.Adapters
{
    /// <summary>
    /// Adapt JSON content so it can be run through the difference engine.
    /// </summary>
    public class JsonAdapter : AbstractAdapter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">The content to be adapted.</param>
        public JsonAdapter(string content)
        {
            var top = JToken.Parse(content);

            Content = Adapt(top);
        }



        private ZToken Adapt(JToken jtoken)
        {
            var type = jtoken.GetType();

            if (type == typeof(JObject))
            {
                return Adapt((JObject)jtoken);
            }

            if (type == typeof(JValue))
            {
                return Adapt((JValue)jtoken);
            }

            if (type == typeof(JArray))
            {
                return Adapt((JArray)jtoken);
            }

            throw new NotImplementedException("Adapting type '" + type.Name + "' is not yet implemented.");
        }


        private ZToken Adapt(JObject jobject)
        {
            ZObject zobject = new ZObject();

            foreach (var jprop in jobject.Properties())
            {
                zobject.Add(new ZProperty(jprop.Name, Adapt(jprop.Value)));
            }

            return zobject;
        }



        private ZToken Adapt(JValue jvalue)
        {
            // TODO - pass along the type?!

            ZValue zvalue = new ZValue(jvalue.Value);

            return zvalue;
        }



        private ZToken Adapt(JArray jarray)
        {
            ZArray zarray = new ZArray();

            foreach (var item in jarray)
            {
                zarray.Add(Adapt(item));
            }

            return zarray;
        }
    }
}

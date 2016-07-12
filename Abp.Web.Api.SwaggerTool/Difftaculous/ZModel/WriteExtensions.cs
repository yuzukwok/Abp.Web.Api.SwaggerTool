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
using System.IO;
using System.Text;


namespace Difftaculous.ZModel
{
    internal static class WriteExtensions
    {


        public static string AsJson(this ZToken token)
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                token.WriteJson(writer);
            }
            return builder.ToString();
        }



        public static void WriteJson(this ZToken token, TextWriter writer)
        {
            WriteTokenJson(token, writer);
        }



        private static void WriteTokenJson(ZToken token, TextWriter writer)
        {
            if (token is ZObject)
            {
                WriteJsonObject((ZObject)token, writer);
            }
            else if (token is ZValue)
            {
                WriteJsonValue((ZValue)token, writer);
            }
            else if (token is ZArray)
            {
                WriteJsonArray((ZArray)token, writer);
            }
            else
            {
                throw new NotImplementedException("Don't know how to write the JSON for: " + token.GetType().Name);
            }
        }


        private static void WriteJsonObject(ZObject obj, TextWriter writer)
        {
            writer.Write("{");

            bool first = true;
            foreach (var pair in obj)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Write(",");
                }

                writer.Write(" {0}: ", pair.Key);
                WriteTokenJson(pair.Value, writer);
            }

            writer.Write(" }");
        }



        private static void WriteJsonValue(ZValue val, TextWriter writer)
        {
            // TODO - what about multiple values?
            writer.Write(val.Value);
        }


        private static void WriteJsonArray(ZArray array, TextWriter writer)
        {
            writer.Write("[ ");

            bool first = true;
            foreach (var token in array)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Write(", ");
                }

                WriteTokenJson(token, writer);
            }

            writer.Write(" ]");
        }
    }
}

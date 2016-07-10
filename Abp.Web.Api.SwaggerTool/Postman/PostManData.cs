using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool.Postman
{
    public class PostMan
    {
        private static readonly RandomNumberGenerator Randgen = new RNGCryptoServiceProvider();
        private static readonly char[] UrlUnsafeBase64Chars = { '+', '/' };
        public static string GetId()
        {
            return Guid.NewGuid().ToString();
        }
        public static string GetRaddomId()
        {
            string base64Id;
            do
            {
                base64Id = CreateRandomBase64Id();
            } while (Base64StringContainsUrlUnfriendlyChars(base64Id));
            return base64Id;
        }
        public static long GetTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        private static string CreateRandomBase64Id()
        {
            var data = new byte[15];
            Randgen.GetBytes(data);
            return Convert.ToBase64String(data);
        }
        private static bool Base64StringContainsUrlUnfriendlyChars(string base64)
        {
            return base64.IndexOfAny(UrlUnsafeBase64Chars) >= 0;
        }
    }

    public class PostmanCollection
    {
        public PostmanCollection()
        {
            //不能为空
            order = new List<string>();
        }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string> order { get; set; }
        public long timestamp { get; set; }

        public List<Postfolder> folders { get; set; }
        public int owner { get; set; }
        public string remoteLink { get; set; }
        public bool @public{get;set;}
        public List<PostmanRequest> requests { get; set; }
    }

    public class Postfolder
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public IList<string> order { get; set; }
        public int owner { get; set; }
    }

    public class PostmanRequest
    {
        
        public string id { get; set; }
        public string name { get; set; }
        public string dataMode { get; set; }
        public List<PostmanData> data { get; set; }
        public string description { get; set; }
        public string descriptionFormat { get; set; }
        public string headers { get; set; }
        public string method { get; set; }
        public Dictionary<string, string> pathVariables { get; set; }
        public string url { get; set; }
        public int version { get; set; }
        public string collectionId { get; set; }
        public string folder { get; set; }
        public string rawModeData { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string tagname { get; set; }
    }

    public class PostmanData
    {
        public string key { get; set; }
        public string value { get; set; }
        public string type
        {
            get { return "text"; }
        }
        public bool enabled
        {
            get { return true; }
        }
    }
}

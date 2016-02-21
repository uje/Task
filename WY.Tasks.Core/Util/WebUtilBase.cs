using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace WY.Util {

    public class WebUtilBase {

        private string userAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0; .NET4.0E; .NET4.0C; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729)";
        private int timeout = 60000;
        private Encoding encoding = Encoding.UTF8;
        private bool compress = false;
        private Version version = HttpVersion.Version11;
        private RequestMethod method = RequestMethod.GET;
        private CookieContainer cookies = new CookieContainer();
        private string urlReferr = string.Empty;
        private string contentType = "application/x-www-form-urlencoded;charset=utf-8";
        public string UserAgent { get { return userAgent; } set { userAgent = value; } }
        public string UrlReferr { get { return urlReferr; } set { urlReferr = value; } }
        public int Timeout { get { return timeout; } set { timeout = value; } }
        public Encoding Charset { get { return encoding; } set { encoding = value; } }
        public string ContentType { get { return contentType; } set { contentType = value; } }
        public bool Compress { get { return compress; } set { compress = value; } }
        public Version Version { get { return version; } set { value = version; } }
        public RequestMethod Method { get { return method; } set { value = method; } }
        public bool KeepAlive { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Url { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public CookieContainer Cookies { get { return cookies; } set { cookies = value; } }

        public virtual IWebProxy Proxy { get; set; }

        public string Get() {
            if (Method == RequestMethod.GET) {
                return Get(Url, Data, Headers);
            }
            else if (Method == RequestMethod.POST) {
                return Post(Url, Data, Headers);
            }
            throw new NotImplementedException();
        }

        public byte[] GetBytes() {
            using (var s = GetStream(this.Url)) {
                return s.ToArray();
            }
        }
        public Stream GetStream() {
            return GetStream(this.Url);
        }

        public string Get(string url,
                          Dictionary<string, object> data = null,
                          Dictionary<string, string> headers = null) {

            if (data != null) {
                UriBuilder builder = new UriBuilder(url);
                builder.Query = BuildQueryString(data);
                url = builder.Uri.ToString();
            }

            HttpWebRequest req = CreateHttpRequest(url, RequestMethod.GET, headers);
            using (HttpWebResponse rsp = req.GetResponse() as HttpWebResponse) {
                InternalSet(rsp);
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet == null || string.IsNullOrEmpty(rsp.CharacterSet) ? Encoding.UTF8.WebName : rsp.CharacterSet);
                return GetResponseAsString(rsp);
            }

        }

        public MemoryStream GetStream(string url,
                  Dictionary<string, object> data = null,
                  Dictionary<string, string> headers = null) {

            if (data != null) {
                UriBuilder builder = new UriBuilder(url);
                builder.Query = BuildQueryString(data);
                url = builder.Uri.ToString();
            }

            HttpWebRequest req = CreateHttpRequest(url, RequestMethod.GET, headers);
            using (HttpWebResponse rsp = req.GetResponse() as HttpWebResponse) {
                InternalSet(rsp);

                return ToMemoryStream(rsp.GetResponseStream());
            }

        }

        public string Post(string url,
                          string data,
                          Dictionary<string, string> headers = null) {

            HttpWebRequest req = CreateHttpRequest(url, RequestMethod.POST, headers);
            if (string.IsNullOrEmpty(data)) {
                using (Stream s = req.GetRequestStream()) {
                    byte[] postData = this.Charset.GetBytes(data);
                    s.Write(postData, 0, postData.Length);
                    s.Close();
                }

            } 
            using (HttpWebResponse rsp = req.GetResponse() as HttpWebResponse) {
                InternalSet(rsp);
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet == null || string.IsNullOrEmpty(rsp.CharacterSet) ? Encoding.UTF8.WebName : rsp.CharacterSet);
                return GetResponseAsString(rsp);
            }
        }
        public string Post(string url,
                          Dictionary<string, object> data,
                          Dictionary<string, string> headers = null) {

            HttpWebRequest req = CreateHttpRequest(url, RequestMethod.POST, headers);

            if (data != null) {
                bool hasByte = data.Where(t => IsBinaryData(t.Value)).Count() > 0;
                using (Stream s = req.GetRequestStream()) {
                    byte[] postData = null;
                    if (hasByte) {
                        string boundary = GenerateTimeStamp();
                        req.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                        req.AllowWriteStreamBuffering = true;
                        req.KeepAlive = true;
                        postData = BuildPostData(boundary, data);
                    }
                    else {
                        postData = this.Charset.GetBytes(BuildQueryString(data));
                    }
                    s.Write(postData, 0, postData.Length);
                    s.Close();
                }
            }


            using (HttpWebResponse rsp = req.GetResponse() as HttpWebResponse) {
                InternalSet(rsp);
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet == null || string.IsNullOrEmpty(rsp.CharacterSet) ? Encoding.UTF8.WebName : rsp.CharacterSet);
                return GetResponseAsString(rsp);
            }
        }

        protected void InternalSet(HttpWebResponse rsp) {
            if (rsp.Cookies != null) {
                Cookies = new CookieContainer();
            }
            foreach (Cookie c in rsp.Cookies) {
                Cookies.Add(c);
            }

            UrlReferr = rsp.ResponseUri.ToString();
        }

        protected string GetResponseAsString(HttpWebResponse rsp) {
            var result = new StringBuilder();
            Stream s = null;
            StreamReader sr = null;

            try {
                s = rsp.GetResponseStream();
                switch (rsp.ContentEncoding) {
                    case "gzip":
                        s = new GZipStream(s, CompressionMode.Decompress);
                        break;
                    case "deflate":
                        s = new DeflateStream(s, CompressionMode.Decompress);
                        break;
                }

                sr = new StreamReader(s, encoding);
                result.Append(sr.ReadToEnd());
            }
            finally {
                if (s != null) s.Close();
                if (sr != null) sr.Close();
                if (rsp != null) rsp.Close();
            }
            return result.ToString();
        }
        protected HttpWebRequest CreateHttpRequest(string url, RequestMethod method, Dictionary<string, string> headers) {

            var req = WebRequest.Create(url) as HttpWebRequest;
            req.ServicePoint.Expect100Continue = false;
            req.Method = method.ToString();
            req.KeepAlive = KeepAlive;
            req.ProtocolVersion = Version;
            req.UserAgent = UserAgent;
            req.Timeout = Timeout;
            req.ContentType = ContentType;
            req.Referer = UrlReferr;


            if (Compress) 
                req.Headers.Add("Accept-Encoding", "gizp,deflate");
            
            if (Cookies != null) 
                req.CookieContainer = Cookies;

            if (Proxy != null)
                req.Proxy = Proxy;
            

            if (headers != null) {
                foreach (var header in Headers) {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

            return req;
        }
        protected byte[] BuildPostData(string boundary, Dictionary<string, object> parms) {

            var pairs = parms.ToList();
            byte[] headerBuff = this.Charset.GetBytes(string.Format("\r\n--{0}\r\n", boundary));
            byte[] footerBuff = this.Charset.GetBytes(string.Format("\r\n--{0}--\r\n", boundary));

            MemoryStream ms = new MemoryStream();
            foreach (var p in parms) {
                if (!IsBinaryData(p.Value)) {
                    if (p.Value is string && string.IsNullOrEmpty(string.Format("{0}", p.Value))) {
                        continue;
                    }


                    ms.Write(headerBuff, 0, headerBuff.Length);
                    byte[] dispositonBuff = this.Charset.GetBytes(string.Format("content-disposition: form-data; name=\"{0}\"\r\n\r\n{1}", p.Key, p.Value.ToString()));
                    ms.Write(dispositonBuff, 0, dispositonBuff.Length);

                }
                else {
                    ms.Write(headerBuff, 0, headerBuff.Length);
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: \"image/unknow\"\r\nContent-Transfer-Encoding: binary\r\n\r\n";
                    byte[] fileBuff = this.Charset.GetBytes(string.Format(headerTemplate, p.Key, string.Format("upload{0}.png", BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0))));
                    ms.Write(fileBuff, 0, fileBuff.Length);
                    byte[] file = (byte[])p.Value;
                    ms.Write(file, 0, file.Length);
                }
            }

            ms.Write(footerBuff, 0, footerBuff.Length);
            ms.Position = 0;

            return ToBytes(ms);

        }

        public static MemoryStream ToMemoryStream(Stream s) {
            MemoryStream ms = new MemoryStream();
            byte[] byteBuffers = new byte[1024];
            var bytesRead = 0;
            while ((bytesRead = s.Read(byteBuffers, 0, byteBuffers.Length)) > 0) {
                ms.Write(byteBuffers, 0, bytesRead);
            }

            return ms;
        }

        public static byte[] ToBytes(Stream s) {
            byte[] by = new byte[s.Length];
            s.Read(by, 0, by.Length);
            s.Seek(0, SeekOrigin.Begin);

            return by;
        }

        /// <summary>
        /// Generate the UNIX style timestamp for DateTime.UtcNow        
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp() {
            return GenerateTimeStamp(DateTime.UtcNow, false);
        }
        /// <summary>
        /// Generate the UNIX style timestamp for DateTime.UtcNow        
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp(DateTime dt, bool isMiliseconds) {
            TimeSpan ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (isMiliseconds) {
                ts = dt - new DateTime(1970, 1, 1, 8, 0, 0, 0);
                return Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }
            else {
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonce() {
            // Just a simple implementation of a RandomGenerator number between 123400 and 9999999
            return new Random().Next(123400, 9999999).ToString();
        }

        public static string BuildQueryString(Dictionary<string, object> parms, bool IsEncode = true) {
            var querys = new List<string>();
            foreach (var param in parms) {
                if (param.Value == null || string.IsNullOrEmpty(param.Value.ToString()))
                    continue;

                //if (param is TencentParam) {
                //    var p = param as TencentParam;
                //    querys.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(p.Name), p.Value));
                //    continue;
                //}
                querys.Add(string.Format("{0}={1}", param.Key, Uri.EscapeDataString(param.Value.ToString())));
            }

            return string.Join("&", querys.ToArray());
        }
        /// <summary>
        /// 将query转为字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryToDictionary(string query) {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (query.IndexOf('&') == -1) {
                if (query.IndexOf('=') != -1) {
                    var nameValue = query.Split('=');
                    dict.Add(Uri.UnescapeDataString(nameValue[0]), Uri.UnescapeDataString(nameValue[1]));
                }
                return dict;
            }
            var values = query.Split('&');
            foreach (var item in values) {
                dict.Concat(QueryToDictionary(item));
            }
            return dict;
        }

        /// <summary>
        /// 是否为二进制参数（如图片、文件等）
        /// </summary>
        public static bool IsBinaryData(object value) {

            if (value != null && value.GetType() == typeof(byte[]))
                return true;
            else
                return false;

        }

        /// <summary>
        /// 提交方法 
        /// </summary>
        public enum RequestMethod {
            GET, POST
        }
    }

}

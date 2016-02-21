using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using WY.Util;

namespace WY.Tasks.Plugin.Combine {
    internal class WebUtil : WebUtilBase {

        public override System.Net.IWebProxy Proxy {
            get {
                string proxy = ConfigurationManager.AppSettings["proxy"];

                if (proxy != null) {
                    var _proxy = proxy.Split(':');
                    var ip = _proxy[0];
                    var port = _proxy[1];
                    return new WebProxy(ip, int.Parse(port));
                }

                return null;
            }
        }

        public long GetFileLength(string url) {
            var request = this.CreateHttpRequest(url, RequestMethod.GET, null);
            using (var response = request.GetResponse()) {
                var length = response.Headers["content-length"];
                if (length != null && !string.IsNullOrEmpty(length)) {
                    return long.Parse(length);
                }
                if (response.ContentLength != 0) {
                    return response.ContentLength;
                }
            }
            throw new Exception("无法远程文件获取长度");
        }
    }
}

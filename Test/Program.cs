using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Test {
    class Program {
        static WY.Util.WebUtilBase util = new WY.Util.WebUtilBase();
        static void Main(string[] args) {

            //TestTask tt = new TestTask();
            //tt.Execute();

            //util.Url = "https://www.google.com/search?q=%E9%A3%8E%E6%99%AF%E5%A3%81%E7%BA%B8&hl=zh-CN&tbo=d&biw=1280&bih=706&tbm=isch";
            //var result = util.Get();
            //var urls = Regex.Matches(result, "imgrefurl=[^\"]+", RegexOptions.IgnoreCase);
            //var url = urls[0].Value;
            //var imgref = Regex.Match(url, "imgrefurl=([^&]+)").Result("$1");
            //var imgurl = Regex.Match(url, "imgurl=([^&]+)").Result("$1");
            //var bytes = GetBytes(imgref, imgurl);

            var baseUrl = string.Format("http://ptlogin2.qq.com/check?uin=939567050&appid=46000101&r=",GenerateNonce());
            //var loginParam = string.Format("VER=1.1&CMD=Login&SEQ={0}&UIN=6004198&PS={1}&M5=1&LC=9326B87B234E7235", new Random().Next(1000,9000),WY.Util.StringUtil.Md5Encode("WY.net2003"));
            util.UrlReferr = "http://web.qq.com";
            var txt = util.Get(baseUrl);
        }

        static byte[] GetBytes(string referrUrl,string url){
            util.UrlReferr = referrUrl;
            util.Url = url;
            return util.GetBytes();
        }

        public static string GenerateNonce() {
            // Just a simple implementation of a RandomGenerator number between 123400 and 9999999
            return new Random().Next(123400, 9999999).ToString();
        }
       
    }
}

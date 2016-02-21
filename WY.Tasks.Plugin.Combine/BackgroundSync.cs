using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WY.Tasks.Api;
using WY.Util;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace WY.Tasks.Plugin.Combine {
    public class BackgroundSync : Task {

        string bgDir = "";
        MatchCollection images;
        static WebUtilBase web;
        readonly string[] defaultKeywords = new string[] { "卡通", "超清", "风景", "清新", "旅游胜地", "山水", "花草", "动物" };
        public string[] Keywords {
            get {
                if (ConfigurationManager.AppSettings["keywords"].IsNullOrWhiteSpace()) {
                    AppUtil.WriteSetting("keywords", string.Join(",", defaultKeywords));
                }
                if (!ConfigurationManager.AppSettings["keywords"].IsNullOrWhiteSpace() &&
                    ConfigurationManager.AppSettings["keywords"].IndexOf(",") != -1) {
                    return ConfigurationManager.AppSettings["keywords"].Split(',');
                }
                return new string[] { ConfigurationManager.AppSettings["keywords"] };
            }
        }
        public BackgroundSync() {

            web = Util.GetWebUtil();
            var keyword = Keywords[GetIndex(Keywords.Length)];
            var screen = System.Windows.Forms.Screen.GetWorkingArea(new Point(0, 0));
            Logger.Write("当前壁纸关键词为：{0},屏幕宽度：{1},高度：{2}", keyword, screen.Width, screen.Height);
            web.Url = string.Format("http://cn.bing.com/images/search?q={0}&FORM=HDRSC2", keyword);
            web.KeepAlive = true;
            web.Method = WY.Util.WebUtilBase.RequestMethod.GET;
            web.Compress = true;

            bgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BackgroundSync.Data");

            var di = new DirectoryInfo(bgDir);
            if (!di.Exists) {
                di.Create();
            }
        }

        public int GetIndex(int count) {
            return new Random().Next(0, count);
        }

        static Stream GetStream(string url) {
            WebUtilBase util = new WebUtilBase();
            util.Url = url;
            return util.GetStream();
        }

        #region Win32 API
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfoA", SetLastError = true)]
        static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, string lpvParam, Int32 fuWinIni);//////lpvParam要设置成string
        private const int SPI_SETDESKWALLPAPER = 20;

        #endregion

        public override void Execute() {

            if (images == null) {
                Logger.Write("获取壁纸列表");
                var result = web.Get();
                images = Regex.Matches(result, "imgurl:&quot;([^&]+)", RegexOptions.IgnoreCase);
            }

            var url = images[GetIndex(images.Count)].Result("$1");
            var bgName = Path.Combine(bgDir, StringUtil.Md5Encode(url) + ".jpg");
            if (!File.Exists(bgName)) {
                DateTime bgStartTime = DateTime.Now;
                using (var s = GetStream(url)) {
                    var image = Image.FromStream(s);
                    image.Save(bgName, ImageFormat.Bmp);
                }
                DateTime bgEndTime = DateTime.Now;

                Logger.Write("下载图片:{0},耗时：{1}秒", url, (bgEndTime - bgStartTime).Seconds);
            }


            var value = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, bgName, 1);
            if (value == 0) {
                var errorCode = Marshal.GetLastWin32Error();
                Logger.Write("出现错误，错误代码为：{0}", errorCode);
                return;
            }
            Logger.Write("设置壁纸成功！本地路径：{0},结果：{1}", bgName, value);
        }
    }
}

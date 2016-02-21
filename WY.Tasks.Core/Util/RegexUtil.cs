using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WY.Util {
    public class RegexUtil {
        /// <summary>
        /// 验证是否邮箱格式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(string source) {
            string pattern = @"^([a-zA-Z0-9.-]+)@([a-zA-Z0-9-]+)((\.[a-zA-Z0-9-]{2,3}){1,2})$";
            return Regex.IsMatch(source, pattern,RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否中国手机号码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsMobile(string source) {
            string pattern = @"^(13[0-9]|15[0|3|6|7|8|9]|18[0-9])\d{8}$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否中国电话号码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPhone(string source) {
            string pattern = @"(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否数字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumber(string source) {
            string pattern = @"^\d+$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否网址
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(string source) {
            string pattern = @"^(?:(?:http(?:s)?):\/\/(?:[a-zA-Z0-9\u4E00-\u9FA5.-_]{1,})(?:\:[0-9]{1,5})?(?:\/[^\s\r\n\u4E00-\u9FA5]*)?)$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否正确的用户ID
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUserID(string source) {
            string pattern = @"^([a-zA-Z]{1,}[^\u4E00-\u9FA5])([a-zA-Z0-9]+)$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否正确的用户名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUserName(string source) {
            string pattern = @"^([a-zA-Z\u4E00-\u9FA5]{1})([a-zA-Z0-9\u4E00-\u9FA5]+){1,19}$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否中文 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsChinese(string source) {
            string pattern = @"^[\u4E00-\u9FA5]+$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证是否英文
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEng(string source) {
            string pattern = @"^[a-zA-Z]+[a-zA-Z\s]*$";
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WY.Util {
    /// <summary>
    /// 转换类
    /// </summary>
    public class ConvertUtil {
        public static DateTime? ToDateTime(string date) {
            DateTime dateTime;
            bool isTranslate = DateTime.TryParse(date, out dateTime);
            if (isTranslate)
                return dateTime;
            return null;
        }

        /// <summary>
        /// 转换成时间，当date无法转换时返回默认时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="defaultDateTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string date, DateTime defaultDateTime) {
            var dateTime = ToDateTime(date);
            return dateTime.HasValue ? dateTime.Value : defaultDateTime;
        }
        public static string ToString(object obj) {
            return Convert.ToString(obj) ?? "";
        }
        /// <summary>
        /// 当对象为空时返回默认字串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string ToString(object obj, string defaultStr) {
            return string.IsNullOrEmpty(Convert.ToString(obj) ?? "") ? defaultStr : obj.ToString();
        }

        public static bool ToBool(object obj, bool defaultValue) {
            bool value = false;
            return !Convert.IsDBNull(obj) && bool.TryParse(obj.ToString(), out value) ?  value :defaultValue;
        }
        public static int? ToInt(string str) {
            int data;
            bool isTranslate = int.TryParse(str, out data);
            if (isTranslate)
                return data;
            return null;
        }
        public static int ToInt(string str, int defaultInt) {
            var data = ToInt(str);
            return data.HasValue ? data.Value : defaultInt;
        }
        public static long? ToLong(string str) {
            long data;
            bool isTranslate = long.TryParse(str, out data);
            if (isTranslate)
                return data;
            return null;
        }
        public static long ToLong(string str, long defaultInt) {
            var data = ToLong(str);
            return data.HasValue ? data.Value : defaultInt;
        }
    }
}
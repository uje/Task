using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web.Script.Serialization;

namespace WY.Util {
    public class SerializeUtil {
        #region XML与对象转换
        /// <summary>
        /// 将对象转换为XML
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>XML字串</returns>
        public static string XmlSerializer<T>(T t) {
            var type = typeof(T);
            var serializer = new XmlSerializer(type);
            using (var ms = new MemoryStream()) {
                serializer.Serialize(ms, t);
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        /// <summary>
        /// 将XML转换为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML文本</param>
        /// <returns>对象</returns>
        public static T XmlDeSerializer<T>(string xml) {
            var serializer = new XmlSerializer(typeof(T));
            var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));
            var t = (T)serializer.Deserialize(ms);
            return t;
        }
        #endregion

        #region JSON与对象转换
        /// <summary>
        /// 将对象转换为Json文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>Json文本</returns>
        public static string JsonSerializer(object obj) {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
        /// <summary>
        /// 将JSON文本系列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonText">JSON文本</param>
        /// <returns>对象</returns>
        public static T JsonDeSerializer<T>(string jsonText) {
            var jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(jsonText);
        }
        #endregion

        #region XML与JSON转换
        /// <summary>
        /// 将XML转换为JSON
        /// </summary>
        /// <typeparam name="T">XML模型对象</typeparam>
        /// <param name="xml">XML文本</param>
        /// <returns>JSON字口串</returns>
        public static string XmlToJson<T>(string xml) {
            var t = XmlDeSerializer<T>(xml);
            return JsonSerializer(t);
        }
        /// <summary>
        /// 将JSON文本转换为XML文本
        /// </summary>
        /// <typeparam name="T">JSON对象模型</typeparam>
        /// <param name="Json">JSON文本</param>
        /// <returns>XML文本</returns>
        public static string JsonToXml<T>(string Json) {
            var t = JsonDeSerializer<T>(Json);
            return XmlSerializer<T>(t);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace WY.Util {
    public class FileUtil {
        /// <summary>
        /// 将文件移动至另一位置
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        /// <param name="overrideFileIsExist">是否覆盖</param>
        public static void MoveFile(string sourceFile, string destFile,bool overrideFileIsExist) {
            FileInfo source = new FileInfo(sourceFile);
            FileInfo fileInfo = new FileInfo(destFile);

            DirectoryInfo dir = Directory.GetParent(destFile);
            if (!dir.Exists)
                dir.Create();

            if (!source.Exists)
                throw new IOException("sourceFile is not exist");

            if (source.Length == 0)
                throw new IOException("file is null");

            if (fileInfo.Exists && overrideFileIsExist)
                fileInfo.Delete();

            source.MoveTo(destFile);
        }

        public static string GetFileNameWithoutExtension(string fileName) {
            if (fileName.IndexOf(".") == -1) {
                return fileName;
            }
            return Regex.Match(fileName,@"^(.+)\.[a-zA-Z]+$").Result("$1");
        }
       
    }
}

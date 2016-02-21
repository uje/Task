using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace WY.Util {
    /// <summary>
    /// 字符串处理类
    /// </summary>
    public static class StringUtil {
        private const string DESKey = "9722a43b9558421798542990702aaf50";

        /// <summary>
        /// 把阿拉伯数字的金额转换为中文大写数字
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ConvertToChinese(double x) {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", delegate(Match m) { return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
        }

        /// <summary>
        /// 将字串转换为数组
        /// </summary>
        /// <param name="source"></param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static string[] StringToArray(string source, string split) {
            List<string> data = new List<string>();
            if (source.IndexOf(split) == -1) {
                data.Add(source);
            }
            else {
                data.AddRange(source.Split(new string[]{ split},StringSplitOptions.None));
            }

            return data.ToArray();
        }

        /// <summary>
        /// 将字串转换为数字数组
        /// </summary>
        /// <param name="source"></param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static int[] StringToIntArray(string source, string split) {
            List<int> data = new List<int>();
            if (source.IndexOf(split) == -1) {
                data.Add(int.Parse(source));
            }
            else {
                var array = StringToArray(source, split);
                foreach (var str in array) {
                    data.Add(int.Parse(str));
                }
            }

            return data.ToArray();
        }

        /// <summary>
        /// 获取字串,当前面字串为空时选取后面的字串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetString(params string[] s) {
            foreach (var c in s) {
                if(!string.IsNullOrEmpty(c)){
                    return c;
                }
            }
            return "";
        }
        /// <summary>
        /// 获取字串，当前面字串为空时选取后面的字串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetString(params object[] s) {
            foreach (var c in s) {
                if (c != null && !string.IsNullOrEmpty(c.ToString().Trim())) {
                    return c.ToString().Trim();
                }
            }
            return "";
        }


        /// <summary>
        /// 把字符串进行 MD5 加密(XXX)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MD5String(string s) {
            byte[] hashvalue = (new System.Security.Cryptography.MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(s));
            return BitConverter.ToString(hashvalue).Replace("-", "").ToLower();
        
        }


        /// <summary>
        /// 把字符串进行 MD5 编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Md5Encode(string s) {

            var md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(s));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string DESEncrypt(string key, string iv, string s) {
            DES_ des = new DES_(key, iv);
            return des.Encrypt(s);
        }

        /// <summary>
        /// 用AES加密字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string AESEncrypt(string key, string iv, string s) {
            return AES_.EncryptString(s, key, iv);
        }

        /// <summary>
        /// 用AES解密字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string AESDecrypt(string key, string iv, string s) {
            return AES_.DecryptString(s, key, iv);
        }

        /// <summary>
        /// 用DES加密字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DESEncrypt(string key, string s) {
            DES_ des = new DES_(key);
            return des.Encrypt(s);
        }

        public static string DESEncrypt(string s) {
            return DESEncrypt(DESKey, s);
        }

        public static string DESDecrypt(string key, string iv, string s) {
            DES_ des = new DES_(key, iv);
            return des.Decrypt(s);
        }

        /// <summary>
        /// 用DES解密字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DESDecrypt(string key, string s) {
            DES_ des = new DES_(key);
            return des.Decrypt(s);
        }

        public static string DESDecrypt(string s) {
            return DESDecrypt(DESKey, s);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeBase64(string source) {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string DecodeBase64(string result) {
            return Encoding.UTF8.GetString(Convert.FromBase64String(result));
        }

        /// <summary>
        /// DES
        /// </summary>
        private class DES_ {
            private DES mydes;
            public string Key;
            public string IV;
            /// <summary>
            /// 对称加密类的构造函数
            /// </summary>
            public DES_(string key) {
                mydes = new DESCryptoServiceProvider();
                Key = key;
                IV = "728#$$%^TyguyshdsufhsfwofnhKJHJKHIYhfiusf98*(^%$^&&(*&()$##@%%$RHGJJHHJ";
            }
            /// <summary>
            /// 对称加密类的构造函数
            /// </summary>
            public DES_(string key, string iv) {
                mydes = new DESCryptoServiceProvider();
                Key = key;
                IV = iv;
            }
            /// <summary>
            /// 获得密钥
            /// </summary>
            /// <returns>密钥</returns>
            private byte[] GetLegalKey() {
                string sTemp = Key;
                mydes.GenerateKey();
                byte[] bytTemp = mydes.Key;
                int KeyLength = bytTemp.Length;
                if (sTemp.Length > KeyLength)
                    sTemp = sTemp.Substring(0, KeyLength);
                else if (sTemp.Length < KeyLength)
                    sTemp = sTemp.PadRight(KeyLength, ' ');
                return ASCIIEncoding.ASCII.GetBytes(sTemp);
            }
            /// <summary>
            /// 获得初始向量IV
            /// </summary>
            /// <returns>初试向量IV</returns>
            private byte[] GetLegalIV() {
                string sTemp = IV;
                mydes.GenerateIV();
                byte[] bytTemp = mydes.IV;
                int IVLength = bytTemp.Length;
                if (sTemp.Length > IVLength)
                    sTemp = sTemp.Substring(0, IVLength);
                else if (sTemp.Length < IVLength)
                    sTemp = sTemp.PadRight(IVLength, ' ');
                return ASCIIEncoding.ASCII.GetBytes(sTemp);
            }
            /// <summary>
            /// 加密方法
            /// </summary>
            /// <param name="Source">待加密的串</param>
            /// <returns>经过加密的串</returns>
            public string Encrypt(string Source) {
                try {
                    byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                    MemoryStream ms = new MemoryStream();
                    mydes.Key = GetLegalKey();
                    mydes.IV = GetLegalIV();
                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                    cs.Write(bytIn, 0, bytIn.Length);
                    cs.FlushFinalBlock();
                    ms.Close();
                    byte[] bytOut = ms.ToArray();
                    return Convert.ToBase64String(bytOut);
                }
                catch (Exception ex) {
                    throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
                }
            }
            /// <summary>
            /// 解密方法
            /// </summary>
            /// <param name="Source">待解密的串</param>
            /// <returns>经过解密的串</returns>
            public string Decrypt(string Source) {
                try {
                    byte[] bytIn = Convert.FromBase64String(Source);
                    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                    mydes.Key = GetLegalKey();
                    mydes.IV = GetLegalIV();
                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    return sr.ReadToEnd();
                }
                catch (Exception ex) {
                    throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
                }
            }
            /// <summary>
            /// 加密方法byte[] to byte[]
            /// </summary>
            /// <param name="Source">待加密的byte数组</param>
            /// <returns>经过加密的byte数组</returns>
            public byte[] Encrypt(byte[] Source) {
                try {
                    byte[] bytIn = Source;
                    MemoryStream ms = new MemoryStream();
                    mydes.Key = GetLegalKey();
                    mydes.IV = GetLegalIV();
                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                    cs.Write(bytIn, 0, bytIn.Length);
                    cs.FlushFinalBlock();
                    ms.Close();
                    byte[] bytOut = ms.ToArray();
                    return bytOut;
                }
                catch (Exception ex) {
                    throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
                }
            }
            /// <summary>
            /// 解密方法byte[] to byte[]
            /// </summary>
            /// <param name="Source">待解密的byte数组</param>
            /// <returns>经过解密的byte数组</returns>
            public byte[] Decrypt(byte[] Source) {
                try {
                    byte[] bytIn = Source;
                    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                    mydes.Key = GetLegalKey();
                    mydes.IV = GetLegalIV();
                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
                }
                catch (Exception ex) {
                    throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
                }
            }


            /// <summary>
            /// 加密方法File to File
            /// </summary>
            /// <param name="inFileName">待加密文件的路径</param>
            /// <param name="outFileName">待加密后文件的输出路径</param>

            public void Encrypt(string inFileName, string outFileName) {
                try {

                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    mydes.Key = GetLegalKey();
                    mydes.IV = GetLegalIV();

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen) {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                }
                catch (Exception ex) {
                    throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
                }
            }
            /// <summary>
            /// 解密方法File to File
            /// </summary>
            /// <param name="inFileName">待解密文件的路径</param>
            /// <param name="outFileName">待解密后文件的输出路径</param>
            public void Decrypt(string inFileName, string outFileName) {
                try {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;
                    mydes.Key = GetLegalKey();
                    mydes.IV = GetLegalIV();
                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen) {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                }
                catch (Exception ex) {
                    throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
                }
            }

        }

        private class AES_ {
            /// <summary>
            /// Encrpyts the sourceString, returns this result as an Aes encrpyted, BASE64 encoded string
            /// </summary>
            /// <param name="plainSourceStringToEncrypt">a plain, Framework string (ASCII, null terminated)</param>
            /// <param name="key">The pass phrase.</param>
            /// <returns>
            /// returns an Aes encrypted, BASE64 encoded string
            /// </returns>
            public static string EncryptString(string plainSourceStringToEncrypt, string key, string iv) {
                // 处理 key
                key = (key ?? "").PadRight(16, '\0').Substring(0, 16);

                // 处理 iv
                iv = (iv ?? "").PadRight(16, '\0').Substring(0, 16);

                //Set up the encryption objects
                using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(iv))) {
                    byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                    using (ICryptoTransform ictE = acsp.CreateEncryptor()) {
                        //Set up stream to contain the encryption
                        using (MemoryStream msS = new MemoryStream()) {

                            //Perform the encrpytion, storing output into the stream
                            using (CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write)) {
                                csS.Write(sourceBytes, 0, sourceBytes.Length);
                                csS.FlushFinalBlock();

                                //sourceBytes are now encrypted as an array of secure bytes
                                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                                //return the encrypted bytes as a BASE64 encoded string
                                return Convert.ToBase64String(encryptedBytes);
                            }
                        }
                    }
                }
            }


            /// <summary>
            /// Decrypts a BASE64 encoded string of encrypted data, returns a plain string
            /// </summary>
            /// <param name="base64StringToDecrypt">an Aes encrypted AND base64 encoded string</param>
            /// <param name="key">The passphrase.</param>
            /// <returns>returns a plain string</returns>
            public static string DecryptString(string base64StringToDecrypt, string key, string iv) {
                // 处理 key
                key = (key ?? "").PadRight(16, '\0').Substring(0, 16);

                // 处理 iv
                iv = (iv ?? "").PadRight(16, '\0').Substring(0, 16);

                //Set up the encryption objects
                using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(iv))) {
                    byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                    using (ICryptoTransform ictD = acsp.CreateDecryptor()) {

                        //RawBytes now contains original byte array, still in Encrypted state

                        //Decrypt into stream
                        using (MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length)) {
                            using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read)) {
                                //csD now contains original byte array, fully decrypted

                                //return the content of msD as a regular string
                                return (new StreamReader(csD)).ReadToEnd();
                            }
                        }
                    }
                }
            }

            private static AesCryptoServiceProvider GetProvider(byte[] key, byte[] iv) {
                AesCryptoServiceProvider result = new AesCryptoServiceProvider();
                result.BlockSize = 128;
                result.KeySize = 128;
                result.Mode = CipherMode.CBC;
                result.Padding = PaddingMode.None;

                //result.IV = GetKey(iv, result);
                //result.GenerateIV();
                result.IV = iv;

                byte[] RealKey = GetKey(key, result);
                result.Key = RealKey;
                // result.IV = RealKey;
                return result;
            }

            private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p) {
                byte[] kRaw = suggestedKey;
                List<byte> kList = new List<byte>();

                for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8) {
                    kList.Add(kRaw[(i / 8) % kRaw.Length]);
                }
                byte[] k = kList.ToArray();
                return k;
            }

        }
    }
}

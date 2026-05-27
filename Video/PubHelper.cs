using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace STARDC.PubFunc
{
    /// <summary>
    ///PubFunc 的摘要说明
    /// </summary>
    public static class PubHelper
    {
        public class ComboBoxItemTextValue
        {
            public string selectText;
            public string selectValue;
            public ComboBoxItemTextValue(string _selectValue, string _selectText)
            {
                selectValue = _selectValue;
                selectText = _selectText;
            }
            public override string ToString()
            {
                return selectText;
            }
        }
        /// <summary>
        /// 返回程序运行路径
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyPath()
        {
            try
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

                codeBase = codeBase.Substring(8, codeBase.Length - 8);    // 8是 file:// 的长度

                string[] arrSection = codeBase.Split(new char[] { '/' });

                string folderPath = "";
                for (int i = 0; i < arrSection.Length - 1; i++)
                {
                    folderPath += arrSection[i] + "/";
                }

                return folderPath;
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="value"></param>
        public static void ShowDebug(object value)
        {
            Debug.Write(value);
        }
        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string strMd5(string encypStr)
        {
            try
            {
                string retStr = string.Empty;
                MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
                //创建md5对象
                byte[] inputBye;
                byte[] outputBye;
                inputBye = Encoding.GetEncoding("gb2312").GetBytes(encypStr);
                //使用ascii编码方式把字符串转化为字节数组．
                outputBye = m5.ComputeHash(inputBye);
                retStr = System.BitConverter.ToString(outputBye);
                retStr = retStr.Replace("-", "").ToUpper();
                return (retStr);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 生成加密字符串(length字符串长度)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomNumString(int length)
        {
            try
            {
                StringBuilder sbd = new StringBuilder();
                if (length <= 0)
                    return string.Empty;
                byte[] buffer = new byte[length * 4];
                RandomNumberGenerator.Create().GetBytes(buffer);
                for (int i = 0; i < length; i++)
                    sbd.Append(Math.Abs(BitConverter.ToInt32(buffer, i * 4)) % 10);

                return sbd.ToString();
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取字符串在字符串数组内的位置
        /// </summary>
        /// <param name="arrStr"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetIndexbyName(string[] arrStr, string str)
        {
            try
            {
                int Ival = -1;
                if (arrStr != null)
                {
                    for (int index = 0; index < arrStr.Length; index++)
                    {
                        string ts = arrStr[index];
                        if (str == ts)
                        {
                            Ival = index;
                            break;
                        }
                    }
                }
                return Ival;
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return -1;
            }
        }

        /// <summary>
        /// 实现对一个文件md5的读取，path为文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MD5FileTZ(string path)
        {
            try
            {
                using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
                    byte[] hash_byte = get_md5.ComputeHash(get_file);
                    string resule = BitConverter.ToString(hash_byte);
                    resule = resule.Replace("-", "");
                    return resule;
                }
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 将指定符号分割的字符串以数组给出
        /// </summary>
        /// <param name="Strs">分割字符串</param>
        /// <param name="Separator">分隔符</param>
        /// <returns></returns>
        public static string[] GetSubStrs(string Strs, string Separator)
        {
            try
            {
                return Strs.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                string[] ra = new string[0];
                return ra;
            }
        }
        /// <summary>
        /// 将指定符号分割的字符串以数组给出
        /// </summary>
        /// <param name="Strs">分割字符串</param>
        /// <param name="Separator">分隔符</param>
        /// <param name="ssOptions">空字符处理方式</param>
        /// <returns></returns>
        public static string[] GetSubStrs(string Strs, string Separator, StringSplitOptions ssOptions)
        {
            try
            {
                return Strs.Split(new string[] { Separator }, ssOptions);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                string[] ra = new string[0];
                return ra;
            }
        }
        /// <summary>
        /// 该函数用于拆分以特殊字符如‘#’,','间隔的字符串 异常返回""
        /// </summary>
        /// <param name="Strs"></param>
        /// <param name="Separator"></param>
        /// <param name="mPosition"></param>mPosition:指定的字符串序号，第一个字串的序号为0
        /// <returns></returns>
        public static string SubLongStr(string Strs, string Separator, int mPosition)
        {
            string[] Ts = Strs.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                return Ts[mPosition];
            }
            catch(Exception ex)
            {
                ShowDebug(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// AES加密 
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();//using System.Security.Cryptography;   
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;//using System.Security.Cryptography;   
            rDel.Padding = PaddingMode.PKCS7;//using System.Security.Cryptography;   

            ICryptoTransform cTransform = rDel.CreateEncryptor();//using System.Security.Cryptography;   
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static double StrToDoubleDef(string S, double Default)
        {
            try
            {
                if (S.Trim().Length == 0)
                    return Default;
                return double.Parse(S);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return Default;
            }
        }
        public static float StrToFloatDef(string S, float Default)
        {
            try
            {
                if (S.Trim().Length == 0)
                    return Default;
                return float.Parse(S);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return Default;
            }
        }
        public static int StrToIntDef(string S, int Default)
        {
            try
            {
                if (S.Trim().Length == 0)
                    return Default;
                return int.Parse(S);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return Default;
            }
        }
        public static bool StrToBoolDef(string S, bool Default)
        {
            try
            {
                if (S.Trim().Length == 0)
                    return Default;
                return bool.Parse(S);
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return Default;
            }
        }
        public static bool StrToDateTime(string S, out DateTime dateTime)
        {
            dateTime = DateTime.Now;
            try
            {
                string dtStr = S;
                if (S.Length == 0)
                    return false;
                if (S.Length == 12)
                {
                    dtStr = S.Substring(0, 2) + "-" + S.Substring(2, 2) + "-" + S.Substring(4, 2) + " ";
                    dtStr += S.Substring(6, 2) + ":" + S.Substring(8, 2) + ":" + S.Substring(10, 2);
                    dtStr = "20" + dtStr;
                }
                else if (S.Length == 14)
                {
                    dtStr = S.Substring(0, 4) + "-" + S.Substring(4, 2) + "-" + S.Substring(6, 2) + " ";
                    dtStr += S.Substring(8, 2) + ":" + S.Substring(10, 2) + ":" + S.Substring(12, 2);
                }
                else if (S.Length == 15)
                {
                    dtStr = S.Substring(0, 2) + "-" + S.Substring(2, 2) + "-" + S.Substring(4, 2) + " ";
                    dtStr += S.Substring(6, 2) + ":" + S.Substring(8, 2) + ":" + S.Substring(10, 2);
                    dtStr = "20" + dtStr;
                }
                else if (S.Length == 17)
                {
                    dtStr = S.Substring(0, 4) + "-" + S.Substring(4, 2) + "-" + S.Substring(6, 2) + " ";
                    dtStr += S.Substring(8, 2) + ":" + S.Substring(10, 2) + ":" + S.Substring(12, 2);
                }
                else if (S.Length == 8)
                {
                    dtStr = S.Substring(0, 4) + "-" + S.Substring(4, 2) + "-" + S.Substring(6, 2);
                }
                dateTime = Convert.ToDateTime(dtStr);
                return true;
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return false;
            }
        }
        public static DateTime StrToDateTimeDef(string S, DateTime Default)
        {
            try
            {
                DateTime dateTime;

                if (StrToDateTime(S, out dateTime))
                    return dateTime;
                else
                    return Default;
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return Default;
            }
        }
        /// <summary>
        /// yyMMddHHmmssfff 转换为 yy-MM-dd HH:mm:ss fff
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static string dtStrFormat(string S)
        {
            string dtStr = string.Empty;
            try
            {
                if (S.Length == 15)
                {
                    dtStr = S.Substring(0, 2) + "-" + S.Substring(2, 2) + "-" + S.Substring(4, 2) + " ";
                    dtStr += S.Substring(6, 2) + ":" + S.Substring(8, 2) + ":" + S.Substring(10, 2);
                    dtStr += " " + S.Substring(12, 3);
                }
                return dtStr;
            }
            catch (Exception ex)
            {
                ShowDebug(ex);
                return dtStr;
            }
        }
        /// <summary>
        /// Bool类型转换为string，true为1, false为1
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string BoolToStr(bool _value)
        {
            return _value ? "1" : "0";
        }
        /// <summary>
        /// 格式StrBool字符串
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string StrBoolFormat(string StrBool)
        {
            if (StrBool == "False")
                return "0";
            else if (StrBool == "True")
                return "1";
            else if (StrBool == "1")
                return "True";
            else if (StrBool == "False")
                return "0";
            else
                return "0";
        }
        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsNumeric(string number)
        {
            try
            {   
                for (int i = 0; i < number.Length; i++)
                {
                    if (!char.IsNumber(number, i))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 格式化字符串长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="Length">格式字符串长度</param>
        /// <param name="Default">前缀字符</param>
        /// <returns></returns>
        public static string FormatStrLength(string str, int Length, string Default)
        {
            if (str.Length > Length)
                str = str.Substring(str.Length - Length, Length);
            else
            {
                while (str.Length < Length)
                    str = Default + str;
            }
            return str;
        }
        /// <summary>
        /// 条形码增加校验码
        /// </summary>
        public static string MetIdAddJYM(string MetId)
        {
            string strMetId = MetId;
            if (strMetId.Length != 21)
                return strMetId;
            
            int ii = 0;
            int iC = 0;
            foreach (char cstr in strMetId)
            {

                if (ii % 2 == 0)
                    iC += (Convert.ToInt16(cstr) - 48) * 3;
                else
                    iC += (Convert.ToInt16(cstr) - 48);
                ii++;
            }
            iC = iC % 10;
            if (iC != 0)
                iC = 10 - iC;
            return strMetId + iC.ToString();
        }

       /// <summary>
        /// 将 Stream 转成 byte[]
       /// </summary>
       /// <param name="stream"></param>
       /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void Compress(Stream source, Stream dest)
        {
            using (GZipStream zipStream = new GZipStream(dest, CompressionMode.Compress, true))
            {
                byte[] buf = new byte[1024];
                int len = 0;
                while ((len = source.Read(buf, 0, buf.Length)) > 0)
                {
                    zipStream.Write(buf, 0, len);
                }
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void Decompress(Stream source, Stream dest)
        {
            using (GZipStream zipStream = new GZipStream(source, CompressionMode.Decompress, true))
            {
                byte[] buf = new byte[1024];
                int len;
                while ((len = zipStream.Read(buf, 0, buf.Length)) > 0)
                {
                    dest.Write(buf, 0, len);
                }
            }
        }
    }
}
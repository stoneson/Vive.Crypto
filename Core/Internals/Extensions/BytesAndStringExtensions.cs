using Org.BouncyCastle.Math;
using System;
using System.Globalization;
using System.Text;

namespace Vive.Crypto.Core.Internals.Extensions
{
    /// <summary>
    /// 字节数组及字符串扩展
    /// </summary>
    internal static class BytesAndStringExtensions
    {
        /// <summary>
        /// 将字节数组转换成16进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        internal static string ToHexString(this byte[] bytes)
        {
            var sb=new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取加密字符串的加密字节数组
        /// </summary>
        /// <param name="data">加密字符串</param>
        /// <param name="outType">输出类型</param>
        /// <returns></returns>
        internal static byte[] GetEncryptBytes(this string data, OutType outType)
        {
            switch (outType)
            {
                case OutType.Base64:
                    return Convert.FromBase64String(data);
                case OutType.Hex:
                    return ToBytes(data);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将16进制字符串转换成字节数组
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns></returns>
        internal static byte[] ToBytes(this string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[]{0};
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }
            byte[] result=new byte[hex.Length/2];
            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

       
        internal static byte[] GetBytes(this string strUtf8)
        {
            if (string.IsNullOrEmpty(strUtf8) || strUtf8.Length == 0)
            {
                return new byte[] { 0 };
            }

            return Encoding.UTF8.GetBytes(strUtf8);
        }
        /// <summary>
        /// 十六进制串转化为byte数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] hexToByte(this String hex)
        {
            return Org.BouncyCastle.Utilities.Encoders.Hex.Decode(hex);
        }
        /// <summary>
        /// 字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static String byteToHex(this byte[] b)
        {
            if (b == null)
            {
                throw new ArgumentException("Argument b ( byte array ) is null! ");
            }
            return Org.BouncyCastle.Utilities.Encoders.Hex.ToHexString(b);
        }
        //------------------------------------------------------------------------------------------------------------------------------
        public static byte[] AsciiBytes(string s)
        {
            byte[] bytes = new byte[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                bytes[i] = (byte)s[i];
            }

            return bytes;
        }

        public static byte[] HexToByteArray(this string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string s = hexString.Substring(i, 2);
                bytes[i / 2] = byte.Parse(s, NumberStyles.HexNumber, null);
            }

            return bytes;
        }

        public static string ByteArrayToHex(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }

        public static string ByteArrayToHex(this byte[] bytes, int len)
        {
            return ByteArrayToHex(bytes).Substring(0, len * 2);
        }

        public static byte[] RepeatByte(byte b, int count)
        {
            byte[] value = new byte[count];

            for (int i = 0; i < count; i++)
            {
                value[i] = b;
            }

            return value;
        }

        public static byte[] SubBytes(this byte[] bytes, int startIndex, int length)
        {
            byte[] res = new byte[length];
            Array.Copy(bytes, startIndex, res, 0, length);
            return res;
        }

        public static byte[] XOR(this byte[] value)
        {
            byte[] res = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                res[i] ^= value[i];
            }
            return res;
        }

        public static byte[] XOR(this byte[] valueA, byte[] valueB)
        {
            int len = valueA.Length;
            byte[] res = new byte[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = (byte)(valueA[i] ^ valueB[i]);
            }
            return res;
        }
        
        /// <summary>
        /// 整形转换成网络传输的字节流（字节数组）型数据
        /// </summary>
        /// <param name="num">一个整型数据</param>
        /// <returns>4个字节的自己数组</returns>
        public static byte[] intToBytes(this int num)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(0xff & (num >> 0));
            bytes[1] = (byte)(0xff & (num >> 8));
            bytes[2] = (byte)(0xff & (num >> 16));
            bytes[3] = (byte)(0xff & (num >> 24));
            return bytes;
        }
        /// <summary>
        /// 四个字节的字节数据转换成一个整形数据
        /// </summary>
        /// <param name="bytes">4个字节的字节数组</param>
        /// <returns>一个整型数据</returns>
        public static int byteToInt(this byte[] bytes)
        {
            int num = 0;
            int temp;
            temp = (0x000000ff & (bytes[0])) << 0;
            num = num | temp;
            temp = (0x000000ff & (bytes[1])) << 8;
            num = num | temp;
            temp = (0x000000ff & (bytes[2])) << 16;
            num = num | temp;
            temp = (0x000000ff & (bytes[3])) << 24;
            num = num | temp;
            return num;
        }
       
        /// <summary>
        /// 长整形转换成网络传输的字节流（字节数组）型数据
        /// </summary>
        /// <param name="num">一个长整型数据</param>
        /// <returns>8个字节的自己数组</returns>
        public static byte[] longToBytes(this long num)
        {
            byte[] bytes = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = (byte)(0xff & (num >> (i * 8)));
            }

            return bytes;
        }

        /// <summary>
        /// 大数字转换字节流（字节数组）型数据
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static byte[] byteConvert32Bytes(this BigInteger n)
        {
            byte[] tmpd = new byte[0];
            if (n == null)
            {
                return null;
            }

            if (n.ToByteArray().Length == 33)
            {
                tmpd = new byte[32];
                System.Array.Copy(n.ToByteArray(), 1, tmpd, 0, 32);
            }
            else if (n.ToByteArray().Length == 32)
            {
                tmpd = n.ToByteArray();
            }
            else
            {
                tmpd = new byte[32];
                for (int i = 0; i < 32 - n.ToByteArray().Length; i++)
                {
                    tmpd[i] = 0;
                }
                System.Array.Copy(n.ToByteArray(), 0, tmpd, 32 - n.ToByteArray().Length, n.ToByteArray().Length);
            }
            return tmpd;
        }
        /// <summary>
        /// 换字节流（字节数组）型数据转大数字
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger byteConvertInteger(this byte[] b)
        {
            if (b[0] < 0)
            {
                byte[] temp = new byte[b.Length + 1];
                temp[0] = 0;
                System.Array.Copy(b, 0, temp, 1, b.Length);
                return new BigInteger(temp);
            }
            return new BigInteger(b);
        }


        /// <summary>
        ///   将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象。
        /// </summary>
        /// <param name="value">要转换的枚举名称或基础值的字符串表示形式。</param>
        /// <typeparam name="TEnum">
        ///   要将 <paramref name="value" /> 转换到的枚举类型。
        /// </typeparam>
        /// <returns>
        ///   当此方法返回时，如果分析操作成功，<paramref name="value" /> 将包含值由 <paramref name="value" /> 表示的 <paramref name="TEnum" /> 类型的对象。
        ///    如果分析操作失败，<paramref name="value" /> 包括 <paramref name="TEnum" /> 的基础类型的默认值。
        ///    请注意，此值无需为 <paramref name="TEnum" /> 枚举的成员。
        ///    此参数未经初始化即被传递。
        /// </returns>
        internal static TEnum ToEnum<TEnum>(this string value) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, true, out TEnum enumStr))
            {
                return enumStr;
            }
            var ens = Enum.GetValues(typeof(TEnum));
            foreach (var en in ens)
            {
                if (en.ToString().ToLower().Contains(value.ToLower()))
                {
                    return (TEnum)en;
                }
            }
            return default(TEnum);
        }
    }
}

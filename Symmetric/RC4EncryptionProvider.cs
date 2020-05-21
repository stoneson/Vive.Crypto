using System;
using System.Linq;
using System.Text;
using HCenter.Encryption.Abstractions;
using HCenter.Encryption.Core;
using HCenter.Encryption.Core.Internals.Extensions;

// ReSharper disable once CheckNamespace
namespace HCenter.Encryption
{
    #region class RC4EncryptionProvider
    /// <summary>
    /// RC4 加密提供程序
    /// 参考：https://bitlush.com/blog/rc4-encryption-in-c-sharp
    /// </summary>
    internal class RC4EncryptionProvider : ISymmetricEncyption
    {
        /// <summary>
        /// 初始化一个<see cref="RC4EncryptionProvider"/>类型的实例
        /// </summary>
        private RC4EncryptionProvider() { }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码类型，默认Wie<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Encrypt(string value, string key, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return Convert.ToBase64String(EncryptCore(encoding.GetBytes(value), encoding.GetBytes(key)));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] value, byte[] key)
        {
            return EncryptCore(value, key);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码类型，默认Wie<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Decrypt(string value, string key, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetString(EncryptCore(Convert.FromBase64String(value), encoding.GetBytes(key)));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] value, byte[] key)
        {
            return EncryptCore(value, key);
        }

        /// <summary>
        /// 核心加密方法
        /// </summary>
        /// <param name="sourceBytes">待加密的字节数组</param>
        /// <param name="keyBytes">密钥字节数组</param>
        /// <returns></returns>
        private static byte[] EncryptCore(byte[] sourceBytes, byte[] keyBytes)
        {
            var s = Initialize(keyBytes);
            int i = 0, j = 0;

            return sourceBytes.Select(b =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;
                Swap(s, i, j);
                return (byte) (b ^ s[(s[i] + s[j]) & 255]);
            }).ToArray();
        }

        /// <summary>
        /// 初始化密钥
        /// </summary>
        /// <param name="keyBytes">密钥字节数组</param>
        /// <returns></returns>
        private static byte[] Initialize(byte[] keyBytes)
        {
            var s = Enumerable.Range(0, 256).Select(i => (byte) i).ToArray();
            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + keyBytes[i % keyBytes.Length] + s[i]) & 255;
                Swap(s, i, j);
            }

            return s;
        }

        /// <summary>
        /// 交换位置
        /// </summary>
        private static void Swap(byte[] s, int i, int j)
        {
            var b = s[i];
            s[i] = s[j];
            s[j] = b;
        }
    }
    #endregion

    #region RC4Encryption

    /// <summary>
    /// RC4 加密提供程序
    /// </summary>
    internal class RC4Encryption : AbsSymmetricProvider
    {
        /// <summary>
        /// 初始化一个<see cref="RC4Encryption"/>类型的实例
        /// </summary>
        public RC4Encryption() : this(OutType.Hex, Encoding.UTF8) { }
        /// <summary>
        /// 初始化一个<see cref="RC4Encryption"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public RC4Encryption(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <returns></returns>
        public override SymmetricKey CreateKey()
        {
            var key = TripleDESEncryptionProvider.CreateKey(TripleDESKeySizeType.L192, Encoding);
            return new SymmetricKey() { Key = key.Key, IV = key.IV };
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public override string Encrypt(string value, string key, string iv = null)
        {
            var result = RC4EncryptionProvider.Encrypt(Encoding.GetBytes(value), Encoding.GetBytes(key));
            if (OutType == OutType.Base64)
            {
                return Convert.ToBase64String(result);
            }
            return result.ToHexString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Decrypt(string value, string key, string iv = null)
        {
            var result = RC4EncryptionProvider.Decrypt(value.GetEncryptBytes(OutType), Encoding.GetBytes(key));
            return Encoding.GetString(result);
        }

    }
    #endregion
}

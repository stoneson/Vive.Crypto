using System;
using System.Text;
using Vive.Crypto.Core;
using Vive.Crypto.Core.Internals.Extensions;

// ReSharper disable once CheckNamespace
namespace Vive.Crypto
{
    /// <summary>
    /// MD4 哈希加密提供程序
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal static class MD4HashingProvider
    {
        /// <summary>
        /// 获取字符串的 MD4 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Signature(string value, Encoding encoding = null)
        {
            return SignatureHash(value, encoding).ToHexString();
        }

        /// <summary>
        /// 获取字符串的 MD4 哈希值
        /// </summary>
        /// <param name="value">待加密的字节数组</param>
        /// <returns></returns>
        public static string Signature(byte[] value)
        {
            return Core(value).ToHexString();
        }

        /// <summary>
        /// 获取字节数组的 MD4 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static byte[] SignatureHash(string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return Core(encoding.GetBytes(value));
        }

        /// <summary>
        /// 获取字节数组的 MD4 哈希值
        /// </summary>
        /// <param name="value">待加密的字节数组</param>
        /// <returns></returns>
        public static byte[] SignatureHash(byte[] value)
        {
            return Core(value);
        }

        /// <summary>
        /// 核心加密算法
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <returns></returns>
        private static byte[] Core(byte[] value)
        {
            using (var md4 = new MD4CryptoServiceProvider())
            {
                return md4.ComputeHash(value);
            }
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="comparison">对比的值</param>
        /// <param name="value">待加密的值</param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static bool Verify(string comparison, string value, Encoding encoding = null) =>
            comparison == Signature(value, encoding);
    }

    /// <summary>
    /// MD4 哈希加密提供程序
    /// </summary>
    internal class MD4Hashing : AbsHashingProvider
    {
        /// <summary>
        /// 初始化一个<see cref="MD4Hashing"/>类型的实例
        /// </summary>
        public MD4Hashing() : this(OutType.Hex, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 初始化一个<see cref="MD4Hashing"/>类型的实例
        public MD4Hashing(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 获取字符串的 MD4 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥(MD4为空)</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Hex"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Signature(string value, string key = "")
        {
            var result = MD4HashingProvider.SignatureHash(value, Encoding);
            if (OutType == OutType.Base64)
            {
                return Convert.ToBase64String(result);
            }
            return result.ToHexString();
        }
    }
}

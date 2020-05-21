using System;
using System.Security.Cryptography;
using System.Text;
using HCenter.Encryption.Core;
using HCenter.Encryption.Core.Internals.Extensions;
using HCenter.Encryption.Hash;

// ReSharper disable once CheckNamespace
namespace HCenter.Encryption
{
    /// <summary>
    /// MD5 哈希加密提供程序
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal static class MD5HasingProvider
    {
        /// <summary>
        /// 获取字符串的 MD5 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="bitType">MD5加密类型，默认为<see cref="MD5BitType.L32"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Signature(string value, MD5BitType bitType = MD5BitType.L32, Encoding encoding = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            switch (bitType)
            {
                case MD5BitType.L16:
                    return Encrypt16Func()(value)(encoding);
                case MD5BitType.L32:
                    return Encrypt32Func()(value)(encoding);
                case MD5BitType.L64:
                    return Encrypt64Func()(value)(encoding);
                default:
                    throw new ArgumentOutOfRangeException(nameof(bitType), bitType, null);
            }
        }

        /// <summary>
        /// 预加密
        /// </summary>
        /// <returns></returns>
        private static Func<string, Func<Encoding, byte[]>> PreencryptFunc() => str => encoding =>
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(encoding.GetBytes(str));
            }
        };

        /// <summary>
        /// 16位加密
        /// </summary>
        /// <returns></returns>
        private static Func<string, Func<Encoding, string>> Encrypt16Func() => str =>
            encoding => BitConverter.ToString(PreencryptFunc()(str)(encoding), 4, 8).Replace("-", "");

        /// <summary>
        /// 32位加密
        /// </summary>
        /// <returns></returns>
        private static Func<string, Func<Encoding, string>> Encrypt32Func() => str => encoding =>
        {
            var bytes = PreencryptFunc()(str)(encoding);
            return bytes.ToHexString();
        };

        /// <summary>
        /// 64位加密
        /// </summary>
        /// <returns></returns>
        private static Func<string, Func<Encoding, string>> Encrypt64Func() => str =>
            encoding => Convert.ToBase64String(PreencryptFunc()(str)(encoding));

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="comparison">对比的值</param>
        /// <param name="value">待加密的值</param>
        /// <param name="bitType">MD5加密类型，默认为<see cref="MD5BitType.L32"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static bool Verify(string comparison, string value, MD5BitType bitType = MD5BitType.L32,
            Encoding encoding = null) => comparison == Signature(value, bitType, encoding);
    }

    /// <summary>
    /// MD5 哈希加密提供程序
    /// </summary>
    internal class MD5Hasing : AbsHashingProvider
    {
        /// <summary>
        /// 初始化一个<see cref="MD5Hasing"/>类型的实例
        /// </summary>
        public MD5Hasing() : this(OutType.Hex, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 初始化一个<see cref="MD5Hasing"/>类型的实例
        public MD5Hasing(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 获取字符串的 MD5 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥(MD5为空)</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Hex"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Signature(string value, string key="")
        {
            switch (OutType)
            {
                case OutType.Hex:
                    return MD5HasingProvider.Signature(value, MD5BitType.L32, Encoding);
                case OutType.Base64:
                    return MD5HasingProvider.Signature(value, MD5BitType.L64, Encoding);
                default:
                    return MD5HasingProvider.Signature(value, MD5BitType.L64, Encoding);
            }
        }
    }
}

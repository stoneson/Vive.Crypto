using System.Security.Cryptography;
using System.Text;
using Vive.Crypto.Core;

// ReSharper disable once CheckNamespace
namespace Vive.Crypto
{
    /// <summary>
    /// SHA384 哈希加密提供程序
    /// </summary>
    internal sealed class SHA384HashingProvider : SHAHashingBase
    {
        /// <summary>
        /// 初始化一个<see cref="SHA384HashingProvider"/>类型的实例
        /// </summary>
        private SHA384HashingProvider() { }

        /// <summary>
        /// 获取字符串的 SHA384 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Hex"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Signature(string value, OutType outType = OutType.Hex, Encoding encoding = null) =>
            Encrypt<SHA384CryptoServiceProvider>(value, encoding, outType);

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="comparison">对比的值</param>
        /// <param name="value">待加密的值</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Hex"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static bool Verify(string comparison, string value, OutType outType = OutType.Hex,
            Encoding encoding = null) => comparison == Signature(value, outType, encoding);
    }

    /// <summary>
    /// SHA384 哈希加密提供程序
    /// </summary>
    internal class SHA384Hashing : AbsHashingProvider
    {
        /// <summary>
        /// 初始化一个<see cref="SHA384Hashing"/>类型的实例
        /// </summary>
        public SHA384Hashing() : this(OutType.Hex, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 初始化一个<see cref="SHA384Hashing"/>类型的实例
        public SHA384Hashing(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 获取字符串的 SHA384 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥(SHA为空)</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Hex"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Signature(string value, string key = "") => SHA384HashingProvider.Signature(value, OutType, Encoding);

    }
}

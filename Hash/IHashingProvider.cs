using Vive.Crypto.Core;
using Vive.Crypto.Core.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto
{
    /// <summary>
    /// 哈希加密提供程序
    /// </summary>
    public interface IHashingProvider
    {
        /// <summary>
        /// 输出类型，默认为<see cref="OutType.Hex"/>
        /// </summary>
        OutType OutType { get; set; }
        /// <summary>
        /// 编码类型，默认为<see cref="Encoding.UTF8"/>
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// 获取字符串的 HMAC_SHA256 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        string Signature(string value, string key);

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="comparison">对比的值</param>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        bool Verify(string comparison, string value, string key);
    }
    /// <summary>
    /// 哈希加密类型
    /// </summary>
    public enum HashingProviderType
    {
        HMACMD5 = 1,
        HMACSHA1 = 2,
        HMACSHA256 = 3,
        HMACSHA384 = 4,
        HMACSHA512 = 5,
        MD4 = 6,
        MD5 = 7,
        SHA1 = 8,
        SHA256 = 9,
        SHA384 = 10,
        SHA512 = 11,
        SM3 = 12,
    }
    /// <summary>
    /// 哈希加密工厂类
    /// </summary>
    internal sealed class HashingProviderFactory
    {
        public static IHashingProvider Create(string providerTypestr = "SHA256")
        {
            var providerType = providerTypestr.ToEnum<HashingProviderType>();
            return Create(providerType);
        }
        public static IHashingProvider Create(HashingProviderType providerType = HashingProviderType.SHA256)
        {
            switch (providerType)
            {
                case HashingProviderType.HMACMD5:
                    return new HMACMD5Hashing();
                case HashingProviderType.HMACSHA1:
                    return new HMACSHA1Hashing();
                case HashingProviderType.HMACSHA256:
                    return new HMACSHA256Hashing();
                case HashingProviderType.HMACSHA384:
                    return new HMACSHA384Hashing();
                case HashingProviderType.HMACSHA512:
                    return new HMACSHA512Hashing();
                case HashingProviderType.MD4:
                    return new MD4Hashing();
                case HashingProviderType.MD5:
                    return new MD5Hasing();
                case HashingProviderType.SHA1:
                    return new SHA1Hashing();
                case HashingProviderType.SHA256:
                    return new SHA256Hashing();
                case HashingProviderType.SHA384:
                    return new SHA384Hashing();
                case HashingProviderType.SHA512:
                    return new SHA512Hashing();
                case HashingProviderType.SM3:
                    return new SM3Hashing();
                default:
                    return new SHA256Hashing();
            }
        }
    }

    /// <summary>
    /// 一个抽象哈希加密提供程序
    /// </summary>
    internal abstract class AbsHashingProvider : IHashingProvider
    {
        public OutType OutType { get; set; }
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 初始化一个<see cref="AbsHashingProvider"/>类型的实例
        /// </summary>
        public AbsHashingProvider(OutType outType = OutType.Hex, Encoding encoding = null)
        {
            OutType = outType;
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            Encoding = encoding;
        }

        /// <summary>
        /// 获取字符串的哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥(SM3,MD4,MD5,SHA 为空)</param>
        /// <returns></returns>
        public abstract string Signature(string value, string key = "");

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="comparison">对比的值</param>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public virtual bool Verify(string comparison, string value, string key) => comparison == Signature(value, key);
    }

    /// <summary>
    /// SM3 哈希加密提供程序
    /// </summary>
    internal class SM3Hashing : AbsHashingProvider
    {
        /// <summary>
        /// 初始化一个<see cref="SM3Hashing"/>类型的实例
        /// </summary>
        public SM3Hashing() : this(OutType.Hex, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 初始化一个<see cref="SM3Hashing"/>类型的实例
        public SM3Hashing(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 获取字符串的 SM3 哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥(SM3为空)</param>
        /// <returns></returns>
        public override string Signature(string value, string key = "")
        {
            return SMCrypto.SM3.Hash(value);
        }
    }
}

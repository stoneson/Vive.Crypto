using System;
using System.Collections.Generic;
using System.Text;
using Vive.Crypto.Core;

namespace Vive.Crypto
{

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

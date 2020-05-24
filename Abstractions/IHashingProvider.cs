using Vive.Crypto.Core;
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

}

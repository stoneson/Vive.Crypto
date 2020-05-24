using Vive.Crypto.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto
{
    /// <summary>
    /// 对称密钥
    /// </summary>
    public class SymmetricKey
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public string IV { get; set; }
    }

    /// <summary>
    /// 对称加密提供程序
    /// </summary>
    public interface ISymmetricProvider
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
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        SymmetricKey CreateKey();

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        string Encrypt(string value, string key, string iv = null);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        string Decrypt(string value, string key, string iv = null);
    }
   
}

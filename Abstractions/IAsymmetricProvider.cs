using Vive.Crypto.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto
{
    /// <summary>
    /// 非对称密钥
    /// </summary>
    public class AsymmetricKey
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublickKey { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
    }

    /// <summary>
    /// 非对称加密提供程序
    /// </summary>
    public interface IAsymmetricProvider
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
        /// 密钥类型
        /// </summary>
        RSAKeyType KeyType { get; set; }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="size">RSA 密钥长度类型</param>
        /// <returns></returns>
        AsymmetricKey CreateKey(RSAKeySizeType size = RSAKeySizeType.L2048);

        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        /// <param name="value">要加密的明文字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        string Encrypt(string value, string publicKey);


        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        /// <param name="value">要解密的密文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        string Decrypt(string value, string privateKey);

        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        string SignData(string source, string privateKey);

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        bool VerifyData(string source, string signData, string publicKey);
    }
}

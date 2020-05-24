using System;
using System.Collections.Generic;
using System.Text;
using Vive.Crypto.Core;

namespace Vive.Crypto
{

    /// <summary>
    /// 一个抽象非对称加密提供程序
    /// </summary>
    internal abstract class AbsAsymmetricProvider : IAsymmetricProvider
    {
        public OutType OutType { get; set; }
        public Encoding Encoding { get; set; }
        public RSAKeyType KeyType { get; set; }

        /// <summary>
        /// 初始化一个<see cref="AbsAsymmetricProvider"/>类型的实例
        /// </summary>
        public AbsAsymmetricProvider(OutType outType = OutType.Hex, RSAKeyType keyType = RSAKeyType.Xml, Encoding encoding = null)
        {
            OutType = outType;
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            Encoding = encoding;
            KeyType = keyType;
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <returns></returns>
        public abstract AsymmetricKey CreateKey(RSAKeySizeType size = RSAKeySizeType.L2048);
        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        /// <param name="value">要加密的明文字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public abstract string Encrypt(string value, string publicKey);

        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        /// <param name="value">要解密的密文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public abstract string Decrypt(string value, string privateKey);

        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns> $"{r}|{s}"</returns>
        public abstract string SignData(string source, string privateKey);

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public abstract bool VerifyData(string source, string signData, string publicKey);
    }

    #region SM2Encryption

    /// <summary>
    /// SM2 加密提供程序
    /// </summary>
    internal class SM2Encryption : AbsAsymmetricProvider
    {
        public SM2Encryption() : this(OutType.Hex, RSAKeyType.Xml, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 初始化一个<see cref="SM2Encryption"/>类型的实例
        /// </summary>
        public SM2Encryption(OutType outType = OutType.Hex, RSAKeyType keyType = RSAKeyType.Xml, Encoding encoding = null) : base(outType, keyType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <returns></returns>
        public override AsymmetricKey CreateKey(RSAKeySizeType size = RSAKeySizeType.L2048)
        {
            // 私钥  
            string prik = "";
            // 公钥  
            string pubk = "";
            SMCrypto.SM2.GenerateKeyPair(out prik, out pubk); //获取公钥 私钥
            return new AsymmetricKey() { PrivateKey = prik, PublickKey = pubk };
        }
        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        /// <param name="value">要加密的明文字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public override string Encrypt(string value, string publicKey)
        {
            return SMCrypto.SM2.Encrypt(publicKey, value);
        }

        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        /// <param name="value">要解密的密文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public override string Decrypt(string value, string privateKey)
        {
            return Encoding.UTF8.GetString(SMCrypto.SM2.Decrypt(privateKey, value));
        }

        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns> $"{r}|{s}"</returns>
        public override string SignData(string source, string privateKey)
        {
            return SMCrypto.SM2.Sm2Sign(source, privateKey, KeyType == RSAKeyType.Xml);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public override bool VerifyData(string source, string signData, string publicKey)
        {
            return SMCrypto.SM2.Verify(source, signData, publicKey, KeyType == RSAKeyType.Xml);
        }
    }
    #endregion
}
